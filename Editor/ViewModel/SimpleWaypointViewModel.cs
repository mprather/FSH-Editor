/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using FSH;

namespace Editor.ViewModel {

  public class SimpleWaypointViewModel : PropertyChangedBase {
    
    FSH.SimpleWaypoint simpleWaypoint                = null;

    public string Name {
      get {
        return this.simpleWaypoint.Data.Name;
      }
      set {
        this.simpleWaypoint.Data.Name = value;
        OnPropertyChanged("Name");
      }
    }

    public string Comment {
      get {
        return this.simpleWaypoint.Data.Comment;
      }
      set {
        this.simpleWaypoint.Data.Comment = value;
        OnPropertyChanged("Comment");
      }
    }

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
    }

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
      }
    }

    public char Symbol {
      get {
        return this.simpleWaypoint.Data.Symbol;
      }
      set {
        this.simpleWaypoint.Data.Symbol = value;
        OnPropertyChanged("Symbol");
      }
    }

    public double Latitude {
      get {
        return this.simpleWaypoint.Data.Latitude;
      }
    }

    public double Longitude {
      get {
        return this.simpleWaypoint.Data.Longitude;
      }
    }

    public SimpleWaypointViewModel(SimpleWaypoint simpleWaypoint) {
      this.simpleWaypoint = simpleWaypoint;
    }

  }
}
