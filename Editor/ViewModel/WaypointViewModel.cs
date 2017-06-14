/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.ComponentModel;

using FSH;

namespace Editor.ViewModel {

  public class WaypointViewModel : PropertyChangedBase {

    private FSH.Waypoint waypoint;

		public string Name {
			get {
				return this.waypoint.Data.Name;
			}
			set {
        this.waypoint.Data.Name = Utilities.TrimmedString(value, false);
				OnPropertyChanged("Name");
			}
    }  // End of property Name

    public string Comment {
			get {
				return this.waypoint.Data.Comment;
			}
			set {
				this.waypoint.Data.Comment = Utilities.TrimmedString(value, true);
				OnPropertyChanged("Comment");
			}
    }  // End of property Comment

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

    public double Depth {
		  get {
				if (this.waypoint.Data.Depth == -1) {
					return 0;
				} else {
					return this.waypoint.Data.Depth / 2.54 / 12;
				}
			}
			set {
				this.waypoint.Data.Depth = (int)(value * 12 * 2.54);
				OnPropertyChanged("Depth");
			}
    }  // End of property Depth

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

    public char Symbol {
		  get {
				return this.waypoint.Data.Symbol;
			}
			set {
				this.waypoint.Data.Symbol = value;
				OnPropertyChanged("Symbol");
			}
    }  // End of property Symbol

    public WaypointViewModel(Waypoint waypoint) {

			this.waypoint = waypoint;

		}  // End of ctor
    
  }  // End of WaypointViewModel class
}
