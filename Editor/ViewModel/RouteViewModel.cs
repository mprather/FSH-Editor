/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.ObjectModel;
using System.Windows.Input;

using FSH;

namespace Editor.ViewModel {

  public class RouteViewModel : PropertyChangedBase {

		private FSH.Route route;

		public ICommand CreateRouteMap {
		  get {
				return new DelegateCommand<RouteViewModel>(
          "CreateRouteMap",
					parameter => {
						if (parameter != null) {
							parameter.DisplayRouteMap();
						}
					},
					DelegateCommand<RouteViewModel>.DefaultCanExecute
				);
			}
		}  // End of property CreateRouteMap

		public string RouteName {
			get {
				return route.Name;
			}
			set {
        if (value.Length > SerializableData.MaximumStringLength) {
          value = value.Substring(0, SerializableData.MaximumStringLength);
        }
        route.Name = value;
				OnPropertyChanged("RouteName");
			}
    }  // End of property RouteName

    public string WaypointCount {
			get {
				return route.WaypointSummary.Waypoints.Count + " Waypoints";
			}
    }  // End of property WaypointCount

    public ObservableCollection<WaypointViewModel> WaypointViewModels { get; set; }

		public RouteViewModel(Route route) {

			this.WaypointViewModels = new ObservableCollection<WaypointViewModel>();

			this.route = route;

			this.route.WaypointSummary.Waypoints.ForEach(w => {

				this.WaypointViewModels.Add(new WaypointViewModel(w));

			});

		}  // End of ctor

		private void DisplayRouteMap() {

      /*
      Reference url: https://msdn.microsoft.com/en-us/library/dn217138.aspx
      Details:       polyline
Specifies a polyline on the map by specifying a set of points. For polylines, the value includes a set of latitude and longitude points, a title, notes, a reference URL, a photo URL, line color, fill color, line weight, line style, dash style, and the latitude and longitude of the label, each separated by an underscore (_).

sp=polyline.lat1_long1_lat2_long2_..._titleString_notesString_linkURL_photoURL_strokeColor_fillColor_strokeWeight_ strokeStyle_strokeDashStyle_labelLatitude_labelLongitude

Fill color and stroke color are each specified as hexadecimal RGB values, such as #00ff00.
Stroke weight is specified as a pixel value, such as 4px.
Stroke style includes the following values: Single, ThinThin, ThickThin, ThinThick, ThickBetweenThin.
Stroke dash style includes the following values: Solid, ShortDash, ShortDot, ShortDashDot, ShortDashDotDot, Dot, Dash, LongDash, DashDot, LongDashDot, and LongDashDotDot.

      */
      string url = "http://bing.com/maps?cp=" + this.WaypointViewModels[0].Latitude.ToString("0.00") + "~" + this.WaypointViewModels[0].Longitude.ToString("0.00") + "&sp=polyline.";
      
      foreach (var q in this.WaypointViewModels) {
				url += q.Latitude.ToString("0.0000") + "_" + q.Longitude.ToString("0.0000") + "_";
			}

			url += this.RouteName + "_____%237F0000_1px";

			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
				UseShellExecute = true,
				FileName = url
			});

		}  // End of DisplayRouteMap

	}

}
