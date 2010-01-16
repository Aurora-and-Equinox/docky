#!/usr/bin/env python

#  
#  Copyright (C) 2009-2010 Jason Smith, Rico Tzschichholz
# 
#  This program is free software: you can redistribute it and/or modify
#  it under the terms of the GNU General Public License as published by
#  the Free Software Foundation, either version 3 of the License, or
#  (at your option) any later version.
# 
#  This program is distributed in the hope that it will be useful,
#  but WITHOUT ANY WARRANTY; without even the implied warranty of
#  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#  GNU General Public License for more details.
# 
#  You should have received a copy of the GNU General Public License
#  along with this program.  If not, see <http://www.gnu.org/licenses/>.
#

import atexit
import gobject
import glib
import dbus
import dbus.glib
import sys
import os

try:
	from docky.docky import DockyItem, DockySink
	from signal import signal, SIGTERM
	from sys import exit
except ImportError, e:
	exit()

tomboybus = "org.gnome.Tomboy"
tomboypath = "/org/gnome/Tomboy/RemoteControl"
tomboyiface = "org.gnome.Tomboy.RemoteControl"

class DockyTomboyItem(DockyItem):
	def __init__(self, path):
		DockyItem.__init__(self, path)
		self.tomboy = None
		
		self.bus.add_signal_receiver(self.name_owner_changed_cb,
                                             dbus_interface='org.freedesktop.DBus',
                                             signal_name='NameOwnerChanged')
                                             
		obj = self.bus.get_object ("org.freedesktop.DBus", "/org/freedesktop/DBus")
		self.bus_interface = dbus.Interface(obj, "org.freedesktop.DBus")
		
		self.bus_interface.ListNames (reply_handler=self.list_names_handler, error_handler=self.list_names_error_handler)
	
		self.bus.add_signal_receiver(self.note_added, "NoteAdded", tomboyiface, tomboybus, tomboypath)
		self.bus.add_signal_receiver(self.note_deleted, "NoteDeleted", tomboyiface, tomboybus, tomboypath)
		self.bus.add_signal_receiver(self.note_changed, "NoteChanged", tomboyiface, tomboybus, tomboypath)
	
	def list_names_handler(self, names):
		if tomboybus in names:
			self.init_tomboy_objects()
			self.set_menu_buttons()

	def list_names_error_handler(self, error):
		print "error getting bus names - %s" % str(error)
	
	def name_owner_changed_cb(self, name, old_owner, new_owner):
		if name == tomboybus:
			if new_owner:
				self.init_tomboy_objects()
			else:
				self.tomboy = None

			self.set_menu_buttons()
	
	def init_tomboy_objects(self):
		obj = self.bus.get_object(tomboybus, tomboypath)
		self.tomboy = dbus.Interface(obj, tomboyiface)
		
	def note_added (self, note):
		self.set_menu_buttons()
		
	def note_deleted (self, note, a):
		self.set_menu_buttons()
	
	def note_changed (self, note):
		self.set_menu_buttons()
	
	def clear_menu_buttons(self):
		for k, v in self.id_map.iteritems():
			try:
				self.iface.RemoveItem(k)
			except:
				break;	
	
	def set_menu_buttons(self):
		self.clear_menu_buttons()
		
		if not self.tomboy:
			return

		self.add_menu_item ("Create New Note", "tomboy", "New", "Tomboy Controls")
		self.add_menu_item ("Find Note...", "gtk-find", "Find", "Tomboy Controls")
		
		for note in self.get_tomboy_menu_notes ():
			self.add_menu_item (self.tomboy.GetNoteTitle(note), "tomboy", note, "Notes")
		
	def get_tomboy_menu_notes(self):
		notes = self.tomboy.ListAllNotes()
		notes.sort(key=self.tomboy.GetNoteChangeDate)
		notes.reverse()
		return notes[:9]
		
	def menu_pressed(self, menu_id):
		if not menu_id in self.id_map:
			return	
		
		menu_id = self.id_map[menu_id]
		
		if menu_id == "New":
			newnote = self.tomboy.CreateNote()
			self.tomboy.DisplayNote(newnote)
		elif menu_id == "Find":
			self.tomboy.DisplaySearch()
		else:
			self.tomboy.DisplayNote(menu_id)
		
	def add_menu_item(self, name, icon, ident, title):
		menu_id = self.iface.AddMenuItem(name, icon, title)
		self.id_map[menu_id] = ident
		
class DockyTomboySink(DockySink):
	def item_path_found(self, pathtoitem, item):
		if item.GetOwnsDesktopFile() and item.GetDesktopFile().endswith ("tomboy.desktop"):
			self.items[pathtoitem] = DockyTomboyItem(pathtoitem)

dockysink = DockyTomboySink()

def cleanup ():
	dockysink.dispose ()

if __name__ == "__main__":
	mainloop = gobject.MainLoop(is_running=True)

	atexit.register (cleanup)
	signal(SIGTERM, lambda signum, stack_frame: exit(1))

	mainloop.run()
