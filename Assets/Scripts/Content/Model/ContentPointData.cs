using System;
using UnityEngine;

namespace Content {
    
    [Serializable]
    public class ContentPointData : ICloneable{
        public ContentType type;
        public ContentMetaData metaData;
        public Transform coordinates;
        
        public ContentPointData() {
            // Empty constructor
        }

        public object Clone() {
            // Shallow copy the object
            ContentPointData clonedData = (ContentPointData)this.MemberwiseClone();

            // Deep copy the reference types if needed
            clonedData.metaData = (ContentMetaData)this.metaData.Clone();
            // clonedData.coordinates = new Transform(this.coordinates.x, this.coordinates.y, this.coordinates.z);

            return clonedData;
        }

    }
    
    public enum ContentType {
        Mob,
        Collectible
    }
}