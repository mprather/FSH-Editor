﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH {
	
  public class TrackMetadata : SerializableData {

		private char a;

		private short items;
		private short items2;
		private short b;
    private int lengthInMeters;

		private int startNorth;
		private int startEast;
		private ushort startTemperature;
		private int startDepth;

		private int endNorth;
		private int endEast;
		private ushort endTemperature;
		private int endDepth;

		private char i;

		private char[] name;
		
		private char j;
    
		private ulong[] guids;

		public string Name { get; set; }
    
    /// <summary>
    /// Length in NM
    /// </summary>
    public double Length { 
      get {
        return this.lengthInMeters * .00053996;
      }
      set {
        this.lengthInMeters = (int) (value / .00053996);
      } 
    }

    public byte Segments { get; set; }

    public List<TrackPoint> Points { get; set; }

    public override ushort CalculateSize() {
      return (ushort)(1 + 2 + 2 + 2 + 2 + 2 + 4 + 4 + 2 + 4 + 4 + 4 + 2 + 4 + 1 + 16 + 1 + 1 + this.guids.Length * 8);
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.a = reader.ReadChar();
			this.items = reader.ReadInt16();
			this.items2 = reader.ReadInt16();
			this.b = reader.ReadInt16();
      this.lengthInMeters = reader.ReadInt32();

			this.startNorth = reader.ReadInt32();
			this.startEast = reader.ReadInt32();
			this.startTemperature = reader.ReadUInt16();
			this.startDepth = reader.ReadInt32();

			this.endNorth = reader.ReadInt32();
			this.endEast = reader.ReadInt32();
			this.endTemperature = reader.ReadUInt16();
			this.endDepth = reader.ReadInt32();

			this.i = reader.ReadChar();

			this.name = reader.ReadChars(16);
			this.Name = CleanString(this.name);

			this.j = reader.ReadChar();

			this.Segments = reader.ReadByte();

			this.guids = new ulong[this.Segments];

			for (int i = 0; i < this.Segments; i++) {
				
        this.guids[i] = reader.ReadUInt64();
				System.Diagnostics.Debug.WriteLine("  (tm-g) ID: " + this.guids[i]);
        
      }

      System.Diagnostics.Debug.WriteLine("  (tm) Name: " + this.Name.Replace('\0', ' ') + ", Segments: " + this.Segments + ", Items: " + this.items);

    }  // End of Deserialize

    public List<TrackPoint> GetAllTrackPoints(List<Flob> parentCollection) {

      List<TrackPoint> list = new List<TrackPoint>();
      int segmentCounter = 0;

      foreach (Flob f in parentCollection) {
        foreach (Block block in f.Blocks.Where(b => b.Data is Track)) {
          if (this.guids.Contains(block.ID)) {
            segmentCounter++;
            Track t = block.Data as Track;
            list.AddRange(t.Points);
          }
        }
      }

      if (segmentCounter != this.Segments) {
        throw new ApplicationException("Error: Actual track segments do not match with Track Metadata track segment value");
      }

      return list;

    }  // End of GetAllTrackPoints

    public override void Serialize(BinaryWriter writer) {

			writer.Write(this.a);
			writer.Write(this.items);
			writer.Write(this.items2);
			writer.Write(this.b);
			writer.Write(this.lengthInMeters);

			writer.Write(this.startNorth);
			writer.Write(this.startEast);
			writer.Write(this.startTemperature);
			writer.Write(this.startDepth);


			writer.Write(this.endNorth);
			writer.Write(this.endEast);
			writer.Write(this.endTemperature);
			writer.Write(this.endDepth);

			writer.Write(this.i);

      // ----------------------------------------------------------------------
      // Note: We have found the E-120W does not return clean data for this field.
      //       In other words, there may be garbage characters persisted in 
      //       between the null-terminator and MaximumStringLength. We have 
      //       chosen to fully pad the space with null.
      // ----------------------------------------------------------------------
      this.name = NullPaddedString(this.Name);

			writer.Write(this.name);
      
			writer.Write(this.j);

			writer.Write(this.Segments);

			for (int i = 0; i < this.guids.Length; i++) {
				writer.Write(this.guids[i]);
			}

    }  // End of Serialize

  }  // End of TrackMetadata class

}
