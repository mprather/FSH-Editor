using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FSH.Enums;

namespace FSH {

  public class TrackMetadata : SerializableData {

		private char a;

    // ----------------------------------------------------------------------------
    // NOTE: Per the documentation, the following 2 fields are item counts that 
    //       always match (listed as fields cnt and _cnt). This is not the case if 
    //       the track count exceeds the maximum allowable items. In the case of 
    //       E-120W, the max items is 10k before the track starts to roll over. 
    //       Therefore, these two fields have been identified as max and roll over counts.
    // ----------------------------------------------------------------------------
    private short maximumItems;
		private short rolloverItems;
    // ----------------------------------------------------------------------------

    private short b;

		private int startNorth;
		private int startEast;
		private ushort startTemperature;
		private int startDepth;

		private int endNorth;
		private int endEast;
		private ushort endTemperature;
		private int endDepth;

		private char[] name;
    
    private char trackColor;

		private char j;

    public byte segments;

    private ulong[] guids;

    /// <summary>
    /// Length in Meters
    /// </summary>
    public int Length { get; set; }

    public string Name { get; set; }

    public Color Color { 
      get {
        return (Color)this.trackColor;
      }
      set {
        this.trackColor = (char) value;
      } 
    }

    public override ushort CalculateSize() {
      return (ushort)(1 + 2 + 2 + 2 + 2 + 2 + 4 + 4 + 2 + 4 + 4 + 4 + 2 + 4 + 1 + 16 + 1 + 1 + this.guids.Length * 8);
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.a = reader.ReadChar();
      System.Diagnostics.Debug.Assert(this.a == 1);

      this.maximumItems = reader.ReadInt16();
			this.rolloverItems = reader.ReadInt16();

      this.b = reader.ReadInt16();
      System.Diagnostics.Debug.Assert(this.b == 0);

      this.Length = reader.ReadInt32();

			this.startNorth = reader.ReadInt32();
			this.startEast = reader.ReadInt32();
			this.startTemperature = reader.ReadUInt16();
			this.startDepth = reader.ReadInt32();

			this.endNorth = reader.ReadInt32();
			this.endEast = reader.ReadInt32();
			this.endTemperature = reader.ReadUInt16();
			this.endDepth = reader.ReadInt32();

			this.trackColor = reader.ReadChar();

			this.name = reader.ReadChars(16);
			this.Name = CleanString(this.name);

			this.j = reader.ReadChar();
      System.Diagnostics.Debug.Assert(this.j == 0);

      this.segments = reader.ReadByte();

			this.guids = new ulong[this.segments];

			for (int i = 0; i < this.segments; i++) {
				
        this.guids[i] = reader.ReadUInt64();
				System.Diagnostics.Debug.WriteLine("  (tm-g) ID: " + this.guids[i]);
        
      }

      System.Diagnostics.Debug.WriteLine("  (tm) Name: " + this.Name.Replace('\0', ' ') + ", Segments: " + this.segments + ", Items: " + this.maximumItems + ", Rollover: " + (this.rolloverItems - this.maximumItems));

    }  // End of Deserialize

    public List<TrackPoint> GetAllTrackPoints(List<Flob> flobs) {

      List<TrackPoint> list = new List<TrackPoint>();
      int segmentCounter = 0;

      foreach (Flob f in flobs) {
        foreach (Block block in f.Blocks.Where(b => b.Data is Track)) {
          if (this.guids.Contains(block.ID)) {
            segmentCounter++;
            Track t = block.Data as Track;
            list.AddRange(t.Points);
          }
        }
      }

      if (segmentCounter != this.segments) {
        throw new ApplicationException("Error: Actual track segments do not match with Track Metadata track segment value");
      }

      return list;

    }  // End of GetAllTrackPoints

    public override void Serialize(BinaryWriter writer) {

			writer.Write(this.a);
			writer.Write(this.maximumItems);
			writer.Write(this.rolloverItems);
			writer.Write(this.b);
			writer.Write(this.Length);

			writer.Write(this.startNorth);
			writer.Write(this.startEast);
			writer.Write(this.startTemperature);
			writer.Write(this.startDepth);


			writer.Write(this.endNorth);
			writer.Write(this.endEast);
			writer.Write(this.endTemperature);
			writer.Write(this.endDepth);

			writer.Write(this.trackColor);

      // ----------------------------------------------------------------------
      // NOTE: We have found the E-120W does not return clean data for this field.
      //       In other words, there may be garbage characters persisted in 
      //       between the null-terminator and MaximumStringLength. We have 
      //       chosen to fully pad the space with null.
      // ----------------------------------------------------------------------
      this.name = NullPaddedString(this.Name, false);

			writer.Write(this.name);
      
			writer.Write(this.j);

			writer.Write(this.segments);

			for (int i = 0; i < this.guids.Length; i++) {
				writer.Write(this.guids[i]);
			}

    }  // End of Serialize

  }  // End of TrackMetadata class

}
