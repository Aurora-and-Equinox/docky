//  
//  Copyright (C) 2009 GNOME Do
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

using Cairo;
using Gtk;
using Mono.Unix;

using Docky.CairoHelper;
using Docky.Painters;
using Docky.Services;

namespace WeatherDocklet
{
	/// <summary>
	/// A painter that displays information about weather forecasts.
	/// </summary>
	public class WeatherPainter : PagingDockPainter
	{
		/// <value>
		/// The color to draw most text in.
		/// </value>
		static readonly Cairo.Color colorTitle = new Cairo.Color (0.627, 0.627, 0.627, 1);
		
		/// <value>
		/// The color to draw high temperatures in.
		/// </value>
		static readonly Cairo.Color colorHigh = new Cairo.Color (0.945, 0.431, 0.431, 1);
		
		/// <value>
		/// The color to draw low temperatures in.
		/// </value>
		static readonly Cairo.Color colorLow = new Cairo.Color (0.427, 0.714, 0.945, 1);
		
		/// <summary>
		/// Creates a new weather painter object.
		/// </summary>
		public WeatherPainter () : base (3)
		{
		}
		
		public override int MinimumHeight {
			get {
				return 42;
			}
		}
		
		public override int MinimumWidth {
			get {
				return 2 * BUTTON_SIZE + 2 * WeatherController.Weather.ForecastDays * Math.Max (MinimumHeight, Allocation.Height);
			}
		}
		
		#region IDockPainter implementation 
		
		protected override void DrawPageOnSurface (int page, DockySurface surface)
		{
			switch (page)
			{
				default:
				case 0:
					DrawForecast (surface.Context);
					break;
				
				case 1:
					DrawTempGraph (surface.Context);
					break;
				
				case 2:
					DrawCurrentCondition (surface.Context);
					break;
			}
		}
		
		#endregion
		
		/// <summary>
		/// Paints an overview of the forecast including high/low temps and a condition icon.
		/// </summary>
		/// <param name="cr">
		/// A <see cref="Cairo.Context"/> to do the painting.
		/// </param>
		void DrawForecast (Cairo.Context cr)
		{
			int xOffset = BUTTON_SIZE;
			
			Pango.Layout layout = DockServices.Drawing.ThemedPangoLayout ();
			Pango.Rectangle inkRect, logicalRect;
			
			layout.FontDescription = new Gtk.Style().FontDescription;
			layout.FontDescription.Weight = Pango.Weight.Bold;
			layout.Ellipsize = Pango.EllipsizeMode.None;
			layout.Width = Pango.Units.FromPixels (Allocation.Height);
			
			for (int day = 0; day < WeatherController.Weather.ForecastDays; day++)
			{
				layout.FontDescription.AbsoluteSize = Pango.Units.FromPixels ((int) (Allocation.Height / 5));
				
				cr.Color = colorTitle;
				layout.SetText (string.Format ("{0}", WeatherForecast.DayShortName (WeatherController.Weather.Forecasts [day].dow)));
				layout.GetPixelExtents (out inkRect, out logicalRect);
				cr.MoveTo (xOffset + (Allocation.Height - inkRect.Width) / 2, 0);
				Pango.CairoHelper.LayoutPath (cr, layout);
				cr.Fill ();
				
				cr.Color = colorHigh;
				layout.SetText (string.Format ("{0}{1}", WeatherController.Weather.Forecasts [day].high, WeatherUnits.TempUnit));
				layout.GetPixelExtents (out inkRect, out logicalRect);
				cr.MoveTo (xOffset + (Allocation.Height - inkRect.Width) / 2, Allocation.Height / 2 - logicalRect.Height / 2);
				Pango.CairoHelper.LayoutPath (cr, layout);
				cr.Fill ();
				
				cr.Color = colorLow;
				layout.SetText (string.Format ("{0}{1}", WeatherController.Weather.Forecasts [day].low, WeatherUnits.TempUnit));
				layout.GetPixelExtents (out inkRect, out logicalRect);
				cr.MoveTo (xOffset + (Allocation.Height - inkRect.Width) / 2, Allocation.Height - logicalRect.Height);
				Pango.CairoHelper.LayoutPath (cr, layout);
				cr.Fill ();
				
				using (Gdk.Pixbuf pbuf = DockServices.Drawing.LoadIcon (WeatherController.Weather.Forecasts [day].image, Allocation.Height - 5))
				{
					Gdk.CairoHelper.SetSourcePixbuf (cr, pbuf, xOffset + Allocation.Height + 2, 5);
					cr.PaintWithAlpha (WeatherController.Weather.Forecasts [day].chanceOf ? .6 : 1);
				}
				
				if (WeatherController.Weather.Forecasts [day].chanceOf)
				{
					layout.FontDescription.AbsoluteSize = Pango.Units.FromPixels ((int) (Allocation.Height / 2));
					
					layout.SetText ("?");
					
					layout.GetPixelExtents (out inkRect, out logicalRect);
					cr.MoveTo (xOffset + Allocation.Height + (Allocation.Height - inkRect.Width) / 2, Allocation.Height / 2 - logicalRect.Height / 2);
					
					cr.LineWidth = 4;
					cr.Color = new Cairo.Color (0, 0, 0, 0.3);
					Pango.CairoHelper.LayoutPath (cr, layout);
					cr.StrokePreserve ();
					
					cr.Color = new Cairo.Color (1, 1, 1, .6);
					cr.Fill ();
				}
				
				xOffset += 2 * (Allocation.Height);
			}
		}
		
		/// <summary>
		/// Paints the forecast temperatures as a chart.
		/// </summary>
		/// <param name="cr">
		/// A <see cref="Cairo.Context"/> to do the painting.
		/// </param>
		void DrawTempGraph (Cairo.Context cr)
		{
			int max = -1000, min = 1000;
			
			for (int day = 0; day < WeatherController.Weather.ForecastDays; day++)
			{
				if (WeatherController.Weather.Forecasts [day].high > max)
					max = WeatherController.Weather.Forecasts [day].high;
				if (WeatherController.Weather.Forecasts [day].low > max)
					max = WeatherController.Weather.Forecasts [day].low;
				if (WeatherController.Weather.Forecasts [day].high < min)
					min = WeatherController.Weather.Forecasts [day].high;
				if (WeatherController.Weather.Forecasts [day].low < min)
					min = WeatherController.Weather.Forecasts [day].low;
		    }
			
			if (max <= min)
				return;
			
			Pango.Layout layout = DockServices.Drawing.ThemedPangoLayout ();
			Pango.Rectangle inkRect, logicalRect;
			
			layout.FontDescription = new Gtk.Style().FontDescription;
			layout.FontDescription.Weight = Pango.Weight.Bold;
			layout.Ellipsize = Pango.EllipsizeMode.None;
			layout.FontDescription.AbsoluteSize = Pango.Units.FromPixels ((int) (Allocation.Height / 5));
			
			// high/low temp
			layout.Width = Pango.Units.FromPixels (Allocation.Height);
			cr.Color = colorHigh;
			layout.SetText (string.Format ("{0}{1}", max, WeatherUnits.TempUnit));
			layout.GetPixelExtents (out inkRect, out logicalRect);
			cr.MoveTo (Allocation.Width - Allocation.Height + (Allocation.Height - inkRect.Width) / 2 - BUTTON_SIZE, Allocation.Height / 6 - logicalRect.Height / 2);
			Pango.CairoHelper.LayoutPath (cr, layout);
			cr.Fill ();
			
			cr.Color = colorLow;
			layout.SetText (string.Format ("{0}{1}", min, WeatherUnits.TempUnit));
			layout.GetPixelExtents (out inkRect, out logicalRect);
			cr.MoveTo (Allocation.Width - Allocation.Height + (Allocation.Height - inkRect.Width) / 2 - BUTTON_SIZE, Allocation.Height * 6 / 9 - logicalRect.Height / 2);
			Pango.CairoHelper.LayoutPath (cr, layout);
			cr.Fill ();
			
			// day names
			layout.Width = Pango.Units.FromPixels (2 * Allocation.Height);
			
			cr.Color = colorTitle;
			for (int day = 0; day < WeatherController.Weather.ForecastDays; day++)
			{
				layout.SetText (WeatherForecast.DayShortName (WeatherController.Weather.Forecasts [day].dow));
				layout.GetPixelExtents (out inkRect, out logicalRect);
				cr.MoveTo (BUTTON_SIZE + day * Allocation.Height * 2 + (Allocation.Height - inkRect.Width) / 2, Allocation.Height * 8 / 9 - logicalRect.Height / 2);
				Pango.CairoHelper.LayoutPath (cr, layout);
			}
			cr.Fill ();
			cr.Save ();
			
			// draw tick lines
			cr.Color = new Cairo.Color (0.627, 0.627, 0.627, .8);
			cr.LineWidth = 1;
			cr.LineCap = LineCap.Round;
			
			int lines = 5;
			for (int line = 0; line < lines - 1; line++) {
				cr.MoveTo (BUTTON_SIZE + Allocation.Height / 4, 4.5 + Allocation.Height * line / lines);
				cr.LineTo (BUTTON_SIZE + (2 * WeatherController.Weather.ForecastDays - 1) * Allocation.Height - Allocation.Height / 4, 4.5 + Allocation.Height * line / lines);
				cr.Stroke ();
			}
			for (int line = 0; ; line++) {
				double x = BUTTON_SIZE + Allocation.Height / 2 + line * 2 * Allocation.Height - 0.5;
				if (x >= BUTTON_SIZE + (2 * WeatherController.Weather.ForecastDays - 1) * Allocation.Height - Allocation.Height / 4)
					break;
				cr.MoveTo (x, 4.5);
				cr.LineTo (x, 4.5 + Allocation.Height * (lines - 2) / lines);
				cr.Stroke ();
			}
			
			cr.Restore ();
			cr.LineWidth = 3;
			double height = ((double) Allocation.Height * 2 / 3 - 5) / (max - min);
			
			// high temp graph
			cr.Color = colorHigh;
			cr.MoveTo (BUTTON_SIZE + Allocation.Height / 2, 5 + height * (max - WeatherController.Weather.Forecasts [0].high));
			for (int day = 1; day < WeatherController.Weather.ForecastDays; day++)
				cr.LineTo (BUTTON_SIZE + day * Allocation.Height * 2 + Allocation.Height / 2, 5 + height * (max - WeatherController.Weather.Forecasts [day].high));
			cr.Stroke ();
			
			// low temp graph
			cr.Color = colorLow;
			cr.MoveTo (BUTTON_SIZE + Allocation.Height / 2, 5 + height * (max - WeatherController.Weather.Forecasts [0].low));
			for (int day = 1; day < WeatherController.Weather.ForecastDays; day++)
				cr.LineTo (BUTTON_SIZE + day * Allocation.Height * 2 + Allocation.Height / 2, 5 + height * (max - WeatherController.Weather.Forecasts [day].low));
			cr.Stroke ();
			
			// high temp points
			for (int day = 0; day < WeatherController.Weather.ForecastDays; day++)
				DrawDataPoint (cr, Allocation.Height, height, max, day, WeatherController.Weather.Forecasts [day].high);
			
			// low temp points
			for (int day = 0; day < WeatherController.Weather.ForecastDays; day++)
				DrawDataPoint (cr, Allocation.Height, height, max, day, WeatherController.Weather.Forecasts [day].low);
		}
		
		void DrawDataPoint (Cairo.Context cr, int cellWidth, double height, int max, int day, int temp)
		{
			cr.Color = new Cairo.Color (0, 0, 0, 0.4);
			cr.Arc (BUTTON_SIZE + day * cellWidth * 2 + cellWidth / 2 + 2, 7 + height * (max - temp), 3, 0, 2 * Math.PI);
			cr.Fill ();
			
			cr.Color = colorTitle;
			cr.Arc (BUTTON_SIZE + day * cellWidth * 2 + cellWidth / 2, 5 + height * (max - temp), 3, 0, 2 * Math.PI);
			cr.Fill ();
		}
		
		/// <summary>
		/// Paints the current condition.
		/// </summary>
		/// <param name="cr">
		/// A <see cref="Cairo.Context"/> to do the painting.
		/// </param>
		void DrawCurrentCondition (Cairo.Context cr)
		{
			Pango.Layout layout = DockServices.Drawing.ThemedPangoLayout ();
			Pango.Rectangle inkRect, logicalRect;
			
			layout.FontDescription = new Gtk.Style().FontDescription;
			layout.FontDescription.Weight = Pango.Weight.Bold;
			layout.Ellipsize = Pango.EllipsizeMode.None;
			layout.Width = Pango.Units.FromPixels ((int) ((Allocation.Width - 4 * BUTTON_SIZE) / 4.5));
			
			layout.FontDescription.AbsoluteSize = Pango.Units.FromPixels ((int) (Allocation.Height / 3));
			
			cr.Color = new Cairo.Color (1, 1, 1, 1);
			
			layout.SetText (WeatherController.Weather.City);
			layout.GetPixelExtents (out inkRect, out logicalRect);
			cr.MoveTo (2 * BUTTON_SIZE, Allocation.Height / 3.5 - logicalRect.Height / 2);
			Pango.CairoHelper.LayoutPath (cr, layout);
			cr.Fill ();
			
			DrawCondition (cr, WeatherController.Weather.SupportsFeelsLike ? 1 : 2, 2, Catalog.GetString ("Humidity"), WeatherController.Weather.Humidity);
			
			DrawCondition (cr, 2, 1, Catalog.GetString ("Temp"), WeatherController.Weather.Temp + WeatherUnits.TempUnit);
			if (WeatherController.Weather.SupportsFeelsLike)
				DrawCondition (cr, 2, 2, Catalog.GetString ("Feels Like"), WeatherController.Weather.FeelsLike + WeatherUnits.TempUnit);
			
			DrawCondition (cr, 3, 1, Catalog.GetString ("Wind"), WeatherController.Weather.Wind + " " + WeatherUnits.WindUnit);
			DrawCondition (cr, 3, 2, Catalog.GetString ("Direction"), WeatherController.Weather.WindDirection);
			
			DrawCondition (cr, 4, 1, Catalog.GetString ("Sunrise"), WeatherController.Weather.SunRise.ToShortTimeString ());
			DrawCondition (cr, 4, 2, Catalog.GetString ("Sunset"), WeatherController.Weather.SunSet.ToShortTimeString ());
		}
		
		void DrawCondition (Cairo.Context cr, int column, int row, string label, string val)
		{
			int xWidth = (Allocation.Width - 4 * BUTTON_SIZE) / 8;
			
			Pango.Layout layout = DockServices.Drawing.ThemedPangoLayout ();
			Pango.Rectangle inkRect, logicalRect;
			
			layout.FontDescription = new Gtk.Style().FontDescription;
			layout.FontDescription.Weight = Pango.Weight.Bold;
			layout.Ellipsize = Pango.EllipsizeMode.None;
			layout.Width = Pango.Units.FromPixels (xWidth);
			
			if (WeatherController.Weather.ForecastDays < 6)
				layout.FontDescription.AbsoluteSize = Pango.Units.FromPixels ((int) (Allocation.Height / 6.5));
			else
				layout.FontDescription.AbsoluteSize = Pango.Units.FromPixels ((int) (Allocation.Height / 4.5));
			
			int xOffset = 2 * BUTTON_SIZE + (column - 1) * 2 * xWidth;
			int yOffset = row == 1 ? Allocation.Height : (int) (Allocation.Height * 2.5);
			yOffset = (int) (yOffset / 3.5);
			
			cr.Color = new Cairo.Color (1, 1, 1, 0.9);
			layout.SetText (label);
			layout.GetPixelExtents (out inkRect, out logicalRect);
			cr.MoveTo (xOffset + (xWidth - inkRect.Width) / 2, yOffset - logicalRect.Height / 2);
			Pango.CairoHelper.LayoutPath (cr, layout);
			cr.Fill ();
			
			cr.Color = new Cairo.Color (1, 1, 1, 0.7);
			layout.SetText (val);
			layout.GetPixelExtents (out inkRect, out logicalRect);
			cr.MoveTo (xOffset + xWidth + (xWidth - inkRect.Width) / 2, yOffset - logicalRect.Height / 2);
			Pango.CairoHelper.LayoutPath (cr, layout);
			cr.Fill ();
		}
		
		/// <summary>
		/// Called when new weather data arrives, to purge the buffers and redraw.
		/// </summary>
		public void WeatherChanged ()
		{
			ResetBuffers ();
			QueueRepaint ();
		}
	}
}
