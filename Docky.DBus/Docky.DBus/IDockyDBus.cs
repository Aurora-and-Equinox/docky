//  
//  Copyright (C) 2009 Jason Smith
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

using org.freedesktop.DBus;
using NDesk.DBus;

namespace Docky.DBus
{
	public delegate void ItemChangedHandler (string path);

	[Interface("org.gnome.Docky")]
	public interface IDockyDBus
	{
		event Action ShuttingDown;
		
		event ItemChangedHandler ItemAdded;
		
		event ItemChangedHandler ItemRemoved;
		
		string[] DockItemPaths ();
		
		string DockItemPathForDesktopID (string id);
		
		string DockItemPathForDesktopFile (string path);
		
		string DockItemPathForWindowXID (uint xid);
		
		void ShowAbout ();
		
		void ShowSettings ();
		
		void Quit ();
	}
}