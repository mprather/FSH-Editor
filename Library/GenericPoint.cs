/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;

namespace FSH {

  public class GenericPoint : SerializableData {
		
	  private short a;
		private short b;
		private short c;
		private short d;
		private short symbol;

    public override ushort CalculateSize() {
      return (ushort) (2 + 2 + 2 + 2 + 2);
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {
			
		  this.a       = reader.ReadInt16();
			this.b       = reader.ReadInt16();
			this.c       = reader.ReadInt16();
			this.d       = reader.ReadInt16();
			this.symbol  = reader.ReadInt16();

		}  // End of Deserialize

		public override void Serialize(BinaryWriter writer) {
			writer.Write(this.a);
			writer.Write(this.b);
			writer.Write(this.c);
			writer.Write(this.d);
			writer.Write(this.symbol);
		}  // End of Serialize

  }  // End of GenericPoint class

}
