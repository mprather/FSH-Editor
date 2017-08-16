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

		/// <summary>
    /// Depth in centimeters
    /// </summary>
    public short Depth { get; set; }

    // ========================================================================
    // Note: This variable appears to be a health state flag. If 0, the lat/lon 
    //       values appear to be legit. If -1, readings are near 0,0.
    // ========================================================================
    private short invalid;

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

    public bool Valid {
      get {
        return this.invalid == 0;
      }
    }

    public override ushort CalculateSize() {
      return (ushort)(4 + 4 + 2 + 2 + 2);
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.north = reader.ReadInt32();
			this.east = reader.ReadInt32();

			this.temperature = reader.ReadUInt16();

			this.Depth = reader.ReadInt16();

			this.invalid = reader.ReadInt16();
      System.Diagnostics.Debug.Assert(this.invalid == 0 || this.invalid == -1, "Proposed Valid flag is not 0 or -1");

      System.Diagnostics.Debug.WriteLine("  (tp) North: " + this.north + ", East:" + this.east + ", Temp: " + this.temperature + ", Depth: " + this.Depth + ", Invalid: " + this.invalid + ", Lat: " + this.Latitude + ", Lon: " + this.Longitude);

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {

			writer.Write(this.north);
			writer.Write(this.east);
			writer.Write(this.temperature);
			writer.Write(this.Depth);
			writer.Write(this.invalid);

    }  // End of Serialize

  }  // End of TrackPoint class

}
