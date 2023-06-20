using UnityEngine;

namespace Content {
    public class ContentPoint  {
        public ContentType type;
        public ContentMetaData metaData;
    }
    
    public enum ContentType {
        Mob,
        Collectible
    }
}