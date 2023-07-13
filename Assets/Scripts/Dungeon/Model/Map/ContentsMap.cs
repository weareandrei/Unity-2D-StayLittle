using System;
using System.Collections.Generic;
using Content;
using Dungeon.Generator;
using HoneyGrid2D;

namespace Dungeon.Model {
    
    public class ContentsMap {
        public FlexGrid2DSpecial<RoomContents> map;
    }

    public class RoomContents : ICloneable {
        public List<ContentPayload> payloads;
        public FlexGrid2DBool walls;

        public RoomContents () {
            payloads = new List<ContentPayload>();
            walls = new FlexGrid2DBool(Consts.Get<int>("RoomSize"), Consts.Get<int>("RoomSize"), true);
        }
        
        public object Clone() {
            RoomContents clone = new RoomContents();
        
            // Clone the list of payloads
            clone.payloads = new List<ContentPayload>(payloads.Count);
            foreach (ContentPayload payload in payloads)
            {
                clone.payloads.Add(payload.Clone() is ContentPayload ? (ContentPayload) payload.Clone() : default);
            }

            // Clone the walls
            clone.walls = walls.Clone() as FlexGrid2DBool;

            return clone;
        }

    }
    
}