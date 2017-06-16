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
    
    public FSH.Enums.Color Color {
      get {
        return trackMetadata.Color;
      }
      set {
        trackMetadata.Color = value;
        OnPropertyChanged("Color");
      }
    }  // End of property Color

    public double Length {
      get {
        return trackMetadata.Length;
      }
    }  // End of property Length

    public string Name {
      get {
        return trackMetadata.Name;
      }
      set {
        trackMetadata.Name = Utilities.TrimmedString(value, false);
        OnPropertyChanged("Name");
      }
    }  // End of property Name

    public int Segments {
      get {
        return trackMetadata.Segments;
      }
    }  // End of Segments

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

      string fileName = Properties.Resources.MapFolderName + "\\" + this.Name + ".mapview.html";

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

        writer.WriteLine(HtmlTemplate.Replace("{TrackName}", this.Name).Replace("{locations}", js));

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
      name.InnerText = this.Name;
      track.AppendChild(name);

      XmlElement source = doc.CreateElement("src");
      source.InnerText = "FSH Editor";
      track.AppendChild(source);

      XmlElement description = doc.CreateElement("desc");
      description.InnerXml = @"<![CDATA[Track length (NM): " + this.trackMetadata.Length + Environment.NewLine +
                              "FSH Editor export date: " + DateTime.UtcNow + " GMT]]>";
      track.AppendChild(description);

      XmlElement link = doc.CreateElement("link");
      XmlAttribute href = doc.CreateAttribute("href");
      href.InnerText = "http://www.okeanvoyaging.com/fsh-editor-download";
      XmlElement text = doc.CreateElement("text");
      text.InnerText = "Archive.FSH data exported by the FSH Editor";

      link.Attributes.Append(href);
      link.AppendChild(text);
      track.AppendChild(link);

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

      doc.Save(Properties.Resources.GPXFolderName + "\\" + this.Name + ".gpx");

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = Properties.Resources.GPXFolderName
      });

    }  // End of Export

  }
}
