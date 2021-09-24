using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Image progressBar;
    void Start(){
        StartCoroutine(LoadAsyncScene());
    }
    // Loads scene while visually representing the loading progress with the fill amount of an image.
    IEnumerator LoadAsyncScene(){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");

        while(!asyncLoad.isDone){
            progressBar.fillAmount = asyncLoad.progress;
            yield return null;
        }
    }
}
