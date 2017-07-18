/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FSH {

	public class RouteWaypointHeader : SerializableData {

		private short a;

		public List<WaypointReference> ReferencedWaypoints { get; set; }

		public RouteWaypointHeader() {
			this.ReferencedWaypoints = new List<WaypointReference>();
    }  // End of ctor

    public override ushort CalculateSize() {
      int length = 2 + 2;
      this.ReferencedWaypoints.ForEach(rw => length += rw.CalculateSize());
      return (ushort)length;
    }  // End of CalculateSize

    public bool Delete(long id) {

      bool itemDeleted = false;

      var q = this.ReferencedWaypoints.Find(m => m.ID == id);
      if (q != null) {
        this.ReferencedWaypoints.Remove(q);
        itemDeleted = true;
      }

      return itemDeleted;

    }  // End of Delete

    public override void Deserialize(BinaryReader reader) {

      short waypointCount = reader.ReadInt16();
			
      this.a = reader.ReadInt16();
      System.Diagnostics.Debug.Assert(a == 0, "Expected A value of 0 not found");

			for (int i = 0; i < waypointCount; i++) {
				WaypointReference wp = new WaypointReference(true);
				wp.Deserialize(reader);
				this.ReferencedWaypoints.Add(wp);
			}

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {

      writer.Write((short)this.ReferencedWaypoints.Count);
			writer.Write(this.a);

			this.ReferencedWaypoints.ForEach(wp => wp.Serialize(writer));

    }  // End of Serialize

  }  // End of RouteWaypointHeader class

}
