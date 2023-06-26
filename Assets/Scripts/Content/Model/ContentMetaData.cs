using System;

namespace Content {
    
    [Serializable]
    public struct ContentMetaData : ICloneable {
        public int ContentID;
        public int SpawnRate;
        public int MaxInstances;
        public int MaxInstancesGlobal;

        public object Clone() {
            return new ContentMetaData {
                ContentID = this.ContentID,
                SpawnRate = this.SpawnRate,
                MaxInstances = this.MaxInstances,
                MaxInstancesGlobal = this.MaxInstancesGlobal
            };
        }
    }

}