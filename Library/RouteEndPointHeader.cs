/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;

namespace FSH {

  public class RouteEndPointHeader : SerializableData {
    
    public int StartLatitude { get; set; }

    public int StartLongitude { get; set; }

    public int EndLatitude { get; set; }

    public int EndLongitude { get; set; }

    private int a;
		private short c;

		// NOTE: this was originally specified as char[], however due to encoding translation 
		//       changes this has been conveted to byte[] to keep all original data 
		//       intact (since the purpose is unknown).
		private byte[] d;

    public override ushort CalculateSize() {
      return (ushort)(4 + 4 + 4 + 4 + 4 + 2 + 24);
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.StartLatitude = reader.ReadInt32();
			this.StartLongitude = reader.ReadInt32();

			this.EndLatitude = reader.ReadInt32();
			this.EndLongitude = reader.ReadInt32();

			this.a = reader.ReadInt32();
			this.c = reader.ReadInt16();
			this.d = reader.ReadBytes(24);

		}  // End of Deserialize

		public override void Serialize(BinaryWriter writer) {
			
		  writer.Write(this.StartLatitude);
			writer.Write(this.StartLongitude);

			writer.Write(this.EndLatitude);
			writer.Write(this.EndLongitude);
			
			writer.Write(this.a);
			writer.Write(this.c);
			writer.Write(this.d);
			
		}  // End of Serialize

  }  // End of RouteEndPointHeader class

}
