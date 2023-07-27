using Dungeon.Data;
using UnityEngine;

namespace Dungeon.Model {
    public class DungeonDataContainer : MonoBehaviour {
        public  DungeonData data;
        
        public DungeonDataContainer(DungeonData data) {
            this.data = data;
        }
    }
}