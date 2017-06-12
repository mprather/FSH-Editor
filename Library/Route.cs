/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FSH {

  public class Route : SerializableData {

		private short a;

		private char rawNameLength;
		private char commentLength;

		private short idCount;

		private ushort b;

		private char[] rawName;
		private char[] rawComment;

    private string name                              = null;
    private string comment                           = null;

		public List<long> Guids { get; set; }

		public string Name {
      get {
        if (this.name == null) {
          this.name = new string(this.rawName);
        }
        return this.name;
      }
      set {
        this.rawNameLength = (char)TrimString(value, ref this.name, ref this.rawName, false);
      }
    }  // End of property Name

		public string Comment { 
      get {
        
        if (this.comment == null) {
          this.comment = new string(this.rawComment);
        }
        return this.comment;

      }
      set {
        // ----------------------------------------------------------------------------        
        // NOTE: Since the E-120W does not use comments within Routes, there is no way 
        //       to determine if comments are limited to 32 characters. Simply using 
        //       the same pattern established with other comments.
        // ----------------------------------------------------------------------------        
        this.commentLength = (char)TrimString(value, ref this.comment, ref this.rawComment, true);
      } 
    }  // End of property Comment

		public RouteEndPointHeader Endpoints { get; set; }

		public RouteWaypointHeader WaypointSummary { get; set; }

		public List<GenericPoint> Points { get; set; }

		public Route() {
			
		  this.Guids     = new List<long>();
			this.Points    = new List<GenericPoint>();

		}  // End of ctor

    public override ushort CalculateSize() {
      
      int length = 2 + 1 + 1 + 2 + 2 + 
                   this.rawNameLength + 
                   this.commentLength + 
                   this.Guids.Count * 8 + 
                   this.Endpoints.CalculateSize() +
                   this.Points.Sum(p => p.CalculateSize()) + 
                   this.WaypointSummary.CalculateSize();

      return (ushort)length;

    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.a             = reader.ReadInt16();
			this.rawNameLength = reader.ReadChar();
			this.commentLength = reader.ReadChar();
			this.idCount       = reader.ReadInt16();
			this.b             = reader.ReadUInt16();

			this.rawName       = reader.ReadChars(this.rawNameLength);
			this.rawComment       = reader.ReadChars(this.commentLength);

			for (int i = 0; i < this.idCount; i++) {
				this.Guids.Add(reader.ReadInt64());
			}

			this.Endpoints = new RouteEndPointHeader();
			this.Endpoints.Deserialize(reader);

			for (int i = 0; i < this.idCount; i++) {
				GenericPoint point = new GenericPoint();
				point.Deserialize(reader);
				this.Points.Add(point);
			}

			this.WaypointSummary = new RouteWaypointHeader();
			this.WaypointSummary.Deserialize(reader);

			System.Diagnostics.Debug.WriteLine("  (r) Name: " + this.Name.Replace('\0', '.') + ", Comment: " + this.Comment + ", a:" + this.a);

		}  // End of Deserialize

		public override void Serialize(BinaryWriter writer) {
			
		  writer.Write(this.a);
			writer.Write(this.rawNameLength);
			writer.Write(this.commentLength);
			writer.Write(this.idCount);
			writer.Write(this.b);

      writer.Write(this.rawName);
			writer.Write(this.rawComment);

			this.Guids.ForEach(g => writer.Write(g));

			this.Endpoints.Serialize(writer);
			
			this.Points.ForEach(p => p.Serialize(writer));

			this.WaypointSummary.Serialize(writer);

		}  // End of Serialize

	}  // End of Route class

}
