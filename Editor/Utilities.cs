using System;
using System.IO;
using System.Xml;

using FSH;

namespace Editor {

  public static class Utilities {

    public static string AddExportTimestamp(string text) {
      return text + (String.IsNullOrEmpty(text) ? "" : Environment.NewLine) + "FSH Editor export date: " + DateTime.UtcNow + " GMT";
    }  // End of ExportTimestamp

    public static void CreateGPXDocument(string itemType, string itemName, string itemDescription, Action<XmlDocument> x) {
      
      XmlDocument doc = new XmlDocument();

      XmlElement root = doc.CreateElement("gpx");
      root.SetAttribute("creator", "FSH Editor");
      doc.AppendChild(root);

      XmlElement metadata = doc.CreateElement("metadata");
      metadata.AppendChild(CreateNameElement(doc, "FSH Editor GPX export"));
      if (Properties.Settings.Default.IncludeDepth) {
        metadata.AppendChild(CreateDescriptionElement(doc, AddExportTimestamp("Depth measured in " + Properties.Settings.Default.DepthUnits)));
      } else {
        metadata.AppendChild(CreateDescriptionElement(doc, AddExportTimestamp("")));
      }

      root.AppendChild(metadata);

      if (!String.IsNullOrEmpty(itemType)) {
        
        XmlElement mainElement = doc.CreateElement(itemType);
        root.AppendChild(mainElement);

        mainElement.AppendChild(CreateNameElement(doc, itemName));
        mainElement.AppendChild(CreateDescriptionElement(doc, AddExportTimestamp(itemDescription)));
        mainElement.AppendChild(CreateSourceElement(doc));
        mainElement.AppendChild(CreateLinkElement(doc));

      }

      if (x != null) {
        x(doc);
      }

      string folderName    = null;
      bool saveAsLayerFile = false;

      if (itemType == null) {
        saveAsLayerFile = Properties.Settings.Default.SaveWaypointAsLayer;
      } else {
        if (itemType == "trk") {
          saveAsLayerFile = Properties.Settings.Default.SaveTrackAsLayer;
        }
      }

      if (saveAsLayerFile) {
        folderName = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "opencpn\\Layers");
      } else {
        folderName = Properties.Resources.GPXFolderName;
      }
      
      // Ensure the holding folder exists...
      System.IO.Directory.CreateDirectory(folderName);

      doc.Save(folderName + "\\" + itemName + ".gpx");

      //if (!saveAsLayerFile) {
        
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
          UseShellExecute  = true,
          FileName         = folderName
        });

      //}

    }  // End of CreateGPXDocument

    public static XmlElement CreateCommentElement(XmlDocument x, string text) {
      XmlElement comment = x.CreateElement("cmt");
      comment.InnerText  = text;
      return comment;
    }  // End of CreateCommentElement

    public static XmlElement CreateDescriptionElement(XmlDocument x, string cdata) {
      
      XmlElement description = x.CreateElement("desc");
      description.InnerXml   = @"<![CDATA[" + cdata + "]]>";
      return description;

    }  // End of CreateDescriptionElement

    public static XmlElement CreateElevationElement(XmlDocument x, double depth) {
      XmlElement elevation = x.CreateElement("ele");

      switch (Properties.Settings.Default.DepthUnits) {
        case Enums.DepthUnits.Feet:
          depth /= 2.54 * 12;
          break;
        case Enums.DepthUnits.Meters:
          depth /= 100;
          break;
      }

      depth += Properties.Settings.Default.DepthMeterOffset;

      elevation.InnerText  = depth.ToString();
      return elevation;
    }  // End of CreateElevationElement

    public static XmlElement CreateLinkElement(XmlDocument x) {
      
      XmlElement link    = x.CreateElement("link");
      XmlAttribute href  = x.CreateAttribute("href");
      href.InnerText     = "http://www.okeanvoyaging.com/fsh-editor-download";
      XmlElement text    = x.CreateElement("text");
      text.InnerText     = "Archive.FSH data exported by the FSH Editor";

      link.Attributes.Append(href);
      link.AppendChild(text);

      return link;

    }  // End of CreateLinkElement

    public static XmlElement CreateNameElement(XmlDocument x, string text) {

      XmlElement name  = x.CreateElement("name");
      name.InnerText   = text;
      return name;

    }  // End of CreateNameElement

    public static XmlElement CreateSourceElement(XmlDocument x) {
      
      XmlElement source  = x.CreateElement("src");
      source.InnerText   = "FSH Editor";

      return source;

    }  // End of CreateSourceElement

    public static XmlElement CreateWaypointElement(XmlDocument x, string elementName, double lat, double lon) {
      
      XmlElement waypoint = x.CreateElement(elementName);
      waypoint.SetAttribute("lat", lat.ToString());
      waypoint.SetAttribute("lon", lon.ToString());

      return waypoint;

    }  // End of CreateWaypointElement

    public static string TrimmedString(string value, bool isComment) {
      int maxLength = isComment ? SerializableData.MaximumCommentLength : SerializableData.MaximumNameLength;
      if (value.Length > maxLength) {
        value = value.Substring(0, maxLength);
      }
      return value;
    }  // End of TrimmedString

  }
}
