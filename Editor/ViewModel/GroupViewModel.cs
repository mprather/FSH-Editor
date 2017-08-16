/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml;

using FSH;

namespace Editor.ViewModel {

  public class GroupViewModel : MappingViewModel {

    private FSH.Group group;

    public ICommand DeleteWaypointsCommand {
      get {
        return new DelegateCommand<GroupViewModel>(
          "DeleteWaypoints",
          parameter => {
            if (parameter != null) {
              parameter.DeleteWaypoints();
            }
          },
          DelegateCommand<RouteViewModel>.DefaultCanExecute
        );
      }
    }  // End of property DeleteWaypointsCommand

    public string GroupName {
		  get {
				return group.Name;
			}
			set {
        group.Name = Utilities.TrimmedString(value, false);
				OnPropertyChanged("GroupName");
			}
		}  // End of property GroupName

		public string WaypointCount {
		  get {
				return group.Waypoints.Count + " Waypoints";
			}
		}  // End of property WaypointCount

    public ICommand ExportCommand {
      get {
        return new DelegateCommand<GroupViewModel>(
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

    public ObservableCollection<WaypointViewModel> WaypointViewModels { get; set; }

    public GroupViewModel (Group group, Flob parent) {

			this.WaypointViewModels = new ObservableCollection<WaypointViewModel>();

		  this.group = group;

			this.group.Waypoints.ForEach(w => {

				this.WaypointViewModels.Add(new WaypointViewModel(w));

			});

		}  // End of ctor

    protected override void CreateMap() {
      
      base.CreateMap();
    
      string fileName = Properties.Resources.MapFolderName + "\\" + this.GroupName + ".mapview.html";

      string locations = null;
      string options = null;

      for (int i = 0; i < this.WaypointViewModels.Count; i++) {
        if (!string.IsNullOrEmpty(locations)) {
          locations += ",";
          options += ",";
        }
        locations += "new l(" + this.WaypointViewModels[i].Latitude.ToString("00.000000") + ", " + 
                                this.WaypointViewModels[i].Longitude.ToString("00.000000") + ")";
        options += "{title: \"" + this.WaypointViewModels[i].Name + "\", color: \"red\"}";
      }
      
      using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName)) {
        writer.WriteLine(HtmlTemplate.Replace("{PageTitle}", this.GroupName).Replace("{locations}", locations).Replace("{options}", options));
      }

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = fileName
      });

    }  // End of CreateMap
    
    private void DeleteWaypoints() {

      List<WaypointViewModel> selectedItems = new List<WaypointViewModel>(this.WaypointViewModels.Where(w => w.IsSelected));

      foreach (var q in selectedItems) {
        this.WaypointViewModels.Remove(q);
      }

    }  // End of DeleteWaypoints

    private void Export() {

      Utilities.CreateGPXDocument(null,
                                  this.GroupName,
                                  null,
                                  x => {

                                    foreach (var q in this.WaypointViewModels) {

                                      XmlElement waypoint = Utilities.CreateWaypointElement(x, "wpt", q.Latitude, q.Longitude);
                                      x.DocumentElement.AppendChild(waypoint);

                                      waypoint.AppendChild(Utilities.CreateNameElement(x, q.Name));
                                      waypoint.AppendChild(Utilities.CreateDescriptionElement(x, Utilities.AddExportTimestamp(q.Comment)));
                                      waypoint.AppendChild(Utilities.CreateSourceElement(x));
                                      waypoint.AppendChild(Utilities.CreateLinkElement(x));

                                      if (Properties.Settings.Default.IncludeDepth && q.Depth != -1) {
                                        waypoint.AppendChild(Utilities.CreateElevationElement(x, q.Depth));
                                      }

                                    }

                                  });

    }  // End of Export

  }
}
