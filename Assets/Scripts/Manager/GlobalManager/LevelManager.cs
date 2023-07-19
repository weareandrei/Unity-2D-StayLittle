using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Interaction;
using Manager.SubManager;

namespace Manager {
    
    [Serializable]
    public static class LevelManager {

        // this will be stores in some Json and retrieved when game starts
        [SerializeField]
        public static string currentSceneOpen = "Home4";
        // selectLevelBasedOnScene() will decide on the Level based on the [currentSceneOpen]
        [SerializeField]
        public static string currentLevelOpen = "";
        
        // ----- Dungeon Level -----
        [SerializeField]
        private static ElevatorMovementParameters elevatorMovementParameters;

        public static void LoadLevelBasedOnScene(string sceneName) {
            switch(sceneName) {
                case "Home1":
                    // currentLevelOpen = "Home1";
                    // LoadHome1Level();
                    break;
                case "Home2":
                    currentLevelOpen = "Home2";
                    LoadHome2Level();
                    break;
                case "Home3":
                    currentLevelOpen = "Home3";
                    LoadHome3Level();
                    break;
                case "Home4":
                    currentLevelOpen = "Home4";
                    LoadHome4Level();
                    break;
                case "Dungeon":
                    currentLevelOpen = "Dungeon";
                    LoadDungeonLevel();
                    break;
            }
        }

        private static void ShowLoadedScenes () {
            Scene[] loadedScenes = SceneManager.GetAllScenes();
            for (int i = 0; i < loadedScenes.Length; i++)
            {
                Debug.Log(loadedScenes[i].name);
            }

        }

        public static void MoveObjectToAnotherScene(GameObject other, string currentScene) {
            SceneBound[] sceneBounds = GameObject.FindObjectsOfType<SceneBound>();
        
            foreach (SceneBound sceneBound in sceneBounds) {
                if (sceneBound.connectedSceneName == currentScene) {
                    GameObject spawnPoint = sceneBound.gameObject.transform.Find("SpawnPoint")?.gameObject;
                    Vector3 teleportToLocation = spawnPoint.transform.position;
                    other.transform.position = new Vector3(teleportToLocation.x, teleportToLocation.y+10, teleportToLocation.z);
                    
                    // Move the gameObject to the connected scene
                    Scene destinationScene = SceneManager.GetSceneByName(currentScene);
                    SceneManager.MoveGameObjectToScene(other, destinationScene);
                }
            }
        }
        
        // This will load all scenes at once to allow async working for all at the same tine
        // private static void LoadHomeLevel() {
        //     // SceneManager.LoadScene("Home1", LoadSceneMode.Additive);
        //     SceneManager.LoadScene("Home2", LoadSceneMode.Additive);
        //     SceneManager.LoadScene("Home3", LoadSceneMode.Additive);
        //     SceneManager.LoadScene("Home4", LoadSceneMode.Additive);
        //     // SceneManager.LoadScene("Home5", LoadSceneMode.Additive);
        // }

        private static void LoadHome1Level() {
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home1", LoadSceneMode.Single);
        }
        
        private static void LoadHome2Level() {
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home2", LoadSceneMode.Single);
        }
        
        private static void LoadHome3Level() {
            
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home3", LoadSceneMode.Single);
        }
        
        private static void LoadHome4Level() {
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home4", LoadSceneMode.Single);
        }

        private static void LoadDungeonLevel() {
            elevatorMovementParameters = GameObject.Find("Elevator")
                .GetComponent<ElevatorController>().moveParams;
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Dungeon", LoadSceneMode.Single);
            loadingState.completed += OnDungeonLoaded;
        }
        
        private static void OnDungeonLoaded(AsyncOperation operation) {
            GameObject.Find("Elevator").GetComponent<ElevatorController>()
                .ContinueMoving(elevatorMovementParameters);
            
            DungeonManager.RenderDungeonsAll();
        }

        public static void SetCurrentSceneActive() {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentSceneOpen));
        }

    }
    
}
