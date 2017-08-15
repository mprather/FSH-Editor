/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;

namespace FSH {

  public class WaypointReference : Waypoint {
	  
    private bool manageAllProperties;

    /// <summary>
    /// Unique ID. If the parent waypoint is a member of a Group, this value 
    /// matches the id of the parent. This provides a relationship between 
    /// Groups and WaypointReferences.
    /// However, if the parent waypoint is a standalone waypoint, this value
    /// appears to be unique and does not provide any relational assistance.
    /// </summary>
    public long ID { get; set; }

    public override ushort CalculateSize() {
      return (ushort)((this.manageAllProperties ? 8 : 0) + base.CalculateSize());
    }  // End of CalculateSize

    public WaypointReference(bool manageAllProperties) {
      this.manageAllProperties = manageAllProperties;
    }

    public override void Deserialize(BinaryReader reader) {

      if (this.manageAllProperties) {
        this.ID = reader.ReadInt64();
      }

      base.Deserialize(reader);
      
    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {

      if (this.manageAllProperties) {
        writer.Write(this.ID);
      }

			base.Serialize(writer);

    }  // End of Serialize

  }  // End of RouteWaypoint class

}
