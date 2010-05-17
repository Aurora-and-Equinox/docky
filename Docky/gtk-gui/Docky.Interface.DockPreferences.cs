// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Docky.Interface {
    
    
    public partial class DockPreferences {
        
        private Gtk.VBox vbox1;
        
        private Gtk.Table table3;
        
        private Gtk.ComboBox autohide_box;
        
        private Gtk.CheckButton fade_on_hide_check;
        
        private Gtk.Label hide_desc;
        
        private Gtk.HScale icon_scale;
        
        private Gtk.Label label1;
        
        private Gtk.Label label3;
        
        private Gtk.CheckButton panel_mode_button;
        
        private Gtk.CheckButton threedee_check;
        
        private Gtk.CheckButton window_manager_check;
        
        private Gtk.CheckButton zoom_checkbutton;
        
        private Gtk.HScale zoom_scale;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Docky.Interface.DockPreferences
            Stetic.BinContainer.Attach(this);
            this.Name = "Docky.Interface.DockPreferences";
            // Container child Docky.Interface.DockPreferences.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.table3 = new Gtk.Table(((uint)(7)), ((uint)(3)), false);
            this.table3.Name = "table3";
            this.table3.RowSpacing = ((uint)(6));
            this.table3.ColumnSpacing = ((uint)(6));
            // Container child table3.Gtk.Table+TableChild
            this.autohide_box = Gtk.ComboBox.NewText();
            this.autohide_box.AppendText(Mono.Unix.Catalog.GetString("None"));
            this.autohide_box.AppendText(Mono.Unix.Catalog.GetString("Autohide"));
            this.autohide_box.AppendText(Mono.Unix.Catalog.GetString("Intellihide"));
            this.autohide_box.AppendText(Mono.Unix.Catalog.GetString("Window Dodge"));
            this.autohide_box.Name = "autohide_box";
            this.autohide_box.Active = 3;
            this.table3.Add(this.autohide_box);
            Gtk.Table.TableChild w1 = ((Gtk.Table.TableChild)(this.table3[this.autohide_box]));
            w1.LeftAttach = ((uint)(1));
            w1.RightAttach = ((uint)(2));
            w1.XOptions = ((Gtk.AttachOptions)(4));
            w1.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.fade_on_hide_check = new Gtk.CheckButton();
            this.fade_on_hide_check.TooltipMarkup = "Fade out dock instead of slide off screen";
            this.fade_on_hide_check.CanFocus = true;
            this.fade_on_hide_check.Name = "fade_on_hide_check";
            this.fade_on_hide_check.Label = Mono.Unix.Catalog.GetString("_Fade On Hide");
            this.fade_on_hide_check.DrawIndicator = true;
            this.fade_on_hide_check.UseUnderline = true;
            this.table3.Add(this.fade_on_hide_check);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table3[this.fade_on_hide_check]));
            w2.LeftAttach = ((uint)(2));
            w2.RightAttach = ((uint)(3));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.hide_desc = new Gtk.Label();
            this.hide_desc.Name = "hide_desc";
            this.hide_desc.Yalign = 0F;
            this.hide_desc.UseMarkup = true;
            this.table3.Add(this.hide_desc);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table3[this.hide_desc]));
            w3.TopAttach = ((uint)(1));
            w3.BottomAttach = ((uint)(2));
            w3.RightAttach = ((uint)(3));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.icon_scale = new Gtk.HScale(null);
            this.icon_scale.CanFocus = true;
            this.icon_scale.Name = "icon_scale";
            this.icon_scale.Adjustment.Upper = 100;
            this.icon_scale.Adjustment.PageIncrement = 10;
            this.icon_scale.Adjustment.StepIncrement = 1;
            this.icon_scale.DrawValue = true;
            this.icon_scale.Digits = 0;
            this.icon_scale.ValuePos = ((Gtk.PositionType)(0));
            this.table3.Add(this.icon_scale);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table3[this.icon_scale]));
            w4.TopAttach = ((uint)(2));
            w4.BottomAttach = ((uint)(3));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(3));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label1 = new Gtk.Label();
            this.label1.CanFocus = true;
            this.label1.Name = "label1";
            this.label1.Xalign = 1F;
            this.label1.Yalign = 0F;
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("_Icon Size:");
            this.label1.UseUnderline = true;
            this.table3.Add(this.label1);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table3[this.label1]));
            w5.TopAttach = ((uint)(2));
            w5.BottomAttach = ((uint)(3));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.label3 = new Gtk.Label();
            this.label3.CanFocus = true;
            this.label3.Name = "label3";
            this.label3.Xalign = 1F;
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("_Hiding:");
            this.label3.UseUnderline = true;
            this.table3.Add(this.label3);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table3[this.label3]));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.panel_mode_button = new Gtk.CheckButton();
            this.panel_mode_button.CanFocus = true;
            this.panel_mode_button.Name = "panel_mode_button";
            this.panel_mode_button.Label = Mono.Unix.Catalog.GetString("_Panel Mode");
            this.panel_mode_button.DrawIndicator = true;
            this.panel_mode_button.UseUnderline = true;
            this.table3.Add(this.panel_mode_button);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table3[this.panel_mode_button]));
            w7.TopAttach = ((uint)(4));
            w7.BottomAttach = ((uint)(5));
            w7.RightAttach = ((uint)(3));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.threedee_check = new Gtk.CheckButton();
            this.threedee_check.CanFocus = true;
            this.threedee_check.Name = "threedee_check";
            this.threedee_check.Label = Mono.Unix.Catalog.GetString("3D Back_ground");
            this.threedee_check.DrawIndicator = true;
            this.threedee_check.UseUnderline = true;
            this.table3.Add(this.threedee_check);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table3[this.threedee_check]));
            w8.TopAttach = ((uint)(5));
            w8.BottomAttach = ((uint)(6));
            w8.RightAttach = ((uint)(3));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.window_manager_check = new Gtk.CheckButton();
            this.window_manager_check.CanFocus = true;
            this.window_manager_check.Name = "window_manager_check";
            this.window_manager_check.Label = Mono.Unix.Catalog.GetString("_Manage Windows Without Launcher");
            this.window_manager_check.DrawIndicator = true;
            this.window_manager_check.UseUnderline = true;
            this.window_manager_check.Xalign = 0F;
            this.table3.Add(this.window_manager_check);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table3[this.window_manager_check]));
            w9.TopAttach = ((uint)(6));
            w9.BottomAttach = ((uint)(7));
            w9.RightAttach = ((uint)(3));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.zoom_checkbutton = new Gtk.CheckButton();
            this.zoom_checkbutton.CanFocus = true;
            this.zoom_checkbutton.Name = "zoom_checkbutton";
            this.zoom_checkbutton.Label = Mono.Unix.Catalog.GetString("_Zoom:");
            this.zoom_checkbutton.DrawIndicator = true;
            this.zoom_checkbutton.UseUnderline = true;
            this.zoom_checkbutton.Xalign = 1F;
            this.table3.Add(this.zoom_checkbutton);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table3[this.zoom_checkbutton]));
            w10.TopAttach = ((uint)(3));
            w10.BottomAttach = ((uint)(4));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table3.Gtk.Table+TableChild
            this.zoom_scale = new Gtk.HScale(null);
            this.zoom_scale.CanFocus = true;
            this.zoom_scale.Name = "zoom_scale";
            this.zoom_scale.UpdatePolicy = ((Gtk.UpdateType)(1));
            this.zoom_scale.Adjustment.Upper = 100;
            this.zoom_scale.Adjustment.PageIncrement = 10;
            this.zoom_scale.Adjustment.StepIncrement = 0.01;
            this.zoom_scale.Adjustment.Value = 20;
            this.zoom_scale.DrawValue = true;
            this.zoom_scale.Digits = 2;
            this.zoom_scale.ValuePos = ((Gtk.PositionType)(0));
            this.table3.Add(this.zoom_scale);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table3[this.zoom_scale]));
            w11.TopAttach = ((uint)(3));
            w11.BottomAttach = ((uint)(4));
            w11.LeftAttach = ((uint)(1));
            w11.RightAttach = ((uint)(3));
            w11.XOptions = ((Gtk.AttachOptions)(4));
            w11.YOptions = ((Gtk.AttachOptions)(4));
            this.vbox1.Add(this.table3);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.vbox1[this.table3]));
            w12.Position = 0;
            w12.Expand = false;
            w12.Fill = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.label1.MnemonicWidget = this.icon_scale;
            this.label3.MnemonicWidget = this.autohide_box;
            this.Hide();
            this.window_manager_check.Toggled += new System.EventHandler(this.OnWindowManagerCheckToggled);
            this.threedee_check.Toggled += new System.EventHandler(this.OnThreedeeCheckToggled);
            this.panel_mode_button.Toggled += new System.EventHandler(this.OnPanelModeButtonToggled);
        }
    }
}
