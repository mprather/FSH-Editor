﻿/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml;

using FSH;

namespace Editor.ViewModel {

  public class TrackMetadataViewModel : PropertyChangedBase {
    
    public static string HtmlTemplate                = Editor.Properties.Resources.TracksHtmlMapTemplate.Replace("{MapServiceKey}", Editor.Properties.Resources.MapServiceKey);

    private FSH.TrackMetadata trackMetadata;

    public ICommand CreateMapCommand {
      get {
        return new DelegateCommand<TrackMetadataViewModel>(
          "CreateMapCommand",
          parameter => {
            if (parameter != null) {
              parameter.CreateMap();
            }
          },
          DelegateCommand<RouteViewModel>.DefaultCanExecute
        );
      }
    }  // End of property CreateMapCommand

    public ICommand ExportCommand {
      get {
        return new DelegateCommand<TrackMetadataViewModel>(
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
    
    public string TrackName {
      get {
        return trackMetadata.Name;
      }
      set {
        if (value.Length > SerializableData.MaximumStringLength) {
          value = value.Substring(0, SerializableData.MaximumStringLength);
        }
        trackMetadata.Name = value;
        OnPropertyChanged("TrackName");
      }
    }  // End of property TrackName

    public string Info {
      get {
        return trackMetadata.Length.ToString("0.00") + " NM, " + this.TrackPointViewModels.Count + " points, " + trackMetadata.Segments + " segments";
      }
    }  // End of property Info

    public ObservableCollection<TrackPointViewModel> TrackPointViewModels { get; set; }

    public TrackMetadataViewModel(FSH.SerializableData data, List<Flob> parentCollection) {

      this.TrackPointViewModels = new ObservableCollection<TrackPointViewModel>();
      
      TrackMetadata tm = data as TrackMetadata;

      this.trackMetadata = tm;

      trackMetadata.GetAllTrackPoints(parentCollection).ForEach(tp => {
        this.TrackPointViewModels.Add(new TrackPointViewModel(tp));
      });


    }  // End of ctor

    private void CreateMap() {

      string fileName = Properties.Resources.MapFolderName + "\\" + this.TrackName + ".mapview.html";

      // Ensure the holding folder exists...
      System.IO.Directory.CreateDirectory(Properties.Resources.MapFolderName);

      using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName)) {

        string js = null;

        foreach (var q in this.TrackPointViewModels) {
          if (Math.Abs(q.Latitude) < 0.00001 || Math.Abs(q.Longitude) < 0.00001) {
            System.Diagnostics.Debug.WriteLine("Zero Points");
            continue;
          }
          if (!String.IsNullOrEmpty(js)) {
            js += ",";
          }
           js += "new Microsoft.Maps.Location(" + q.Latitude.ToString("00.00000") + "," + q.Longitude.ToString("00.00000") + ")";
        }

        writer.WriteLine(HtmlTemplate.Replace("{TrackName}", this.TrackName).Replace("{locations}", js));

      }  // End of using writer mapview

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = fileName
      });

    }  // End of CreateMap

    private void Export() {
      
      XmlDocument doc = new XmlDocument();

      XmlElement gpx = doc.CreateElement("gpx");
      gpx.SetAttribute("creator", "FSH Editor");
      doc.AppendChild(gpx);

      XmlElement track = doc.CreateElement("trk");
      gpx.AppendChild(track);

      XmlElement name = doc.CreateElement("name");
      name.InnerText = this.TrackName;
      track.AppendChild(name);

      XmlElement source = doc.CreateElement("src");
      source.InnerText = "Exported using the FSH Editor";
      track.AppendChild(source);

      XmlElement comment = doc.CreateElement("cmt");
      comment.InnerText = "Track length (NM): " + this.trackMetadata.Length + ", Export Timestamp: " + DateTime.UtcNow;
      track.AppendChild(comment);

      XmlElement segment = doc.CreateElement("trkseg");
      track.AppendChild(segment);

      foreach (var q in this.TrackPointViewModels) {
        if (Math.Abs(q.Latitude) < 0.00001 || Math.Abs(q.Longitude) < 0.00001) {
          System.Diagnostics.Debug.WriteLine("Errant data (" + q.Latitude + "," + q.Longitude + ")");
          continue;
        }
        
        XmlElement point = doc.CreateElement("trkpt");
        point.SetAttribute("lat", q.Latitude.ToString());
        point.SetAttribute("lon", q.Longitude.ToString());

        segment.AppendChild(point);

      }

      // Ensure the holding folder exists...
      System.IO.Directory.CreateDirectory(Properties.Resources.GPXFolderName);

      doc.Save(Properties.Resources.GPXFolderName + "\\" + this.TrackName + ".gpx");

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = Properties.Resources.GPXFolderName
      });

    }  // End of Export

  }
}
