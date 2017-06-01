/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;

namespace FSH {

  public class Timestamp : SerializableData {

		public uint TimeOfDay { get; set; }

		public ushort Date { get; set; }

    public override ushort CalculateSize() {
      return 6;
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.TimeOfDay  = reader.ReadUInt32();
			this.Date       = reader.ReadUInt16();

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {
			writer.Write(this.TimeOfDay);
			writer.Write(this.Date);
    }  // End of Serialize

  }  // End of Timestamp class

}
