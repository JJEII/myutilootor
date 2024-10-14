/*
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace myutilootor.src
{
#if SERIALIZABLE_EXCEPTIONS
    [Serializable] // OBSOLETE AS OF .NET 8.0+
#endif
    internal class MyException : Exception
    {
        internal int line;
        internal MyException() {
        }
        internal MyException(string message) : base(message) {
        }
        internal MyException(string message, int line) : base(message) {
            this.line = line;
        }
        internal MyException(string message, Exception inner) : base(message, inner) {
        }

        // OBSOLETE AS OF .NET 8.0+ :
#if SERIALIZABLE_EXCEPTIONS
        protected MyException(
            System.Runtime.Serialization.SerializationInfo Info,
            System.Runtime.Serialization.StreamingContext context) : base(Info, context) {
        }
#endif
    }
}
