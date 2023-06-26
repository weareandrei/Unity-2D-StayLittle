using System.Collections.Generic;
using Content;
using Dungeon.Generator;
using HoneyGrid2D;

namespace Dungeon.Model {
    
    public class ContentsMap {
        public List<ContentPointData> contentPointsAll;
        public List<ContentPointData> contentPointsUsed;
        public FlexGrid2DSpecial<CloneableList<ContentPointData>> contentPointGrid;
    }
    
}