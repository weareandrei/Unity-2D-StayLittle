using System;
using System.Collections;
using System.Collections.Generic;
using Dungeon;
using Dungeon.Data;
using Dungeon.Generator;
using Dungeon.Renderer;
using Dungeon.Gameplay;
using UnityEngine;
using Random = Util.Random;

namespace Manager.SubManager {
    public static class DungeonManager {
        
        private static ScoreCounter scoreCounter;
        private static string dungeonInProgress;

        public static string currentDungeon; // Dungeon ID's ike "a", "b", etc ...
        
        public static void Initialize(string seed = "") {
            GenerateDungeonList(seed);
        }
        
        private static void GenerateDungeonList(string seed = "") {
            if (string.IsNullOrEmpty(seed)) {
                seed = Random.GenerateSeed();
            }
            // use the seed to generate the dungeon list
            DungeonList.dungeons = DungeonListGenerator.Generate(Consts.Get<int>("MaxDungeons"), seed);
            DungeonList.availableDungeons = DungeonList.dungeons;
        }

        public static void RenderDungeonsAll() {
            foreach (DungeonData dungeonData in DungeonList.dungeons) {
                DungeonRenderer renderer = new DungeonRenderer();
                renderer.RenderDungeon(
                    dungeonData, 
                    DungeonGenerator.GenerateDungeonBySeed(dungeonData.seed, dungeonData.coordinates)
                );
            }
        }
        
        public static string ChooseRandomDungeon() {
            foreach (DungeonData dungeonData in DungeonList.dungeons) {
                if (dungeonData.typeOfDungeon == DungeonType.regularDungeon) {
                    currentDungeon = dungeonData.id;
                    return dungeonData.id;
                }
            }

            throw new Exception();
        }

        public static bool PlayerCompletedDungeon() {
            // Check if the Player has had any impact on the Dungeon. If yes, then we can mark this Dungeon as completed.
            return true;
        }

        public static ScoreCounter ShowFinalScore() {
            // DungeonManagerHelper ... ?
            return FindScoreCounterFoDungeon(currentDungeon);
        }

        public static ScoreCounter FindScoreCounterFoDungeon(string id) {
            return new ScoreCounter();
        }

        public static void DungeonSelected(string id) {

            if (dungeonInProgress != null) {
                
            }
            
            dungeonInProgress = id;
            scoreCounter = new ScoreCounter();
        }
        
        
        // private class DungeonManagerHelper : MonoBehaviour {
        //     
        //     public List<GameObject> renderedRooms = new List<GameObject>();
        //     private DungeonRenderer dungeonRenderer; // Reference to DungeonRenderer instance
        //
        //     public void SetDungeonRenderer(DungeonRenderer renderer) {
        //         dungeonRenderer = renderer;
        //     }
        //
        //     public void StartRenderCoroutine(GameObject dungeonParent) {
        //         StartCoroutine(RenderCoroutine(dungeonParent));
        //     }
        //
        //     private IEnumerator RenderCoroutine(GameObject dungeonParent) {
        //         dungeonRenderer.RenderRooms(dungeonParent);
        //         yield return new WaitForEndOfFrame();
        //         dungeonRenderer.RenderContents(dungeonParent);
        //         yield return new WaitForEndOfFrame();
        //     }
        // }
        
    }
    
}