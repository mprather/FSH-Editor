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

namespace Editor.ViewModel {

  public class StandaloneWaypointsSummaryViewModel : MappingViewModel {
    
    public ICommand DeleteWaypointsCommand {
      get {
        return new DelegateCommand<StandaloneWaypointsSummaryViewModel>(
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

    public ICommand ExportCommand {
      get {
        return new DelegateCommand<StandaloneWaypointsSummaryViewModel>(
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

    public ObservableCollection<StandaloneWaypointViewModel> StandaloneWaypoints { get; set; }

    public StandaloneWaypointsSummaryViewModel() {
      this.StandaloneWaypoints = new ObservableCollection<StandaloneWaypointViewModel>();
    }  // End of ctor

    public void DeleteWaypoints() {

      List<StandaloneWaypointViewModel> selectedItems = new List<StandaloneWaypointViewModel>(this.StandaloneWaypoints.Where(w => w.IsSelected));

      foreach (var q in selectedItems) {
        this.StandaloneWaypoints.Remove(q);
      }

    }  // End of DeleteWaypoints

    protected override void CreateMap() {
      
      base.CreateMap();

      string fileName = Properties.Resources.MapFolderName + "\\StandaloneWaypoints.mapview.html";

      string locations = null;
      string options = null;

      for (int i = 0; i < this.StandaloneWaypoints.Count; i++) {
        if (!string.IsNullOrEmpty(locations)) {
          locations += ",";
          options += ",";
        }
        locations += "new l(" + this.StandaloneWaypoints[i].Latitude.ToString("00.00000") + ", " + this.StandaloneWaypoints[i].Longitude.ToString("00.00000") + ")";
        options += "{title: \"" + this.StandaloneWaypoints[i].Name + "\", color: \"red\"}";
      }

      using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName)) {
        writer.WriteLine(HtmlTemplate.Replace("{PageTitle}", "Standalone Waypoints").Replace("{locations}", locations).Replace("{options}", options));
      }

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = fileName
      });

    }  // End of CreateMap

    private void Export() {

      Utilities.CreateGPXDocument(null,
                                  "Standalone Waypoints",
                                  null,
                                  x => {

                                    foreach (var q in this.StandaloneWaypoints) {

                                      XmlElement waypoint = Utilities.CreateWaypointElement(x, "wpt", q.Latitude, q.Longitude);
                                      x.DocumentElement.AppendChild(waypoint);

                                      waypoint.AppendChild(Utilities.CreateNameElement(x, q.Name));
                                      waypoint.AppendChild(Utilities.CreateDescriptionElement(x, Utilities.AddExportTimestamp(q.Comment)));
                                      waypoint.AppendChild(Utilities.CreateSourceElement(x));
                                      waypoint.AppendChild(Utilities.CreateLinkElement(x));

                                    }

                                  });

    }  // End of Export

  }

}
