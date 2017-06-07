/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.IO;

namespace FSH {

  public class TrackPoint : SerializableData {
    
    private int north;
		private int east;

		private ushort temperature;

		private short depth;

    // ========================================================================
    // Note: This variable appears to be a health state flag. If 0, the lat/lon 
    //       values appear to be legit. If -1, readings are near 0,0.
    // ========================================================================
    private short c;

    // ========================================================================
    // The following variables are backing fields used to hold the 
    // calculated mercator values taken from North and East values.
    // ========================================================================
    private double latitude                          = Double.MaxValue;
    private double longitude                         = Double.MaxValue;
    // ========================================================================

    public double Latitude {
      get {
        if (this.latitude == Double.MaxValue) {
          this.latitude = Mercator.Latitude(this.north);
        }
        return this.latitude;
      }
    }  // End of property Latitude

		public double Longitude {
		  get {
        if (this.longitude == Double.MaxValue) {
          this.longitude = Mercator.Longitude(this.east);
        }
        return this.longitude;
			}
		}  // End of property Longitude

    public override ushort CalculateSize() {
      return (ushort)(4 + 4 + 2 + 2 + 2);
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.north = reader.ReadInt32();
			this.east = reader.ReadInt32();

			this.temperature = reader.ReadUInt16();

			this.depth = reader.ReadInt16();

			this.c = reader.ReadInt16();
      System.Diagnostics.Debug.Assert(this.c == 0 || this.c == -1, "Proposed Valid flag is not 0 or -1");

      System.Diagnostics.Debug.WriteLine("  (tp) North: " + this.north + ", East:" + this.east + ", Temp: " + this.temperature + ", Depth: " + this.depth + ", c: " + this.c + ", Lat: " + this.Latitude + ", Lon: " + this.Longitude);

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {

			writer.Write(this.north);
			writer.Write(this.east);
			writer.Write(this.temperature);
			writer.Write(this.depth);
			writer.Write(this.c);

    }  // End of Serialize

  }  // End of TrackPoint class

}
