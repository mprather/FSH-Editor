using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FSH;

namespace UnitTests {
	
  [TestClass]
	public class Deserialization {
		
	  [TestMethod]
		public void GeneralStructure() {

			ArchiveFile archive = new ArchiveFile();

			using (FileStream stream = new FileStream(@"data\\archive - Sept 2016.fsh", FileMode.Open)) {
				using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII)) {
					archive.Deserialize(reader);
				}
			}

			Assert.IsTrue(archive.Flobs.Count == 128, "Unexpected flob count encountered");
      Assert.IsTrue(archive.Flobs.Any(f => f.Blocks.Any(b => b.Status == 0x00)), "Archive data was not located");
			Assert.IsTrue(archive.Flobs[0].FlobType == FlobType.Data);
			Assert.IsTrue(archive.Flobs[1].FlobType == FlobType.Last);
			Assert.IsTrue(archive.Flobs[2].FlobType == FlobType.Empty);

		}

	}

}
