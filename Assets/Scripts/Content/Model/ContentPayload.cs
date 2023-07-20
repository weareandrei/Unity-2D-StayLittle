using System;

namespace Content {
    
    [Serializable]
    public struct ContentPayload : ICloneable {
        public ContentType type;
        public string ContentID;
        public int SpawnRate;
        public int MaxInstances;
        public int MaxInstancesGlobal;

        public ContentPayload(ContentType type) {
            this.type = type;
            ContentID = "";
            SpawnRate = 0;
            MaxInstances = 0;
            MaxInstancesGlobal = 0;
        }

        public object Clone() {
            return new ContentPayload {
                type = this.type,
                ContentID = this.ContentID,
                SpawnRate = this.SpawnRate,
                MaxInstances = this.MaxInstances,
                MaxInstancesGlobal = this.MaxInstancesGlobal
            };
        }
    }

}