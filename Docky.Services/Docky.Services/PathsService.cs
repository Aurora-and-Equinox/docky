//  
//  Copyright (C) 2010 Chris Szikszoy
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
using System;

using GLib;

namespace Docky.Services
{
	public class PathsService
	{

		public File SystemDataFolder {
			get { return FileFactory.NewForPath (AssemblyInfo.DataDirectory).GetChild ("docky"); }
		}

		public File UserDataFolder {
			get { return FileFactory.NewForPath (Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData)).GetChild ("docky"); }
		}
		
		public File AutoStartFile {
			get { return FileFactory.NewForPath (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData)).GetChild ("autostart").GetChild ("docky.desktop"); }
		}
	}
}

