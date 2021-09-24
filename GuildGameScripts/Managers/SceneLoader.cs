using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Slider progressBar;
    public string menuScene;
    public string guildScene;
    public string recapScene;
    int lastScene;
    void Start(){
        lastScene = PlayerPrefs.GetInt("LastScene");
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene(){
        AsyncOperation asyncLoad;
        switch(lastScene)
        {
            case 0:
                asyncLoad = SceneManager.LoadSceneAsync(menuScene);
                break;
            case 1:
                asyncLoad = SceneManager.LoadSceneAsync(guildScene);
                break;
            case 2:
                asyncLoad = SceneManager.LoadSceneAsync(recapScene);
                break;
            default:
                asyncLoad = SceneManager.LoadSceneAsync(menuScene);
                break;
        }

        while(!asyncLoad.isDone){
            progressBar.value = asyncLoad.progress;
            yield return null;
        }
        
    }
}
