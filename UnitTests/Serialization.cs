using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FSH;

namespace UnitTests {

  [TestClass]
  public class Serialization {

    private static int BufferLength                  = 4096;

    [TestMethod]
    public void BasicExportTest() {

      ArchiveFile archive = new ArchiveFile();

      using (FileStream originalStream = new FileStream(@"data\\archive - Sept 2016.fsh", FileMode.Open))
      using (BinaryReader originalReader = new BinaryReader(originalStream, Encoding.ASCII)) {

        // Load the archive file...
        archive.Deserialize(originalReader);

        // Now let's export the file (mostly) untouched...
        // ----------------------------------------------------------------------
        // Note: TrackMetadata.Name is null padded. This is a difference.
        // ----------------------------------------------------------------------
        using (FileStream exportStream = new FileStream("exported.fsh", FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(exportStream, Encoding.ASCII)) {
          archive.Serialize(writer);
        }

        // Let's compare the two files...
        using (FileStream exportedStream = new FileStream(@"exported.fsh", FileMode.Open))
        using (BinaryReader exportedReader = new BinaryReader(exportedStream, Encoding.ASCII)) {

          Assert.IsTrue(originalStream.Length == exportedStream.Length, "File size does not match");

          // Rewind the original stream...
          originalStream.Seek(0, SeekOrigin.Begin);

          byte[] originalBuffer = new byte[BufferLength];
          byte[] exportedBuffer = new byte[BufferLength];

          long position = 0;

          while (originalReader.PeekChar() != -1 && exportedReader.PeekChar() != -1) {
            
            // ----------------------------------------------------------------------
            // Note: this method doesn't work well if the errors are within the boundary
            //       of the buffer length. it works ok for what is being tested though.
            // ----------------------------------------------------------------------

            originalBuffer = originalReader.ReadBytes(BufferLength);
            exportedBuffer = exportedReader.ReadBytes(BufferLength);

            // Let's walk through bytes...
            for (int i = 0; i < originalBuffer.Length; i++) {

              // ----------------------------------------------------------------------
              // Note: TrackMetadata.Name is null padded but the original file has 
              //       garbage chars. The nulls should be the only differences 
              //       found in the exported file.
              // ----------------------------------------------------------------------
              switch (position++) {
                case 79432:
                  i += 2;
                  position += 2;
                  break;
                case 87222:
                  i++;
                  position++;
                  break;
                case 91433:
                  i += 7;
                  position += 7;
                  break;
                default:
                  Assert.IsTrue(originalBuffer[i] == exportedBuffer[i], "Difference located at " + position + ". " + originalBuffer[i].ToString("X") + " vs. " + exportedBuffer[i].ToString("X"));
                  break;
              }

            }  // End of for i

          }

        }  // End of using exportedStream

      }  // End of using originalStream

    }  // End of BasicExportTest

  }  // End of Serialization class

}  // End of the UnitTests namespace
