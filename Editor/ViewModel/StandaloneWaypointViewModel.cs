/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Linq;
using FSH;

namespace Editor.ViewModel {

  public class StandaloneWaypointViewModel : PropertyChangedBase {
    
    private StandaloneWaypoint standaloneWaypoint      = null;
    private bool selected                              = false;
    private ushort originalStatus                      = 0;

    public string Comment {
      get {
        return this.standaloneWaypoint.Data.Comment;
      }
      set {
        this.standaloneWaypoint.Data.Comment = Utilities.TrimmedString(value, true);
        OnPropertyChanged("Comment");
      }
    }  // End of property Comment

    public long ID { 
      get {
        return this.standaloneWaypoint.ID;
      }
    }  // End of property ID

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
        standaloneWaypoint.Parent.Status = value ? (ushort)0 : originalStatus;
        OnPropertyChanged("IsSelected");
      }
    }  // End of property IsSelected

    public double Latitude {
      get {
        return this.standaloneWaypoint.Data.Latitude;
      }
    }  // End of property Latitude

    public double Longitude {
      get {
        return this.standaloneWaypoint.Data.Longitude;
      }
    }  // End of property Longitude

    public string Name {
      get {
        return this.standaloneWaypoint.Data.Name;
      }
      set {
        this.standaloneWaypoint.Data.Name = Utilities.TrimmedString(value, false);
        OnPropertyChanged("Name");
      }
    }  // End of property Name

    public char Symbol {
      get {
        return this.standaloneWaypoint.Data.Symbol;
      }
      set {
        this.standaloneWaypoint.Data.Symbol = value;
        OnPropertyChanged("Symbol");
      }
    }  // End of property Symbol

    public double Temperature {
      get {
        if (this.standaloneWaypoint.Data.Temperature == ushort.MaxValue) {
          return 0;
        } else {
          return 1.8 * (this.standaloneWaypoint.Data.Temperature / 100.0 - 273.15) + 32;
        }

      }
      set {
        this.standaloneWaypoint.Data.Temperature = (ushort)(((value - 32) / 1.8 + 273) * 100);
        OnPropertyChanged("Temperature");
      }
    }  // End of property Temperature

    public StandaloneWaypointViewModel(StandaloneWaypoint simpleWaypoint) {
      
      this.standaloneWaypoint = simpleWaypoint;
      this.originalStatus     = simpleWaypoint.Parent.Status;

    }  // End of ctor

    public void Refresh() {
      OnPropertyChanged("IsEnabled");
    }  // End of Refresh

  }  // End of SimpleWaypointViewModel class
}
