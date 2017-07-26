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

		private short route_a;
    private short rwh_a;

    private char rawNameLength;
		private char commentLength;

		private ushort b;

		private char[] rawName;
		private char[] rawComment;

    private string name                              = null;
    private string comment                           = null;

    private List<GenericPoint> genericPoints;
    private List<long> waypointIDs;
    
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
    
    public Block Parent { get; set; }

    public List<WaypointReference> ReferencedWaypoints { get; set; }

    public Route() {
			
		  this.waypointIDs          = new List<long>();
			this.genericPoints        = new List<GenericPoint>();
      this.ReferencedWaypoints  = new List<WaypointReference>();

		}  // End of ctor

    public override ushort CalculateSize() {

      int length = 2 + 1 + 1 + 2 + 2 +
                   this.rawNameLength +
                   this.commentLength +
                   this.waypointIDs.Count * 8 +
                   this.Endpoints.CalculateSize() +
                   this.genericPoints.Sum(p => p.CalculateSize()) +
                   2 + 2;

      this.ReferencedWaypoints.ForEach(rw => length += rw.CalculateSize());

      return (ushort)length;

    }  // End of CalculateSize

    public bool DeleteWaypointReference(long targetID) {

      bool itemDeleted = false;

      if (this.waypointIDs.Contains(targetID)) {

        this.genericPoints.RemoveAt(this.waypointIDs.IndexOf(targetID));
        this.waypointIDs.Remove(targetID);

        var q = this.ReferencedWaypoints.Find(m => m.ID == targetID);
        if (q != null) {
          this.ReferencedWaypoints.Remove(q);
        }

        itemDeleted = true;

      }

      return itemDeleted;

    }  // End of DeleteWaypointReference

    public override void Deserialize(BinaryReader reader) {

			this.route_a       = reader.ReadInt16();
			this.rawNameLength = reader.ReadChar();
			this.commentLength = reader.ReadChar();
			short idCount      = reader.ReadInt16();
			this.b             = reader.ReadUInt16();

			this.rawName       = reader.ReadChars(this.rawNameLength);
			this.rawComment    = reader.ReadChars(this.commentLength);

			for (int i = 0; i < idCount; i++) {
				this.waypointIDs.Add(reader.ReadInt64());
			}

			this.Endpoints = new RouteEndPointHeader();
			this.Endpoints.Deserialize(reader);

			for (int i = 0; i < idCount; i++) {
				GenericPoint point = new GenericPoint();
				point.Deserialize(reader);
				this.genericPoints.Add(point);
			}

      // ============================================================================
      // Section formerly within RouteWaypointHeader (aka Route Header 3)
      // ============================================================================
      short waypointCount = reader.ReadInt16();

      this.rwh_a = reader.ReadInt16();
      System.Diagnostics.Debug.Assert(rwh_a == 0, "Expected A value of 0 not found");

      for (int i = 0; i < waypointCount; i++) {
        WaypointReference wp = new WaypointReference(true);
        wp.Deserialize(reader);
        this.ReferencedWaypoints.Add(wp);
      }
      // ============================================================================

      System.Diagnostics.Debug.WriteLine("  (r) Name: " + this.Name.Replace('\0', '.') + ", Comment: " + this.Comment + ", a:" + this.route_a);

		}  // End of Deserialize

    public void Reverse() {

      this.ReferencedWaypoints.Reverse();
      this.waypointIDs.Reverse();
      this.genericPoints.Reverse();
      CalculateEndpoints();

    }  // End of Reverse

		public override void Serialize(BinaryWriter writer) {
			
		  writer.Write(this.route_a);
			writer.Write(this.rawNameLength);
			writer.Write(this.commentLength);
			writer.Write((short) this.waypointIDs.Count);
			writer.Write(this.b);

      writer.Write(this.rawName);
			writer.Write(this.rawComment);

			this.waypointIDs.ForEach(g => writer.Write(g));

			this.Endpoints.Serialize(writer);
			
			this.genericPoints.ForEach(p => p.Serialize(writer));

      // ============================================================================
      // Section formerly within RouteWaypointHeader (aka Route Header 3)
      // ============================================================================
      writer.Write((short)this.ReferencedWaypoints.Count);
      writer.Write(this.rwh_a);

      this.ReferencedWaypoints.ForEach(wp => wp.Serialize(writer));
      // ============================================================================
       
    }  // End of Serialize

    private void CalculateEndpoints() {

      this.Endpoints.StartLatitude   = (int)System.Math.Round(this.ReferencedWaypoints[0].Latitude * 10000000.0);
      this.Endpoints.StartLongitude  = (int)System.Math.Round(this.ReferencedWaypoints[0].Longitude * 10000000.0);

      this.Endpoints.EndLatitude     = (int)System.Math.Round(this.ReferencedWaypoints[this.ReferencedWaypoints.Count - 1].Latitude * 10000000.0);
      this.Endpoints.EndLongitude    = (int)System.Math.Round(this.ReferencedWaypoints[this.ReferencedWaypoints.Count - 1].Longitude * 10000000.0);

    }  // End of CalculateEndpoints

  }  // End of Route class

}
