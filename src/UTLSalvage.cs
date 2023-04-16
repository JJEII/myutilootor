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
	class UTLSalvage {
		internal E.Salvage type;
		internal string combo;
		internal string? value;

		internal UTLSalvage() {
			type = E.Salvage.V_Agate;
			combo = "";
			value = null;
		}

		internal UTLSalvage(UTLSalvage s) {
			type = s.type;
			combo = s.combo;
			value = s.value;
        }
		internal UTLSalvage(E.Salvage t, string c) {
			type = t;
			combo = c;
			value = null;
		}
		internal UTLSalvage(E.Salvage t, string c, string? v) {
			type = t;
			combo = c;
			value = v;
		}
	}
}
