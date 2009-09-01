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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Cairo;
using Gdk;
using Gtk;

using Docky;
using Docky.Items;

namespace Docky.Interface
{


	public class Dock : IDisposable
	{
		DockPreferences prefs;
		
		public IDockPreferences Preferences {
			get { return prefs as IDockPreferences; }
		}
		
		public Gtk.Widget PreferencesWidget { 
			get { return Preferences as Gtk.Widget; }
		}
		
		public Dock (DockPreferences prefs)
		{
			this.prefs = prefs;
		}
		
		#region IDisposable implementation
		public void Dispose ()
		{
			prefs.Destroy ();
			prefs.Dispose ();
		}
		#endregion

	}
}
