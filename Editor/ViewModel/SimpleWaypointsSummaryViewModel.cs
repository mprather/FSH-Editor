/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.ObjectModel;

namespace Editor.ViewModel {

  public class SimpleWaypointsSummaryViewModel : MappingViewModel {
    
    public ObservableCollection<SimpleWaypointViewModel> SimpleWaypoints { get; set; }

    public SimpleWaypointsSummaryViewModel() {
      this.SimpleWaypoints = new ObservableCollection<SimpleWaypointViewModel>();
    }  // End of ctor

    protected override void CreateMap() {
      
      base.CreateMap();

      string fileName = Properties.Resources.MapFolderName + "\\SimpleWaypoints.mapview.html";

      string locations = null;
      string options = null;

      for (int i = 0; i < this.SimpleWaypoints.Count; i++) {
        if (!string.IsNullOrEmpty(locations)) {
          locations += ",";
          options += ",";
        }
        locations += "new Microsoft.Maps.Location(" + this.SimpleWaypoints[i].Latitude.ToString("00.00000") + ", " + this.SimpleWaypoints[i].Longitude.ToString("00.00000") + ")";
        options += "{title: \"" + this.SimpleWaypoints[i].Name + "\", color: \"red\"}";
      }

      using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName)) {
        writer.WriteLine(HtmlTemplate.Replace("{PageTitle}", "Simple Waypoints").Replace("{locations}", locations).Replace("{options}", options));
      }

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = fileName
      });

    }  // End of CreateMap
  }

}
