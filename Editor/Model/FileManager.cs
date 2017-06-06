/*
Copyright © 2017 Okean Voyaging LLC
Author: Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;
using System.Text;

using FSH;

namespace Editor.Model {

  public class FileManager {

    public ArchiveFile ArchiveFile { get; set; }

    public string FileName { get; set; }

    public long FileSize { get; set; }

    public bool Open(string fileName) {

      FileInfo file = new FileInfo(fileName);

      if (file.Extension != ".fsh") {
        return false;
      }

      this.ArchiveFile = new ArchiveFile();
      
      try {
        
        using (FileStream stream = new FileStream(fileName, FileMode.Open)) {
          using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII)) {
            this.ArchiveFile.Deserialize(reader);
          }
        }

        this.FileName = file.FullName;
        this.FileSize = file.Length;

      }
      catch {
        
        // ----------------------------------------------------------------------------
        // Note: this could be improved but for the time being a generic exception is sufficient
        // ----------------------------------------------------------------------------
        return false;

      }
      return true;

    }  //  End of Open

    public void Save(string fileName) {

      // Ensure the folder exists...
      Directory.CreateDirectory(Properties.Resources.ExportFolderName);

      fileName = Path.Combine(Properties.Resources.ExportFolderName, fileName);

      using (FileStream stream = new FileStream(fileName, FileMode.Create)) {
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII)) {
          this.ArchiveFile.Serialize(writer);
        }
      }

    }  // End of Save

  }
}
