using UnityEngine;

namespace Content {
    public class ContentPoint : MonoBehaviour  {
        public ContentType type;
        public ContentMetaData metaData;
    }
    
    public enum ContentType {
        Mob,
        Collectible
    }
}