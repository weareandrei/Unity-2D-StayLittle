using System.Collections;
using Global;
using Manager;
using Manager.SubManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGamePressed() {
        GlobalVariables.environment = "DEV";
        DungeonManager.Initialize("5556444221");
        LevelManager.LoadLevelBasedOnScene("Home4");
        // StartCoroutine(ExitMainMenu());
    }

    // IEnumerator ExitMainMenu() {
    //     GlobalVariables.environment = "DEV";
    //     yield return new WaitForSeconds(0.1f);
    //     DungeonManager.Initialize("5556444221");
    //     SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Menu"));
    //     // yield return new WaitForSeconds(1f);
    //     // LevelManager.SetCurrentSceneActive();
    //     // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Home2"));
    //     yield return null;
    // }
}
