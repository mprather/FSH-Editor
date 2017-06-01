/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.Collections.Generic;
using System.IO;

namespace FSH {

	public class RouteWaypointHeader : SerializableData {

		private short a;

		private short waypointCount;

		public List<RouteWaypoint> Waypoints { get; set; }

		public RouteWaypointHeader() {
			this.Waypoints = new List<RouteWaypoint>();
    }  // End of ctor

    public override ushort CalculateSize() {
      int length = 2 + 2;
      this.Waypoints.ForEach(rw => length += rw.CalculateSize());
      return (ushort)length;
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.waypointCount = reader.ReadInt16();
			this.a = reader.ReadInt16();

			for (int i = 0; i < this.waypointCount; i++) {
				RouteWaypoint wp = new RouteWaypoint();
				wp.Deserialize(reader);
				this.Waypoints.Add(wp);
			}

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {
			
		  writer.Write(this.waypointCount);
			writer.Write(this.a);

			this.Waypoints.ForEach(wp => wp.Serialize(writer));

    }  // End of Serialize

  }  // End of RouteWaypointHeader class

}
