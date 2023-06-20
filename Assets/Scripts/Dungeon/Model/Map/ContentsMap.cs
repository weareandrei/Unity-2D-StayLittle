using System.Collections.Generic;
using Content;

namespace Dungeon.Model {
    
    public class ContentsMap : DungeonMap {
        public List<ContentPoint> contentPointsAll;
        public List<ContentPoint> contentPointsUsed;
        private Dictionary<ContentPoint, ContentMetaData> contentPointsData = new Dictionary<ContentPoint, ContentMetaData>(); 
        
        public void CreateContentDictionary() {
            foreach (ContentPoint point in contentPointsUsed) {
                ContentMetaData metaData = point.metaData;
                // todo: later we will generate this metaData dynamically
                // foreach (ContentPoint contentPoint in _contentsMap.contentPointsUsed) {
                //     ContentType type = contentPoint.type;
                // }            
                contentPointsData.Add(point, metaData);
            }
        }
    }
    
}