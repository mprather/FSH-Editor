/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FSH {

  public class Track : SerializableData {

		private int a;
		private short items;
		private short b;
	  
		public List<TrackPoint> Points { get; set; }

		public Track() {

			this.Points = new List<TrackPoint>();

    }  // End of ctor

    public override ushort CalculateSize() {
      return (ushort) (4 + 2 + 2 + this.Points.Sum(tp => tp.CalculateSize()));
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.a = reader.ReadInt32();
			System.Diagnostics.Debug.Assert(this.a == 0);

			this.items = reader.ReadInt16();
			
			this.b = reader.ReadInt16();
			System.Diagnostics.Debug.Assert(this.b == 0);

			for (int i = 0; i < this.items; i++) {
				TrackPoint tp = new TrackPoint();
				tp.Deserialize(reader);
				this.Points.Add(tp);
			}

			System.Diagnostics.Debug.WriteLine("  (t) a: " + this.a + ", Items: " + this.items + ", b: " + this.b);

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {
			
		  writer.Write(this.a);
			writer.Write(this.items);
			writer.Write(this.b);

			this.Points.ForEach(tp => tp.Serialize(writer));

    }  // End of Serialize

  }  // End of Track class

}
