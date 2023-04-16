# myutilootor v.0.0.0.4
myutilootor is a metaf-like utl editor in an alternate format from that used by the "ClassicLooter" in the VirindiTank addon to the MMORPG game Asheron's Call. myutilootor provides
full feature coverage for editing loot files, and very straightforward bidirectional translation between .utl and .mut, with VirindiTank still running the end results.
Requires .NET Core. Notepad++ strongly recommended.

If you've used metaf, myutilootor should feel somewhat familiar. (See metaf's help file for getting myutilootor similarly set up in Notepad++.) One item of particular note is that
myutilootor (by default) automatically converts to utl only those rules that are active, culling all inactive rules from the results. (Use a terminal "-keep-inactive" command-line
option to suppress this behavior.)

_**The necessary files to run myutilootor are in bin/Debug/net7.0. Read the text file myutilootorREADME.mut that's in there.**_
