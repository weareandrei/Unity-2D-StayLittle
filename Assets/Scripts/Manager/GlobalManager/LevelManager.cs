using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager {
    
    public static class LevelManager {

        // this will be stores in some Json and retrieved when game starts
        private static string _currentSceneOpen = "Home4";
        // selectLevelBasedOnScene() will decide on the Level based on the [currentSceneOpen] 
        private static string _currentLevelOpen = ""; 

        // static LevelManager() {
        //     selectLevelBasedOnScene(currentSceneOpen);
        //     Debug.Log("Level Manager initialized");
        // }

        public static void LoadScenes() {
            loadLevelBasedOnScene(_currentSceneOpen);
        }


        private static void loadLevelBasedOnScene(string thisScene) {
            switch(thisScene) {
                case "Home1":
                    _currentLevelOpen = "Home";
                    LoadHomeLevel();
                    break;
                case "Home2":
                    _currentLevelOpen = "Home";
                    LoadHomeLevel();
                    break;
                case "Home3":
                    _currentLevelOpen = "Home";
                    LoadHomeLevel();
                    break;
                case "Home4":
                    _currentLevelOpen = "Home";
                    LoadHomeLevel();
                    break;
                case "Home5":
                    _currentLevelOpen = "Home";
                    LoadHomeLevel();
                    break;

                case "Elevator":
                    _currentLevelOpen = "Elevator";
                    LoadElevatorLevel();
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

        public static void ChangeSceneTriggered(string sceneName) {
            // var sceneLoaded = SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            // sceneLoaded.completed = (x) => {
            //     SceneBound[] sceneBounds = FindObjectsOfType<SceneBound>();
                
            // };
            // SceneManager.UnloadSceneAsync(SceneManager.Scene.name);
        }
        
        // public static void MoveObjectToAnotherScene(GameObject other, string goToScene) {
        //     // It's already open so we don't need to load it. Just teleport to appropriate location.
        //     SceneBound[] sceneBounds = GameObject.FindObjectsOfType<SceneBound>();
        //
        //     foreach (SceneBound sceneBound in sceneBounds) {
        //         // Check if this SceneBound GameObject is located on the needed scene,
        //         //  because it can be on any other loaded scene.
        //         if (sceneBound.gameObject.scene.name == goToScene) {
        //             if (sceneBound.connectedSceneName == _currentSceneOpen) {
        //                 Vector3 teleportToLocation = sceneBound.gameObject.transform.position;
        //                 other.transform.position = new Vector3(teleportToLocation.x, teleportToLocation.y+10, teleportToLocation.z);
        //                 _currentSceneOpen = goToScene;
        //             }
        //         }   
        //     }
        // }

        // This will load all scenes at once to allow async loading for all at the same tine
        private static void LoadHomeLevel() {
            // SceneManager.LoadScene("Home1", LoadSceneMode.Additive);
            SceneManager.LoadScene("Home2", LoadSceneMode.Additive);
            SceneManager.LoadScene("Home3", LoadSceneMode.Additive);
            SceneManager.LoadScene("Home4", LoadSceneMode.Additive);
            // SceneManager.LoadScene("Home5", LoadSceneMode.Additive);
        }

        private static void LoadElevatorLevel() {
            // SceneManager.LoadScene("Home1", LoadSceneMode.Additive);
            SceneManager.LoadScene("Elevator");
            // SceneManager.LoadScene("Home5", LoadSceneMode.Additive);
        }

        public static void setCurrentSceneActive() {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentSceneOpen));
        }

    }
    
}
