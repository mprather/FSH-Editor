using System;
using System.Xml;

using FSH;

namespace Editor {

  public static class Utilities {
    
    public static void CreateGPXDocument(string itemType, string itemName, string itemDescription, Action<XmlDocument> x) {
      
      XmlDocument doc = new XmlDocument();

      XmlElement root = doc.CreateElement("gpx");
      root.SetAttribute("creator", "FSH Editor");
      doc.AppendChild(root);

      if (!String.IsNullOrEmpty(itemType)) {
        
        XmlElement mainElement = doc.CreateElement(itemType);
        root.AppendChild(mainElement);

        mainElement.AppendChild(CreateNameElement(doc, itemName));

        if (!String.IsNullOrEmpty(itemDescription)) {
          mainElement.AppendChild(CreateDescriptionElement(doc, itemDescription));
        }

        mainElement.AppendChild(CreateSourceElement(doc));
        mainElement.AppendChild(CreateLinkElement(doc));

      }

      if (x != null) {
        x(doc);
      }

      // Ensure the holding folder exists...
      System.IO.Directory.CreateDirectory(Properties.Resources.GPXFolderName);

      doc.Save(Properties.Resources.GPXFolderName + "\\" + itemName + ".gpx");

      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
        UseShellExecute = true,
        FileName = Properties.Resources.GPXFolderName
      });
      
    }  // End of CreateGPXDocument

    public static XmlElement CreateDescriptionElement(XmlDocument x, string cdata) {
      
      XmlElement description = x.CreateElement("desc");
      description.InnerXml   = @"<![CDATA[" + cdata + "]]>";
      return description;

    }  // End of CreateDescriptionElement

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
