/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using FSH;

namespace Editor.ViewModel {

  public class StandaloneWaypointViewModel : PropertyChangedBase {
    
    FSH.StandaloneWaypoint simpleWaypoint                = null;

    public string Name {
      get {
        return this.simpleWaypoint.Data.Name;
      }
      set {
        this.simpleWaypoint.Data.Name = Utilities.TrimmedString(value, false);
        OnPropertyChanged("Name");
      }
    }  // End of property Name

    public string Comment {
      get {
        return this.simpleWaypoint.Data.Comment;
      }
      set {
        this.simpleWaypoint.Data.Comment = Utilities.TrimmedString(value, true);
        OnPropertyChanged("Comment");
      }
    }  // End of property Comment

    public double Depth {
      get {
        if (this.simpleWaypoint.Data.Depth == -1) {
          return 0;
        } else {
          return this.simpleWaypoint.Data.Depth / 2.54 / 12;
        }
      }
      set {
        this.simpleWaypoint.Data.Depth = (int)(value * 12 * 2.54);
        OnPropertyChanged("Depth");
      }
    }  // End of property Depth

    public double Temperature {
      get {
        if (this.simpleWaypoint.Data.Temperature == ushort.MaxValue) {
          return 0;
        } else {
          return 1.8 * (this.simpleWaypoint.Data.Temperature / 100.0 - 273.15) + 32;
        }

      }
      set {
        this.simpleWaypoint.Data.Temperature = (ushort)(((value - 32) / 1.8 + 273) * 100);
        OnPropertyChanged("Temperature");
      }
    }  // End of property Temperature

    public char Symbol {
      get {
        return this.simpleWaypoint.Data.Symbol;
      }
      set {
        this.simpleWaypoint.Data.Symbol = value;
        OnPropertyChanged("Symbol");
      }
    }  // End of property Symbol

    public double Latitude {
      get {
        return this.simpleWaypoint.Data.Latitude;
      }
    }  // End of property Latitude

    public double Longitude {
      get {
        return this.simpleWaypoint.Data.Longitude;
      }
    }  // End of property Longitude

    public StandaloneWaypointViewModel(StandaloneWaypoint simpleWaypoint) {
      this.simpleWaypoint = simpleWaypoint;
    }  // End of ctor

  }  // End of SimpleWaypointViewModel class
}
