using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FSH;

namespace UnitTests {
	
  [TestClass]
	public class Serialization {
		
	  [TestMethod]
		public void BasicExportTest()  {
			
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

          byte originalByte;
          byte exportedByte;

          while (originalReader.PeekChar() != -1 && exportedReader.PeekChar() != -1) {

            originalByte = originalReader.ReadByte();
            exportedByte = exportedReader.ReadByte();

            // ----------------------------------------------------------------------
            // Note: TrackMetadata.Name is null padded but the original file has 
            //       garbage chars. The nulls should be the only differences 
            //       found in the exported file.
            // ----------------------------------------------------------------------
            switch (originalStream.Position) {
              case 79433:
                originalStream.Seek(2, SeekOrigin.Current);
                exportedStream.Seek(2, SeekOrigin.Current);
                break;
              case 87223:
                break;
              case 91434:
                originalStream.Seek(7, SeekOrigin.Current);
                exportedStream.Seek(7, SeekOrigin.Current);
                break;
              default:
                Assert.IsTrue(originalByte == exportedByte, "Difference located at " + originalStream.Position + ". " + originalByte.ToString("X") + " vs. " + exportedByte.ToString("X"));
                break;
            }

          }

        }  // End of using exportedStream

      }  // End of using originalStream

    }  // End of BasicExportTest

  }  // End of Serialization class

}  // End of the UnitTests namespace
