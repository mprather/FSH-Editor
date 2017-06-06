/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using FSH;

namespace Editor.ViewModel {

  public class WaypointViewModel : PropertyChangedBase {

		private FSH.Waypoint waypoint;

		public string Name {
			get {
				return this.waypoint.Data.Name;
			}
			set {
				this.waypoint.Data.Name = value;
				OnPropertyChanged("Name");
			}
		}

		public string Comment {
			get {
				return this.waypoint.Data.Comment;
			}
			set {
				this.waypoint.Data.Comment = value;
				OnPropertyChanged("Comment");
			}
		}

		public double Latitude {
			get {
				return this.waypoint.Latitude;
			}
			set {
				this.waypoint.Latitude = value;
				OnPropertyChanged("Latitude");
			}
		}

		public double Longitude {
			get {
				return this.waypoint.Longitude;
			}
			set {
				this.waypoint.Longitude = value;
				OnPropertyChanged("Longitude");
			}
		}

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
		}

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
			}
		}

		public char Symbol {
		  get {
				return this.waypoint.Data.Symbol;
			}
			set {
				this.waypoint.Data.Symbol = value;
				OnPropertyChanged("Symbol");
			}
		}

		public WaypointViewModel(Waypoint waypoint) {

			this.waypoint = waypoint;

		}

	}
}
