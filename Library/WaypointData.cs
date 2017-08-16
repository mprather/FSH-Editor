/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.IO;
using System.Linq;

namespace FSH {
  
	public class WaypointData : SerializableData {

		private char[] rawName                           = null;
		private char[] rawComment                        = null;

    private byte nameLength;
    private byte commentLength;
    
    private char[] d;
    private char i;
    private int j;

    private string name                              = null;
    private string comment                           = null;

    private int north;
    private int east;

    // ========================================================================
    // The following variables are backing fields used to hold the 
    // calculated mercator values taken from North and East values.
    // ========================================================================
    private double latitude = Double.MaxValue;
    private double longitude = Double.MaxValue;
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
          this.longitude = Mercator.Longitude(east);
        }
        return this.longitude;
      }
    }  // End of property Longitude

    public char Symbol { get; set; }

		public ushort Temperature { get; set; }

    /// <summary>
    /// Depth in CM
    /// </summary>
		public int Depth { get; set; }

		public Timestamp Timestamp { get; set; }
		
		public string Name { 
      get {
        return this.name;
      }
      set {
        this.nameLength = (byte) TrimString(value, ref this.name, ref this.rawName, false);
      }
    }  // End of property Name

    public string Comment {
      get {
        return this.comment;
      }
      set {
        this.commentLength = (byte)TrimString(value, ref this.comment, ref this.rawComment, true);
      }
    }  // End of property Comment

    public override ushort CalculateSize() {
      return (ushort) (4 + 4 + 12 + 1 + 2 + 4 + this.Timestamp.CalculateSize() + 1 + 1 + 1 + 4 + this.nameLength + this.commentLength);
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.north         = reader.ReadInt32();
			this.east          = reader.ReadInt32();
			
			this.d             = reader.ReadChars(12);
			System.Diagnostics.Debug.Assert(this.d.All(x => x == 0x0));

			this.Symbol        = reader.ReadChar();
			this.Temperature   = reader.ReadUInt16();
			this.Depth         = reader.ReadInt32();

			this.Timestamp     = new Timestamp();
			this.Timestamp.Deserialize(reader);

      // ----------------------------------------------------------------------
      // NOTE: this value changes between 0 and 1. format spec reports it stays
      //       at zero but that is not the case with the E-120W.
      // ----------------------------------------------------------------------
      this.i             = reader.ReadChar();
      //System.Diagnostics.Debug.Assert(this.i == 0);
      
      this.nameLength    = reader.ReadByte();
      this.commentLength = reader.ReadByte();

			this.j             = reader.ReadInt32();
			System.Diagnostics.Debug.Assert(this.j == 0, "Expected J value of 0 not found");

			this.rawName       = reader.ReadChars(this.nameLength);
			this.rawComment    = reader.ReadChars(this.commentLength);

			this.name          = new string(this.rawName);
			this.comment       = new string(this.rawComment);

			System.Diagnostics.Debug.WriteLine("  (wpd) " + this.Name + ": North: " + this.north + ", East: " + this.east + ", Depth: " + this.Depth + ", Symbol: " + Convert.ToInt32(this.Symbol).ToString("X") + " --");

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {

			writer.Write(this.north);
			writer.Write(this.east);
			writer.Write(this.d);
			writer.Write(this.Symbol);
			writer.Write(this.Temperature);
			writer.Write(this.Depth);
			this.Timestamp.Serialize(writer);
			writer.Write(this.i);
			writer.Write(this.nameLength);
			writer.Write(this.commentLength);
			writer.Write(this.j);
			writer.Write(this.rawName);
			writer.Write(this.rawComment);

    }  // End of Serialize

  }  // End of WaypointData class

}
