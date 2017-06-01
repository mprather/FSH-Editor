/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;

namespace FSH {

  public class RouteWaypoint : Waypoint {
	  
	  public long ID { get; set; }

    public override ushort CalculateSize() {
      return (ushort)(8 + base.CalculateSize());
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {
			
		  this.ID = reader.ReadInt64();
			base.Deserialize(reader);

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {

			writer.Write(this.ID);
			base.Serialize(writer);

    }  // End of Serialize

  }  // End of RouteWaypoint class

}
