using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Interaction;
using Manager.SubManager;
using UI;

namespace Manager {
    
    [Serializable]
    public static class LevelManager {

        [SerializeField]
        public static string currentSceneOpen = "";
        public static string lastSceneOpen = "Home2";
        
        [SerializeField]
        private static ElevatorMovementParameters elevatorMovementParameters;

        public static void LoadLevelBasedOnScene(string sceneName) {
            UIManager.Instance.HideNonPersistentUI();
            
            switch(sceneName) {
                case "Home1":
                    // currentSceneOpen = "Home1";
                    // LoadHome1Level();
                    break;
                case "Home2":
                    currentSceneOpen = "Home2";
                    LoadHome2Level();
                    break;
                case "Home3":
                    currentSceneOpen = "Home3";
                    LoadHome3Level();
                    break;
                case "Home4":
                    currentSceneOpen = "Home4";
                    LoadHome4Level();
                    break;
                case "Dungeon":
                    currentSceneOpen = "Dungeon";
                    LoadDungeonLevel();
                    break;
            }
        }

        private static void LoadHome1Level() {
            LoadingManager.StartLoading();
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home1", LoadSceneMode.Single);
            loadingState.completed += OnHomeLoaded;
        }
        
        private static void LoadHome2Level() {
            LoadingManager.StartLoading();
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home2", LoadSceneMode.Single);
            loadingState.completed += OnHomeLoaded;
        }
        
        private static void LoadHome3Level() {
            LoadingManager.StartLoading();
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home3", LoadSceneMode.Single);
            loadingState.completed += OnHomeLoaded;
        }
        
        private static void LoadHome4Level() {
            if (lastSceneOpen == "Dungeon") {
                elevatorMovementParameters = GameObject.Find("Elevator")
                    .GetComponent<ElevatorController>().moveParams;
            }
            
            LoadingManager.StartLoading();
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Home4", LoadSceneMode.Single);
            loadingState.completed += OnHomeLoaded;
        }

        private static void LoadDungeonLevel() {
            elevatorMovementParameters = GameObject.Find("Elevator")
                .GetComponent<ElevatorController>().moveParams;
            AsyncOperation loadingState = SceneManager.LoadSceneAsync("Dungeon", LoadSceneMode.Single);
            loadingState.completed += OnDungeonLoaded;
        }

        private static void OnHomeLoaded(AsyncOperation operation) {
            if (lastSceneOpen == "Dungeon") {
                elevatorMovementParameters.targetCoordinateY = 8.5f;
                
                GameObject.Find("Elevator").GetComponent<ElevatorController>()
                    .ContinueMoving(elevatorMovementParameters);
            }
            
            PositionPlayerInNewScene();
            lastSceneOpen = currentSceneOpen;
            LoadingManager.StopLoading();
        }
        
        private static void OnDungeonLoaded(AsyncOperation operation) {
            if (lastSceneOpen == "Dungeon") {
                elevatorMovementParameters.targetCoordinateY = 8.5f;
            }
            
            GameObject.Find("Elevator").GetComponent<ElevatorController>()
                .ContinueMoving(elevatorMovementParameters);
            
            DungeonManager.RenderDungeonsAll();
            
            lastSceneOpen = currentSceneOpen;

        }

        public static void SetCurrentSceneActive() {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentSceneOpen));
        }

        private static void PositionPlayerInNewScene() {
            SceneBound[] sceneBounds = GameObject.FindObjectsOfType<SceneBound>();
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

            foreach (SceneBound sceneBound in sceneBounds) {
                if (sceneBound.connectedSceneName == lastSceneOpen) {
                    GameObject spawnPoint = sceneBound.gameObject.transform.Find("SpawnPoint")?.gameObject;
                    Vector3 teleportToLocation = spawnPoint.transform.position;
                    player.transform.position = new Vector3(teleportToLocation.x, teleportToLocation.y+10, teleportToLocation.z);
                    return;
                }
            }
        }
        


        // public static void MoveObjectToAnotherScene(GameObject other, string currentScene) {
        //     SceneBound[] sceneBounds = GameObject.FindObjectsOfType<SceneBound>();
        //
        //     foreach (SceneBound sceneBound in sceneBounds) {
        //         if (sceneBound.connectedSceneName == currentScene) {
        //             GameObject spawnPoint = sceneBound.gameObject.transform.Find("SpawnPoint")?.gameObject;
        //             Vector3 teleportToLocation = spawnPoint.transform.position;
        //             other.transform.position = new Vector3(teleportToLocation.x, teleportToLocation.y+10, teleportToLocation.z);
        //             
        //             // Move the gameObject to the connected scene
        //             Scene destinationScene = SceneManager.GetSceneByName(currentScene);
        //             SceneManager.MoveGameObjectToScene(other, destinationScene);
        //         }
        //     }
        // }
        
        // This will load all scenes at once to allow async working for all at the same tine
        // private static void LoadHomeLevel() {
        //     // SceneManager.LoadScene("Home1", LoadSceneMode.Additive);
        //     SceneManager.LoadScene("Home2", LoadSceneMode.Additive);
        //     SceneManager.LoadScene("Home3", LoadSceneMode.Additive);
        //     SceneManager.LoadScene("Home4", LoadSceneMode.Additive);
        //     // SceneManager.LoadScene("Home5", LoadSceneMode.Additive);
        // }

    }
    
}
