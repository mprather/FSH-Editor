/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Xml;

using FSH;

namespace Editor.ViewModel {

  public class RouteViewModel : PropertyChangedBase {
    
    private static Regex RouteRegex                  = new Regex("(?<start>.+)(?<marker>->?)(?<end>.+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

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
    
    public ICommand ExportCommand {
      get {
        return new DelegateCommand<RouteViewModel>(
          "ExportCommand",
          parameter => {
            if (parameter != null) {
              parameter.Export();
            }
          },
          DelegateCommand<RouteViewModel>.DefaultCanExecute
        );
      }
    }  // End of property ExportCommand

    public ICommand ReverseCommand {
      get {
        return new DelegateCommand<RouteViewModel>(
          "ReverseCommand",
          parameter => {
            if (parameter != null) {
              parameter.Reverse();
            }
          },
          DelegateCommand<RouteViewModel>.DefaultCanExecute
        );
      }
    }  // End of property ExportCommand

    public string RouteName {
			get {
				return route.Name;
			}
			set {
        route.Name = Utilities.TrimmedString(value, false);
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
        
    private void Export() {

      Utilities.CreateGPXDocument("rte",
                                  this.RouteName,
                                  this.route.Comment,
                                  x => {

                                    XmlNamespaceManager manager = new XmlNamespaceManager(x.NameTable);
                                    x.DocumentElement.SetAttribute("xmlns:opencpn", "http://www.opencpn.org");

                                    XmlElement extensions = x.CreateElement("extensions");
                                    x.DocumentElement.FirstChild.AppendChild(extensions);

                                    // ----------------------------------------------------------------------------
                                    // NOTE: OpenCPN is hard-coded to use the "opencpn" prefix, which is a bug 
                                    //       on their part.
                                    // ----------------------------------------------------------------------------
                                    XmlElement start = x.CreateElement("opencpn", "start", "http://www.opencpn.org");
                                    start.InnerText = this.WaypointViewModels[0].Name;
                                    extensions.AppendChild(start);

                                    XmlElement end = x.CreateElement("opencpn", "end", "http://www.opencpn.org");
                                    end.InnerText = this.WaypointViewModels[this.WaypointViewModels.Count-1].Name;
                                    extensions.AppendChild(end);

                                    foreach (var q in this.WaypointViewModels) {

                                      XmlElement point = Utilities.CreateWaypointElement(x, "rtept", q.Latitude, q.Longitude);
                                      x.DocumentElement.FirstChild.AppendChild(point);

                                      point.AppendChild(Utilities.CreateNameElement(x, q.Name));

                                      if (!String.IsNullOrEmpty(q.Comment)) {
                                        point.AppendChild(Utilities.CreateDescriptionElement(x, q.Comment));
                                      }

                                      XmlElement symbol = x.CreateElement("sym");
                                      symbol.InnerText = "circle";
                                      point.AppendChild(symbol);
                                      
                                    }

                                  });
     
    }  // End of Export
    
    private void Reverse() {

      Match match = RouteRegex.Match(this.RouteName);
      if (match.Success) {
        this.RouteName = match.Groups["end"].Value + match.Groups["marker"].Value + match.Groups["start"].Value;
      }

      this.route.Reverse();
      
      this.WaypointViewModels.Clear();
      this.route.WaypointSummary.Waypoints.ForEach(w => {
        this.WaypointViewModels.Add(new WaypointViewModel(w));
      });

    }  // End of Reverse

  }

}
