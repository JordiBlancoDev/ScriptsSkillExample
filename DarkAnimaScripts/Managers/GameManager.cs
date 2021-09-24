using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    const float fpsMeasurePeriod = 0.1f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;

    PhotoreceptionSystem photoreceptionSystem;
    [Header("Debug")]
    public Text lightText;
    public bool debug;
    public Text fpsText;
    [Header("Main Menu")]
    public GameObject pauseUI;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public Slider gammaSlider;
    public Slider audioSlider;
    public GameObject quitPopup;
    public GameObject resumeButton;
    public GameObject mediumButton;
    public GameObject stayButton;
    [Header("Helper Panel")]
    public Text helperText;
    public GameObject helperPanel;
    [Header("Interact Panel")]
    public GameObject interactPanel;
    public Text interactText;
    [Header("Lore Panel")]
    public GameObject lorePanel;
    public Text loreText;
    public GameObject closeButton;
    [Header("End Panel")]
    public GameObject endPanel;
    public Text leaveText;
    public Text stayText;
    [Header("Death Screen")]
    public GameObject deathPanel;

    bool gamepad;
    bool keyboard;
    bool paused;
    bool reading;
    string intreactKey;
    string leaveKey;
    string concentrateKey;
    string pulseKey;
    string tpKey;
    string grabKey;
    string crouchKey;
    string runKey;

    PlayerManager playerManager;
    InputManager inputManager;
    CameraManager cameraManager;
    Volume volume;
    ChromaticAberration chromaticAberration;
    LiftGammaGain gamma;
    Vignette vignette;
    
    void Awake(){

        photoreceptionSystem = FindObjectOfType<PhotoreceptionSystem>();
        playerManager = FindObjectOfType<PlayerManager>();
        inputManager = FindObjectOfType<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        volume = FindObjectOfType<Volume>();

    }

    void Start()
    {
        Time.timeScale = 1f;
        // Sets the gamma volume.
        if(volume != null){
            volume.profile.TryGet(out gamma);
        }
        // Resets the check points when going to the start menu.
        if(SceneManager.GetActiveScene().buildIndex == 0 && PlayerPrefs.HasKey("playerPosX")){
            DeleteCheckPoints();
        }

        if(SceneManager.GetActiveScene().buildIndex == 2){
            // Checks if the check position positions are saved.
            if(PlayerPrefs.HasKey("playerPosX")){
                // Teleports the player to the checkpoint coordinates.
                playerManager.transform.position = new Vector3(PlayerPrefs.GetFloat("playerPosX"), PlayerPrefs.GetFloat("playerPosY"), PlayerPrefs.GetFloat("playerPosZ"));
                cameraManager.gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("playerPosX"), PlayerPrefs.GetFloat("playerPosY"), PlayerPrefs.GetFloat("playerPosZ"));
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out chromaticAberration);
            SetSettings();
        }else{
            if(PlayerPrefs.HasKey("Volume")){
                SetSettings();
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    void Update(){

        if(debug){
            lightText.text = "Light Value: " + photoreceptionSystem.lightValue;
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod){
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                fpsText.text = "FPS: " + m_CurrentFps;
            }
        }

        if(gamma != null){
            gamma.gamma.value = new Vector4(1f, 1f, 1f, gammaSlider.value);
        }

        if(audioSlider != null){
            AudioListener.volume = audioSlider.value;
        }        
        
        if(SceneManager.GetActiveScene().buildIndex == 2){
            vignette.intensity.value = (-playerManager.health / 100f) + 1f;
            chromaticAberration.intensity.value = (-playerManager.health / 100f) + 1f;

            if(playerManager.health <= 0f){
                DeathScreen();
            }
            // Switches the interaction key depending of current input device.
            if(Gamepad.current != null && Gamepad.current.IsActuated() && !gamepad){
                intreactKey =  "[B]";
                leaveKey = "[A]";
                concentrateKey = "[RT]";
                pulseKey = "[A]";
                tpKey = "[Y]";
                grabKey = "[X]";
                crouchKey = "[LT]";
                runKey = "[Left Stick]";
                keyboard = false;
                gamepad = true;
            }else if(Keyboard.current.IsActuated() && !keyboard){
                intreactKey =  "[F]";
                leaveKey = "[E]";
                concentrateKey = "[Space]";
                pulseKey = "[E]";
                tpKey = "[Right Click]";
                grabKey = "[Left Click]";
                crouchKey = "[Left Ctrl]";
                runKey= "[Left Shift]";
                gamepad = false;
                keyboard = true;
            }
        }

    }

    void SetSettings(){
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        if(audioSlider != null && gammaSlider != null){
            audioSlider.value = PlayerPrefs.GetFloat("Volume");
            gammaSlider.value = PlayerPrefs.GetFloat("Gamma");
            gamma.gamma.value = new Vector4(1f, 1f, 1f, PlayerPrefs.GetFloat("Gamma"));
        }
    }

    public void setHelperText(string text){
        if(text.Contains("concentrateKey")){
            text = text.Replace("concentrateKey", concentrateKey);
        }
        if(text.Contains("pulseKey")){
            text = text.Replace("pulseKey", pulseKey);
        }
        if(text.Contains("grabKey")){
            text = text.Replace("grabKey", grabKey);
        }
        if(text.Contains("tpKey")){
            text = text.Replace("tpKey", tpKey);
        }
        if(text.Contains("crouchKey")){
            text = text.Replace("crouchKey", crouchKey);
        }
        if(text.Contains("runKey")){
            text = text.Replace("runKey", runKey);
        }

        SetText(helperPanel, helperText, text);

    }
    /// <summary>
    /// Interacts with the game object.
    /// </summary>
    /// <param name="obj"> Object to interact with.
    public void Interact(GameObject obj){
        
        if(obj.GetComponent<Key>()){
            
            SetInteracText(intreactKey + " Pick Up Key");
            if(inputManager.isPicking == 1f){
                obj.GetComponent<Key>().PickUp();
            }

        }else if(obj.GetComponent<LockedDoor>()){

            SetInteracText(intreactKey + " Unlock Door");
            if(inputManager.isPicking == 1f){
                obj.GetComponent<LockedDoor>().OpenDoor();
            }

        }else if(obj.GetComponent<LoreObject>()){

            SetInteracText(intreactKey + " Read");
            if(inputManager.isPicking == 1f){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                cameraManager.locked = true;
                SetLoreText(obj.GetComponent<LoreObject>().GetLore());
                Time.timeScale = 0f;
            }

        }

    }

    void DeathScreen(){
        Time.timeScale = 0f;
        playerManager.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathPanel.SetActive(true);
    }

    void SetInteracText(string text){
        SetText(interactPanel, interactText, text);
    }

    void SetLoreText(string text){
        reading = true;
        SetText(lorePanel, loreText, text);
        EventSystem.current.SetSelectedGameObject(closeButton);
    }

    void SetText(GameObject panel, Text text, string inputText){
        panel.SetActive(false);
        panel.SetActive(true);
        text.text = inputText;
    }

    public void HideLoreText(){
        Time.timeScale = 1f;
        reading = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;
        cameraManager.locked = false;
        lorePanel.SetActive(false);
    }

    public void HideInteractText(){
        interactPanel.SetActive(false);
    }

    public void EndGame(){
        stayText.text = intreactKey + " Stay";
        leaveText.text = leaveKey + " Leave";
        endPanel.SetActive(true);
        if(inputManager.isWaving == 1f || inputManager.isPicking == 1f) SceneManager.LoadScene(3);
    }

    public void HideEndGamePanel(){
        endPanel.SetActive(false);
    }

    public void DeterminatePause(){
        if(SceneManager.GetActiveScene().buildIndex == 2){
            if(!reading){
                if(paused){
                    ResumeGame();
                }else{
                    PauseGame();
                }
            }else{
                HideLoreText();
            }
        }
    }

    void PauseGame(){
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cameraManager.locked = true;
        paused = true;
    }

    void ResumeGame(){
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitPopup.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraManager.locked = false;
        paused = false;
    }

    public void SetCheckPoint(){

        if(PlayerPrefs.HasKey("playerPosX")){
            DeleteCheckPoints();
        }

        PlayerPrefs.SetFloat("playerPosX", playerManager.gameObject.transform.position.x);
        PlayerPrefs.SetFloat("playerPosY", playerManager.gameObject.transform.position.y);
        PlayerPrefs.SetFloat("playerPosZ", playerManager.gameObject.transform.position.z);
        PlayerPrefs.Save();
    }

    void DeleteCheckPoints(){
        PlayerPrefs.DeleteKey("playerPosX");
        PlayerPrefs.DeleteKey("playerPosY");
        PlayerPrefs.DeleteKey("playerPosZ");
    }

    public void StartGame(){
        SaveSettings();
        SceneManager.LoadScene(1);
    }

    public void StartScreen(){
        SceneManager.LoadScene(0);
    }

    public void OptionsScreen(){
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mediumButton);
        settingsMenu.SetActive(true);
    }

    public void MainScreen(){
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        settingsMenu.SetActive(false);
    }

    public void MediumQuality(){
        QualitySettings.SetQualityLevel(1);
    }

    public void LowQuality(){
        QualitySettings.SetQualityLevel(0);
    }

    public void HighQuality(){
        QualitySettings.SetQualityLevel(2);
    }

    public void ShowQuitPopUp(){
        if(quitPopup.activeSelf){
            mainMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(resumeButton);
            quitPopup.SetActive(false);
        }else{
            mainMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(stayButton);
            quitPopup.SetActive(true);
        } 
    }

    public void QuitGame(){
        Application.Quit();
    }

    void SaveSettings(){
        
        PlayerPrefs.SetFloat("Gamma", gammaSlider.value);
        PlayerPrefs.SetFloat("Volume", audioSlider.value);
        PlayerPrefs.Save();

    }

}
