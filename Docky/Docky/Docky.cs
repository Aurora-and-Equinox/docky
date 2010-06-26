//  
//  Copyright (C) 2009-2010 Jason Smith, Robert Dyer
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
using System.Threading;
using System.IO;

using Mono.Unix;
using NDesk.DBus;

using Gdk;
using Gtk;

using Docky.DBus;
using Docky.Windowing;
using Docky.Services;

namespace Docky
{
	public static class Docky
	{
		public static UserArgs CommandLinePreferences { get; private set; }
		
		static DockController controller;
		internal static DockController Controller { 
			get {
				if (controller == null)
					controller = new DockController ();
				return controller;
			}
		}
		
		public static void Main (string[] args)
		{
			// output the version number & system info
			Log.DisplayLevel = LogLevel.Info;
			Log.Info ("Docky version: {0} {1}", AssemblyInfo.DisplayVersion, AssemblyInfo.VersionDetails);
			Log.Info ("Kernel version: {0}", System.Environment.OSVersion.Version);
			Log.Info ("CLR version: {0}", System.Environment.Version);
			
			//Init gtk and GLib related
			Catalog.Init ("docky", AssemblyInfo.LocaleDirectory);
			if (!GLib.Thread.Supported) GLib.Thread.Init ();
			Gdk.Threads.Init ();
			NDesk.DBus.BusG.Init ();
			Gtk.Application.Init ("Docky", ref args);
			Gnome.Vfs.Vfs.Initialize ();
			GLib.GType.Init ();
			
			// process the command line args
			CommandLinePreferences = new UserArgs (args);
			
			Wnck.Global.ClientType = Wnck.ClientType.Pager;
			
			// set process name
			DockServices.System.SetProcessName ("docky");
			if (CheckForInstance ()) {
				Log.Error ("Another Docky instance was detected - exiting.");
				return;
			}
			
			// cache main thread
			SystemService.MainThread = Thread.CurrentThread;
			
			// check compositing
			CheckComposite ();
			Gdk.Screen.Default.CompositedChanged += delegate {
				CheckComposite ();
			};
			
			DBusManager.Default.Initialize ();
			DBusManager.Default.QuitCalled += delegate {
				Quit ();
			};
			DBusManager.Default.SettingsCalled += delegate {
				ConfigurationWindow.Instance.Show ();
			};
			DBusManager.Default.AboutCalled += delegate {
				ShowAbout ();
			};
			PluginManager.Initialize ();
			DockServices.Theme.Initialize ();
			Controller.Initialize ();
			
			Gdk.Threads.Enter ();
			Gtk.Application.Run ();
			Gdk.Threads.Leave ();
			
			Controller.Dispose ();
			DockServices.Dispose ();
			PluginManager.Shutdown ();
			Gnome.Vfs.Vfs.Shutdown ();
		}
		
		static uint checkCompositeTimer = 0;
		
		static bool CheckForInstance ()
		{
			return Bus.Session.NameHasOwner ("org.gnome.Docky");
		}
		
		static void CheckComposite ()
		{
			if (checkCompositeTimer != 0) {
				GLib.Source.Remove (checkCompositeTimer);
				checkCompositeTimer = 0;
			}
			
			checkCompositeTimer = GLib.Timeout.Add (2000, delegate {
				if (!Gdk.Screen.Default.IsComposited)
					Log.Notify (Catalog.GetString ("Docky requires compositing to work properly. " +
						"Certain options are disabled and themes/animations will look incorrect. "));
				checkCompositeTimer = 0;
				return false;
			});
		}
		
		public static void ShowAbout ()
		{
			Gtk.AboutDialog about = new Gtk.AboutDialog ();
			about.ProgramName = "Docky";
			about.Version = AssemblyInfo.DisplayVersion + "\n" + AssemblyInfo.VersionDetails;
			about.IconName = "docky";
			about.LogoIconName = "docky";
			about.Website = "https://launchpad.net/docky";
			about.WebsiteLabel = "Website";
			Gtk.AboutDialog.SetUrlHook ((dialog, link) => DockServices.System.Open (link));
			about.Copyright = "Copyright \xa9 2009-2010 Docky Developers";
			about.Comments = "Docky. Simply Powerful.";
			about.Authors = new[] {
				"Jason Smith <jason@go-docky.com>",
				"Robert Dyer <robert@go-docky.com>",
				"Chris Szikszoy <chris@go-docky.com>",
				"Rico Tzschichholz <rtz@go-docky.com>",
				"Seif Lotfy <seif@lotfy.com>",
				"Chris Halse Rogers <raof@ubuntu.com>",
				"Alex Launi <alex.launi@gmail.com>",
			};
			about.Artists = new[] { 
				"Daniel Foré <bunny@go-docky.com>",
			};
			about.Documenters = new[] {
				"Sven Mauhar <zekopeko@gmail.com>",
				"Robert Dyer <robert@go-docky.com>",
				"Daniel Foré <bunny@go-docky.com>",
				"Chris Szikszoy <chris@go-docky.com>",
				"Rico Tzschichholz <rtz@go-docky.com>",
			};
			about.TranslatorCredits = 
				"Asturian\n" +
				" Xuacu Saturio\n" +
				"\n" +
				
				"Basque\n" +
				" Ibai Oihanguren\n" +
				"\n" +
				
				"Bengali\n" +
				" Scio\n" +
				"\n" +
				
				"Brazilian Portuguese\n" +
				" André Gondim, Fabio S Monteiro, Flávio Etrusco, Glauco Vinicius\n" +
				" Lindeval, Thiago Bellini, Victor Mello\n" +
				"\n" +
				
				"Bulgarian\n" +
				" Boyan Sotirov, Krasimir Chonov\n" +
				"\n" +
				
				"Catalan\n" +
				" BadChoice, Siegfried Gevatter\n" +
				"\n" +
				
				"Chinese (Simplified)\n" +
				" Chen Tao, G.S.Alex, Xhacker Liu, fighterlyt, lhquark, skatiger, 冯超\n" +
				"\n" +
				
				"Croatian\n" +
				" Saša Teković, zekopeko\n" +
				"\n" +
				
				"English (United Kingdom)\n" +
				" Alex Denvir, Daniel Bell, David Wood, Joel Auterson, SteVe Cook\n" +
				"\n" +
				
				"Finnish\n" +
				" Jiri Grönroos\n" +
				"\n" +
				
				"French\n" +
				" Hugo M., Kévin Gomez, Pierre Slamich\n" +
				" Simon Richard, alienworkshop, maxime Cheval\n" +
				"\n" +
				
				"Galician\n" +
				" Francisco Diéguez, Indalecio Freiría Santos, Miguel Anxo Bouzada, NaNo\n" +
				"\n" +
				
				"German\n" +
				" Cephinux, Gabriel Shahzad, Jan-Christoph Borchardt, Mark Parigger\n" + 
				" Martin Lettner, augias, fiction, pheder, tai\n" +
				"\n" +
				
				"Hebrew\n" +
				" Uri Shabtay\n" +
				"\n" +
				
				"Hindi\n" +
				" Bilal Akhtar\n" +
				"\n" +
				
				"Hungarian\n" +
				" Bognár András, Gabor Kelemen, Jezsoviczki Ádám, NewPlayer\n" +
				"\n" +
				
				"Icelandic\n" +
				" Baldur, Sveinn í Felli\n" +
				"\n" +
				
				"Indonesian\n" +
				" Andika Triwidada, Fakhrul Rijal\n" +
				"\n" +
				
				"Italian\n" +
				" Andrea Amoroso, Blaster, Ivan, MastroPino, Michele, Milo Casagrande, Quizzlo\n" +
				"\n" +
				
				"Japanese\n" +
				" kawaji\n" +
				"\n" +
				
				"Korean\n" +
				" Bugbear5, Cedna\n" +
				"\n" +
				
				"Polish\n" +
				" 313, Adrian Grzemski, EuGene, Rafał Szalecki, Stanisław Gackowski, bumper, emol007\n" +
				"\n" +
				
				"Romanian\n" +
				" Adi Roiban, George Dumitrescu\n" +
				"\n" +
				
				"Russian\n" +
				" Alexander Semyonov, Alexey Nedilko, Andrey Sitnik, Artem Yakimenko\n" +
				" Dmitriy Bobylev, Ivan, Phenomen, Sergey Demurin, Sergey Sedov\n" +
				" SochiX, Vladimir, legin, sX11\n" +
				"\n" +
				
				"Spanish\n" +
				" Alejandro Navarro, David, DiegoJ, Edgardo Fredz, FAMM, Fuerteventura\n" +
				" Gus, José A. Fuentes Santiago, Julián Alarcón, Malq, Martín V.\n" +
				" Omar Campagne, Ricardo Pérez López, Sebastián Porta, alvin23, augias, elXATU\n" +
				"\n" +
				
				"Swedish\n" +
				" Daniel Nylander, Rovanion, riiga\n" +
				"\n" +
				
				"Turkish\n" +
				" Yalçın Can, Yiğit Ateş\n" +
				"\n" +
				
				"Ukrainian\n" +
				" naker.ua\n";
			
			about.ShowAll ();
			
			about.Response += delegate {
				about.Hide ();
				about.Destroy ();
			};
			
		}
		
		public static void Quit ()
		{
			DBusManager.Default.Shutdown ();
			Gtk.Application.Quit ();
		}
	}
}
