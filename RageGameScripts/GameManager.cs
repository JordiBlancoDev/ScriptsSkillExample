using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
// Script that manages the game.
public class GameManager : MonoBehaviour
{
    [HideInInspector]public int jumps;
    [HideInInspector]public int bumps;
    float time;
    [HideInInspector]public int minutes;
    [HideInInspector]public int hours;
    [HideInInspector]public float distanceClimbed;
    bool paused;
    bool canPause;
    bool easyMode;
    [HideInInspector]public bool gameStart;
    PlayerController player;
    VideoPlayer intro;
    CameraManager cameraManager;
    AchievementManager achievementManager;
    [Header("UI Settings")]
    public GameObject introUI;
    public GameObject pauseUI;
    public GameObject warningUI;
    public GameObject winUI;
    public GameObject progressUI;
    public GameObject hintUI;
    public GameObject achievementUI;
    [Header("Win UI Settings")]
    public Text jumpsText;
    public Text bumpsText;
    public Text timeText;
    public Text distanceClimbedText;
    [Header("Audio")]
    public AudioSource hoverSound;
    public AudioSource clickSound;
    public Slider audioSlider;
    public AudioSource bgmMusic;
    public Slider bgmSlider;
    [Header("Progress Settings")]
    public Slider progressSlider;
    public Text progressText;
    public GameObject finishLine;

    void Awake(){
        if(FindObjectOfType<PlayerController>()) player = FindObjectOfType<PlayerController>();
        if(FindObjectOfType<CameraManager>()) cameraManager = FindObjectOfType<CameraManager>();
        if(FindObjectOfType<VideoPlayer>()){
            intro = FindObjectOfType<VideoPlayer>();
            intro.url = System.IO.Path.Combine(Application.streamingAssetsPath,"AGameBy.mp4");
        }
        if(FindObjectOfType<AchievementManager>()) achievementManager = FindObjectOfType<AchievementManager>();
    }

    void Start(){
        if(SceneManager.GetActiveScene().buildIndex != 0){
            // Stats preparation.
            jumps = 0;
            bumps = 0;
            time = 0f;
            minutes = 0;
            hours = 0;
            distanceClimbed = 0f;
            // Game state.
            paused = true;
            canPause = true;
            player.enabled = false;
            cameraManager.enabled = false;
            easyMode = false;
            gameStart = false;
            // UI preparation.            
            introUI.SetActive(true);
            pauseUI.SetActive(false);
            warningUI.SetActive(false);
            winUI.SetActive(false);
            progressUI.SetActive(false);
            hintUI.SetActive(false);
            achievementUI.SetActive(false);
        }
    }

    
    void Update(){
        if(SceneManager.GetActiveScene().buildIndex != 0){
            time += Time.deltaTime;
            // Pause checks.
            if(Input.GetKeyDown(KeyCode.P) && canPause){
                if(paused) ResumeGame();
                else PauseGame();
            }
            // Time conversion from seconds to minutes for the end stats.
            if(time >= 60){
                minutes += 1;
                time = 0f;
            }
            // Time conversion from minutes to hours for the end stats.
            if(minutes == 60){
                hours = 1;
                minutes = 0;
            }
            // Settings adjustements.
            AudioListener.volume = audioSlider.value;
            bgmMusic.volume = bgmSlider.value;

            UpdateProgress();
        }else{
            // Intro video play.
            if(Time.timeSinceLevelLoad >= intro.length + 2) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    /// <summary>
    /// Updates the in-game progress bar.
    /// </summary>
    void UpdateProgress(){
        float currentProg = player.gameObject.transform.position.y / finishLine.transform.position.y;
        currentProg = Mathf.Clamp(currentProg, 0, 1);
        progressSlider.value = currentProg;
        progressText.text = ((int)(currentProg * 100)) + "%";
        achievementManager.GetAchievement(Achievement.progress, (int)(currentProg * 100));
    }
    /// <summary>
    /// Opens the tiwtter URL.
    /// </summary>
    public void OpenTwitter(){
        Application.OpenURL("https://twitter.com/JordiDevWorld");
    }
    /// <summary>
    /// Opens the Itch URL.
    /// </summary>
    public void OpenItch(){
        Application.OpenURL("https://jordidevworld.itch.io");
    }
    /// <summary>
    /// Opens the WebPage URL.
    /// </summary>
    public void OpenWebPage(){
        Application.OpenURL("https://jordiblancoibanez.wixsite.com/home");
    }

    public void ShowAchievements(){
        achievementUI.SetActive(true);
    }

    public void HideAchievements(){
        achievementUI.SetActive(false);
    }

    public void ToggleEasyMode(){
        easyMode = !easyMode;
        player.ToggleEasyMode();
    }

    public void ShowHint(){
        hintUI.SetActive(true);
    }

    public void HideHint(){
        hintUI.SetActive(false);
    }

    public void onHover(){
        hoverSound.Play();
    }

    public void onClick(){
        clickSound.Play();
    }

    public void ResumeGame(){
        Time.timeScale = 1f;
        gameStart = true;
        player.enabled = true;
        cameraManager.enabled = true;
        progressUI.SetActive(true);
        introUI.SetActive(false);
        pauseUI.SetActive(false);
        warningUI.SetActive(false);
        achievementUI.SetActive(false);
        hintUI.SetActive(false);
        paused = false;
    }

    public void PauseGame(){
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        warningUI.SetActive(false);
        player.enabled = false;
        paused = true;
    }

    public void WarningPopUp(){
        warningUI.SetActive(true);
    }

    public void Quit(){
        Application.Quit();
    }

    public void AddJump(){
        jumps++;
        achievementManager.GetAchievement(Achievement.jump, jumps);
    }

    public void AddBump(){
        bumps++;
        achievementManager.GetAchievement(Achievement.bump, bumps);
    }

    public void AddDistanceClimbed(float distance){
        distanceClimbed += distance;
        achievementManager.GetAchievement(Achievement.climb, (int)distanceClimbed);
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void EndGame(){
        player.gameObject.SetActive(false);
        canPause = false;
        winUI.SetActive(true);
        jumpsText.text = "Times jumped: " + jumps + " jumps.";
        bumpsText.text = "Times bumped into things: " +  bumps + " bumps.";
        if(minutes < 10){
            timeText.text = "Time it took you to escape:  " + hours + " : 0" + minutes + " : " + ((int)time) + " s.";
        }else{
            timeText.text = "Time it took you to escape:  " + hours + " : " + minutes + " : " + ((int)time) + " s.";
        }
        distanceClimbedText.text = "Distance you had to climb: " + ((int)distanceClimbed) + " meters.";
    }
}
