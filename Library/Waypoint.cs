/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System.IO;

namespace FSH {
	
  public abstract class Waypoint : SerializableData {

		private int latitude;
		private int longitude;

	  public double Latitude {
			get {
				return this.latitude / 1e7;
			}
			set {
				this.latitude = (int) (value * 1e7);
			}
		}  // End of property Latitude

		public double Longitude {
			get {
				return this.longitude / 1e7;
			}
			set {
				this.longitude = (int)(value * 1e7);
			}
		}  // End of property Longitude

		public WaypointData Data { get; set; }
    
    public override ushort CalculateSize() {
      return (ushort) (4 + 4 + this.Data.CalculateSize());
    }  // End of CalculateSize

    public override void Deserialize(BinaryReader reader) {

			this.latitude  = reader.ReadInt32();
			this.longitude = reader.ReadInt32();

			this.Data = new WaypointData();
			this.Data.Deserialize(reader);

			System.Diagnostics.Debug.WriteLine("  (wp) Latitude: " + this.Latitude + ", Longitude:" + this.Longitude);

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {
			
		  writer.Write(this.latitude);
			writer.Write(this.longitude);

			this.Data.Serialize(writer);

    }  // End of Serialize

  }  // End of Waypoint class

}
