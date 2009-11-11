//  
// Copyright (C) 2009 Robert Dyer
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;

using Gtk;

namespace WeatherDocklet
{
	public class WeatherConfigurationDialog : Dialog
	{
		public static WeatherConfigurationDialog instance;
		
		public WeatherConfigurationDialog ()
		{
			SkipTaskbarHint = true;
			TypeHint = Gdk.WindowTypeHint.Dialog;
			WindowPosition = Gtk.WindowPosition.Center;
			KeepAbove = true;
			Stick ();
			
			IconName = Gtk.Stock.Preferences;
			
			WeatherConfiguration config = new WeatherConfiguration ();
			
			VBox.PackEnd (config);
			VBox.ShowAll ();
			
			AddButton ("Close", ResponseType.Close);
			SetDefaultSize (350, 400);
			Title = "Weather Configuration";
		}
		
		protected override void OnResponse (ResponseType response_id)
		{
			Hide ();
		}
	}
}
