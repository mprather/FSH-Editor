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

      XmlElement mainElement = doc.CreateElement(itemType);
      root.AppendChild(mainElement);

      XmlElement name = doc.CreateElement("name");
      name.InnerText = itemName;
      mainElement.AppendChild(name);

      if (!String.IsNullOrEmpty(itemDescription)) {
        XmlElement description = doc.CreateElement("desc");
        description.InnerXml = @"<![CDATA[" + itemDescription + "]]>";
        mainElement.AppendChild(description);
      }

      XmlElement source = doc.CreateElement("src");
      source.InnerText = "FSH Editor";
      mainElement.AppendChild(source);

      XmlElement link = doc.CreateElement("link");
      XmlAttribute href = doc.CreateAttribute("href");
      href.InnerText = "http://www.okeanvoyaging.com/fsh-editor-download";
      XmlElement text = doc.CreateElement("text");
      text.InnerText = "Archive.FSH data exported by the FSH Editor";

      link.Attributes.Append(href);
      link.AppendChild(text);
      mainElement.AppendChild(link);

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

    public static string TrimmedString(string value, bool isComment) {
      int maxLength = isComment ? SerializableData.MaximumCommentLength : SerializableData.MaximumNameLength;
      if (value.Length > maxLength) {
        value = value.Substring(0, maxLength);
      }
      return value;
    }  // End of TrimmedString

  }
}
