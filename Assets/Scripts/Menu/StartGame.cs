using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGamePressed() {        
        LevelManager.LoadScenes();
        StartCoroutine(ExitMainMenu());
    }

    IEnumerator ExitMainMenu() {
        yield return new WaitForSeconds(3f);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Menu"));
        yield return new WaitForSeconds(1f);
        LevelManager.setCurrentSceneActive();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Home2"));
        yield return null;
    }
}
