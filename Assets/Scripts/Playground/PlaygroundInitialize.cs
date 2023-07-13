using System.Collections;
using System.Collections.Generic;
using Dungeon;
using Global;
using Manager.SubManager;
using UnityEngine;

namespace Playground {
    public class PlaygroundInitialize : MonoBehaviour
    {
        void Awake() {
            GlobalVariables.environment = "DEV";
            Consts.Set("MaxDungeons", 6);
            Consts.Set("DungeonChunkCount", 5);
            DungeonManager.Initialize("5556444221");
        }
    }
}