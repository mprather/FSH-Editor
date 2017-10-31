/*
Copyright © 2017 Okean Voyaging LLC
Created by Maurice Prather

This software has been released under GPL v3.0 license. 

*/

using System;
using System.Collections.Generic;
using System.IO;

namespace FSH {

  public class Flob : SerializableData {

    public static long BoundarySize = 0x10000;

    private long bytesConsumed;

    private char[] header;
    private short f;
    private short g;

    public FlobType FlobType { get; set; }

    public long StartOffset { get; set; }

    public List<Block> Blocks { get; set; }

    public Flob() {

      this.Blocks = new List<Block>();

      this.header = "RAYFLOB1".ToCharArray();

      this.f = 1;
      this.g = 1;

    }

    public override void Deserialize(BinaryReader reader) {

      reader.BaseStream.Position = this.StartOffset;
      System.Diagnostics.Debug.WriteLine("Flob Position:" + reader.BaseStream.Position);

      this.header    = reader.ReadChars(8);
      this.f         = reader.ReadInt16();
      this.g         = reader.ReadInt16();
      this.FlobType  = (FlobType)reader.ReadUInt16();

      while (this.bytesConsumed < Flob.BoundarySize) {

        System.Diagnostics.Debug.WriteLine("Position: " + reader.BaseStream.Position + ", Consumed: " + this.bytesConsumed);

        Block block = new Block();
        block.Deserialize(reader);

        switch (block.Type) {
          case BlockType.Group:
            Group group = new Group() {
              Parent = block,
            };
            group.Deserialize(reader);
            block.Data = group;
            break;
          case BlockType.Last:
            this.bytesConsumed = Flob.BoundarySize;
            System.Diagnostics.Debug.WriteLine(" [Boundary Reached]");
            break;
          case BlockType.Route:
            Route route = new Route() {
              Parent = block,
            };
            route.Deserialize(reader);
            block.Data = route;
            break;
          case BlockType.Track:
            Track track = new Track();
            //track.Parent = block;
            track.Deserialize(reader);
            block.Data = track;
            break;
          case BlockType.TrackMetadata:
            TrackMetadata md = new TrackMetadata();
            //md.Parent        = block;
            md.Deserialize(reader);
            block.Data = md;
            break;
          case BlockType.StandaloneWaypoint:
            StandaloneWaypoint waypoint = new StandaloneWaypoint() {
              Parent = block,
            };
            waypoint.Deserialize(reader);
            block.Data = waypoint;
            break;
          default:
            throw new NotImplementedException();
        }

        this.Blocks.Add(block);

        if (this.bytesConsumed != Flob.BoundarySize) {

          if (block.DataLength % 2 != 0) {
            System.Diagnostics.Debug.WriteLine(" -- Skipping byte --");
            reader.ReadByte();
          }

          this.bytesConsumed = reader.BaseStream.Position - this.StartOffset;

        }

      }

    }  // End of Deserialize

    public override void Serialize(BinaryWriter writer) {

      writer.Write(this.header);
      writer.Write(this.f);
      writer.Write(this.g);
      writer.Write((ushort)this.FlobType);

      this.Blocks.ForEach(b => {
        b.Serialize(writer);
        if (b.Type == BlockType.Last) {
          while (writer.BaseStream.Position < (this.StartOffset + Flob.BoundarySize)) {
            writer.Write((byte)0xFF);
          }
        }
      });

    }  // End of Serialize

  }  // End of Flob class

}
