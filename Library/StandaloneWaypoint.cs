/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;

namespace FSH {

  /// <summary>
  /// Simple waypoints are not associated with any group. These are effectively stand-alone waypoints.
  /// </summary>
  public class StandaloneWaypoint : SerializableData {

	  public long ID { get; set; }

    public WaypointData Data { get; set; }

    public override ushort CalculateSize() {
      return (ushort)(8 + this.Data.CalculateSize());
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {
			
		  this.ID = reader.ReadInt64();
			
			this.Data = new WaypointData();
			this.Data.Deserialize(reader);

			System.Diagnostics.Debug.WriteLine("  (swp) ID: " + this.ID);

		}  // End of Deserialize

		public override void Serialize(BinaryWriter writer) {

			writer.Write(this.ID);
			this.Data.Serialize(writer);

    }  // End of Serialize

  }  // End of SimpleWaypoint class

}
