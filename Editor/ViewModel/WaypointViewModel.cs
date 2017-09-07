/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Linq;
using System.Windows.Input;

using FSH;

namespace Editor.ViewModel {

  public class WaypointViewModel : PropertyChangedBase {

    private FSH.WaypointReference waypoint;

    private string routes                            = null;
    private bool selected                            = false;

    public ICommand PurgeFromRoutesCommand {
      get {

        return new DelegateCommand<WaypointViewModel>(
            "PurgeFromRoutesCommand",
            parameter => {
              if (parameter != null) {
                parameter.PurgeFromRoutes();
              }
            },
            DelegateCommand<ArchiveFileViewModel>.DefaultCanExecute
        );

      }
    }  // End of PurgeFromRoutes

    public string Routes {
      get {
        if (this.routes == null) {
          DiscoverUsage();
        }
        return this.routes;
      }
    }  // End of property Routes

    public long ID {
      get {
        return waypoint.ID;
      }
    }

    public string Comment {
      get {
        return this.waypoint.Data.Comment;
      }
      set {
        this.waypoint.Data.Comment = Utilities.TrimmedString(value, true);
        OnPropertyChanged("Comment");
      }
    }  // End of property Comment

    public double Depth {
      get {
        return this.waypoint.Data.Depth;
      }
    }  // End of property Depth

    public bool IsEnabled {
      get {
        return !ArchiveFile.Current.WaypointInUse(this.ID, this.Name, this.Latitude, this.Longitude);
      }
    }  // End of property IsEnabled

    public bool IsSelected {
      get {
        return this.selected;
      }
      set {
        this.selected = value;
        OnPropertyChanged("IsSelected");
      }
    }  // End of property IsSelected

    public double Latitude {
      get {
        return this.waypoint.Latitude;
      }
      set {
        this.waypoint.Latitude = value;
        OnPropertyChanged("Latitude");
      }
    }  // End of property Latitude

    public double Longitude {
      get {
        return this.waypoint.Longitude;
      }
      set {
        this.waypoint.Longitude = value;
        OnPropertyChanged("Longitude");
      }
    }  // End of property Longitude

    public string Name {
      get {
        return this.waypoint.Data.Name;
      }
      set {
        this.waypoint.Data.Name = Utilities.TrimmedString(value, false);
        OnPropertyChanged("Name");
      }
    }  // End of property Name

    public char Symbol {
      get {
        return this.waypoint.Data.Symbol;
      }
      set {
        this.waypoint.Data.Symbol = value;
        OnPropertyChanged("Symbol");
      }
    }  // End of property Symbol
    
    public double Temperature {
      get {
        if (this.waypoint.Data.Temperature == ushort.MaxValue) {
          return 0;
        } else {
          return 1.8 * (this.waypoint.Data.Temperature/100.0 - 273.15) + 32;
        }
      }
      set {
        this.waypoint.Data.Temperature = (ushort) (((value-32)/1.8 + 273)*100);
        OnPropertyChanged("Temperature");
      }
    }  // End of property Temperature

    public WaypointViewModel(WaypointReference waypoint) {

      this.waypoint = waypoint;

    }  // End of ctor
    
    public void Refresh() {
      
      OnPropertyChanged("IsEnabled");
      
      DiscoverUsage();
      OnPropertyChanged("Routes");

    }  // End of Refresh

    private void DiscoverUsage() {

      this.routes = null;

      ArchiveFile.Current.Flobs.ForEach(f => {
        foreach (var q in f.Blocks.Where(b => b.Status != 0 && b.Type == BlockType.Route)) {
          Route r = q.Data as Route;
          foreach (var w in r.ReferencedWaypoints.Where(rw => rw.ID == this.ID)) {
            if (!String.IsNullOrEmpty(this.routes)) {
              this.routes += Environment.NewLine;
            }
            this.routes += r.Name;  
          }
        }
      });

      if (String.IsNullOrEmpty(this.routes)) {
        this.routes = "";
      }
      
    }  // End of DiscoverUsage

    private void PurgeFromRoutes() {

      bool itemFound = false;

      foreach (var r in ArchiveFileViewModel.Current.RouteViewModels) {
        
        itemFound = false;
        
        foreach (var wvm in r.WaypointViewModels.Where(w=> w.ID == this.ID)) {
          wvm.IsSelected = true;
          itemFound = true;
        }

        if (itemFound) { 
          r.DeleteWaypoints();
        }

      }

      ArchiveFileViewModel.Current.RefreshViewModels();

    }  // End of PurgeFromRoutes

  }  // End of WaypointViewModel class
}
