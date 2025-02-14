﻿// This code was pulled from https://github.com/Taritsyn/JSMin.NET
// ---------------------------------------------------------------------------

/* This is a .NET port of the Douglas Crockford's JSMin 'C' project.
 * The author's copyright message is reproduced below.
 */

/* jsmin.c
   2019-10-30

Copyright (C) 2002 Douglas Crockford  (www.crockford.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

The Software shall be used for Good, not Evil.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.IO;
using System.Text;

using DouglasCrockford.JsMin.Utilities;

namespace DouglasCrockford.JsMin
{
	/// <summary>
	/// The JavaScript Minifier
	/// </summary>
	public sealed class JsMinifier
	{
		/// <summary>
		/// Average compression ratio
		/// </summary>
		const double AVERAGE_COMPRESSION_RATIO = 0.6;

		const int EOF = -1;

		private StringReader? _reader;
		private StringWriter? _writer;

		private int _theA;
		private int _theB;
		private int _lookAhead = EOF;
		private int _theX = EOF;
		private int _theY = EOF;

		/// <summary>
		/// Synchronizer of minification
		/// </summary>
		private readonly object _minificationSynchronizer = new object();


		/// <summary>
		/// Gets a estimated length of minififation output
		/// </summary>
		/// <param name="content">JavaScript content</param>
		/// <returns>Estimated length of minififation output</returns>
		[Obsolete("Use a length of input string or any other suitable value")]
		public static int GetEstimatedOutputLength(string content)
		{
			return (int)Math.Floor(content.Length * AVERAGE_COMPRESSION_RATIO);
		}

		/// <summary>
		/// Removes a comments and unnecessary whitespace from JavaScript code
		/// </summary>
		/// <param name="content">JavaScript content</param>
		/// <returns>Minified JavaScript content</returns>
		public string Minify(string content)
		{
			if (content == null)
			{
				throw new ArgumentNullException(nameof(content));
			}

			if (string.IsNullOrWhiteSpace(content))
			{
				return string.Empty;
			}

			var outputBuilder = new StringBuilder(content.Length);
			MinifyInternal(content, outputBuilder);

			string minifiedContent = outputBuilder.ToString();
			outputBuilder.Clear();

			return minifiedContent;
		}

		/// <summary>
		/// Removes a comments and unnecessary whitespace from JavaScript code
		/// </summary>
		/// <param name="content">JavaScript content</param>
		/// <param name="outputBuilder">String builder to which to send minification output</param>
		public void Minify(string content, StringBuilder outputBuilder)
		{
			if (content == null)
			{
				throw new ArgumentNullException(nameof(content));
			}

			if (outputBuilder == null)
			{
				throw new ArgumentNullException(nameof(outputBuilder));
			}

			if (string.IsNullOrWhiteSpace(content))
			{
				return;
			}

			MinifyInternal(content, outputBuilder);
		}

		private void MinifyInternal(string content, StringBuilder outputBuilder)
		{
			lock (_minificationSynchronizer)
			{
				_theA = 0;
				_theB = 0;
				_lookAhead = EOF;
				_theX = EOF;
				_theY = EOF;

				_reader = new StringReader(content);
				_writer = new StringWriter(outputBuilder);

				try
				{
					StartMinification();
					_writer.Flush();
					outputBuilder.TrimStart();
				}
				catch (JsMinificationException)
				{
					outputBuilder.Clear();
					throw;
				}
				finally
				{
					_reader.Dispose();
					_reader = null;

					_writer.Dispose();
					_writer = null;
				}
			}
		}

		/// <summary>
		/// Returns a true if the character is a letter, digit, underscore, dollar sign, or non-ASCII character
		/// </summary>
		/// <param name="codeunit">The character</param>
		/// <returns>Result of check</returns>
		private static bool IsAlphanum(int codeunit)
		{
			return (
				(codeunit >= 'a' && codeunit <= 'z')
				|| (codeunit >= '0' && codeunit <= '9')
				|| (codeunit >= 'A' && codeunit <= 'Z')
				|| codeunit == '_'
				|| codeunit == '$'
				|| codeunit == '\\'
				|| codeunit > 126
			);
		}

		/// <summary>
		/// Returns a next character from input stream. Watch out for lookahead.
		/// If the character is a control character, translate it to a space or linefeed.
		/// </summary>
		/// <returns>The character</returns>
		private int Get()
		{
			int codeunit = _lookAhead;
			_lookAhead = EOF;

			if (codeunit == EOF)
			{
				codeunit = _reader!.Read();
			}

			if (codeunit >= ' ' || codeunit == '\n' || codeunit == EOF)
			{
				return codeunit;
			}

			if (codeunit == '\r')
			{
				return '\n';
			}

			return ' ';
		}

		/// <summary>
		/// Gets a next character without advancing
		/// </summary>
		/// <returns>The character</returns>
		private int Peek()
		{
			_lookAhead = Get();

			return _lookAhead;
		}

		/// <summary>
		/// Gets a next character, excluding comments.
		/// <code>Peek()</code> is used to see if a '/' is followed by a '/' or '*'.
		/// </summary>
		/// <returns>The character</returns>
		private int Next()
		{
			int codeunit = Get();

			if (codeunit == '/')
			{
				switch (Peek())
				{
					case '/':
						for (;;)
						{
							codeunit = Get();

							if (codeunit <= '\n')
							{
								break;
							}
						}

						break;
					case '*':
						Get();

						while (codeunit != ' ')
						{
							switch (Get())
							{
								case '*':
									if (Peek() == '/')
									{
										Get();
										codeunit = ' ';
									}

									break;
								case EOF:
									throw new JsMinificationException("Unterminated comment.");
							}
						}

						break;
				}
			}

			_theY = _theX;
			_theX = codeunit;

			return codeunit;
		}

		/// <summary>
		/// Do something! What you do is determined by the argument:
		///		1 - Output A. Copy B to A. Get the next B.
		///		2 - Copy B to A. Get the next B. (Delete A).
		///		3 - Get the next B. (Delete B).
		/// <code>Action</code> treats a string as a single character.
		/// <code>Action</code> recognizes a regular expression if it is preceded by the likes of
		/// <code>(</code> or <code>,</code> or <code>=</code>.
		/// </summary>
		/// <param name="determined">Action type</param>
		private void Action(int determined)
		{
			if (determined == 1)
			{
				Put(_theA);

				if (
					(_theY == '\n' || _theY == ' ')
					&& (_theA == '+' || _theA == '-' || _theA == '*' || _theA == '/')
					&& (_theB == '+' || _theB == '-' || _theB == '*' || _theB == '/')
				)
				{
					Put(_theY);
				}
			}

			if (determined <= 2)
			{
				_theA = _theB;

				if (_theA == '\'' || _theA == '"' || _theA == '`')
				{
					for (;;)
					{
						Put(_theA);
						_theA = Get();

						if (_theA == _theB)
						{
							break;
						}

						if (_theA == '\\')
						{
							Put(_theA);
							_theA = Get();
						}

						if (_theA == EOF)
						{
							throw new JsMinificationException("Unterminated string literal.");
						}
					}
				}
			}

			if (determined <= 3)
			{
				_theB = Next();
				if (_theB == '/' && (
					_theA == '(' || _theA == ',' || _theA == '=' || _theA == ':'
					|| _theA == '[' || _theA == '!' || _theA == '&' || _theA == '|'
					|| _theA == '?' || _theA == '+' || _theA == '-' || _theA == '~'
					|| _theA == '*' || _theA == '/' || _theA == '{' || _theA == '}'
					|| _theA == ';'
				))
				{
					Put(_theA);

					if (_theA == '/' || _theA == '*')
					{
						Put(' ');
					}

					Put(_theB);

					for (;;)
					{
						_theA = Get();

						if (_theA == '[')
						{
							for (;;)
							{
								Put(_theA);
								_theA = Get();

								if (_theA == ']')
								{
									break;
								}

								if (_theA == '\\')
								{
									Put(_theA);
									_theA = Get();
								}

								if (_theA == EOF)
								{
									throw new JsMinificationException("Unterminated set in Regular Expression literal.");
								}
							}
						}
						else if (_theA == '/')
						{
							switch (Peek())
							{
								case '/':
								case '*':
									throw new JsMinificationException("Unterminated set in Regular Expression literal.");
							}

							break;
						}
						else if (_theA == '\\')
						{
							Put(_theA);
							_theA = Get();
						}

						if (_theA == EOF) {
							throw new JsMinificationException("Unterminated Regular Expression literal.");
						}

						Put(_theA);
					}

					_theB = Next();
				}
			}
		}

		/// <summary>
		/// Copies a input to the output, deleting the characters which are insignificant to JavaScript.
		/// Comments will be removed. Tabs will be replaced with spaces.
		/// Carriage returns will be replaced with linefeeds. Most spaces and linefeeds will be removed.
		/// </summary>
		private void StartMinification()
		{
			if (Peek() == 0xEF)
			{
				Get();
				Get();
				Get();
			}

			_theA = '\n';
			Action(3);

			while (_theA != EOF)
			{
				switch (_theA)
				{
					case ' ':
						Action(
							IsAlphanum(_theB)
							? 1
							: 2
						);
						break;
					case '\n':
						switch (_theB)
						{
							case '{':
							case '[':
							case '(':
							case '+':
							case '-':
							case '!':
							case '~':
								Action(1);
								break;
							case ' ':
								Action(3);
								break;
							default:
								Action(
									IsAlphanum(_theB)
									? 1
									: 2
								);
								break;
						}

						break;
					default:
						switch (_theB)
						{
							case ' ':
								Action(
									IsAlphanum(_theA)
									? 1
									: 3
								);
								break;
							case '\n':
								switch (_theA)
								{
									case '}':
									case ']':
									case ')':
									case '+':
									case '-':
									case '"':
									case '\'':
									case '`':
										Action(1);
										break;
									default:
										Action(
											IsAlphanum(_theA)
											? 1
											: 3
										);
										break;
								}

								break;
							default:
								Action(1);
								break;
						}

						break;
				}
			}
		}

		#region Methods for substitution methods of the C language

		/// <summary>
		/// Puts a character to output stream
		/// </summary>
		/// <param name="c">The character</param>
		private void Put(int c)
		{
			_writer!.Write((char)c);
		}

		#endregion
	}
}