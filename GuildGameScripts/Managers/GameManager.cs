using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    public string guildScene;
    public string recapScene;
    public string loadScene;
    public GameObject fadeOutPannel;
    [Header("Guild")]
    public TMPro.TextMeshProUGUI guildNameText;
    public Text guildGoldText;
    public Text guildLevelText;
    public Text guildDayText;
    int currentGuildExp;
    int requieredExp;
    int guildLevel;
    int guildGold;
    int guildDay;
    string guildName;
    int guildHappiness;
    [Header("Adventurers")]
    public GameObject adventurerPreset; // This will be the default adventurer that will be used to create the others.
    Adventurer currentAdventurer; // This variable represents the current adventurer that's on screen.
    [Header("Adventurer Stats")]
    public GameObject statsUI;
    public Text levelText;
    public Text healthText;
    public Text defenseText;
    public Text attackText;
    public TMPro.TextMeshProUGUI classText;
    public Text evasionText;
    public Text manaText;
    public Text magicAttakText;
    [Header("Missions")]
    public Animator missionAnimator;
    public GameObject questPreset;
    public GameObject contents;
    public MissionProgressManager missionProgressManager;
    public TMPro.TextMeshProUGUI buttonBuyText;
    public Button buyQuestsButton;
    int missionCost; // Mission cost in gold.
    bool show; // Indicates if the mission UI should show or not.
    [Header("Data Manager")]
    public DataManager dataManager;
    Data currentData = new Data();
    [Header("Recap Scene")]
    public Text expensesText;
    public Text totalGuildGoldText;
    public Text remainingGoldText;
    public Text guildPositionText;
    public Slider experienceSlider;
    int expenses;
    int remainingGold;
    int guildPosition;
    

    Quest selectedQuest; // The quest the player is currently dragging.


    void Start()
    {
        if(SceneManager.GetActiveScene().name == guildScene) StartGame();
        else if(SceneManager.GetActiveScene().name == recapScene) StartRecap();
    }

    void Update()
    {
        if(Input.touchCount > 0) // Checks if there's a finger touching the screen.
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended) // Checks if the touch ended.
            {
                if(selectedQuest != null) // If there was a quest, it's selected.
                {
                    missionProgressManager.AddQuest(selectedQuest);
                    missionProgressManager.AddAdventurer(currentAdventurer);
                    Next();
                    Destroy(selectedQuest.gameObject);
                }
            }
        }
    }
    
    /// <summary>
    /// Saves the current game data.
    /// </summary>
    public void SaveData()
    {
        currentData.currentGuildExp = GetCurrentGuildExp();
        currentData.day = GetGuildDay();
        currentData.guildGold = GetGuildGold();
        currentData.guildLevel = GetGuildLevel();
        currentData.guildName = GetGuildName();
        currentData.happiness = GetGuildHappiness();
        currentData.guildPosition = GetGuildPosition();
    
        dataManager.SaveData(currentData);
    }

    /// <summary>
    /// Checks the save file, returns true if exists, false if not.
    /// </summary>
    bool CheckSaveFile()
    {
        if(dataManager.LoadData() != null) return true;
        else return false;
    }

    void StartRecap()
    {
        SetGuildStats(CheckSaveFile());
        SetRecap(dataManager.LoadData());
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    void StartGame()
    {
        show = false;
        statsUI.SetActive(false);

        SetGuildStats(CheckSaveFile());
        Next();
        SetQuests();

        SetMissionCost();
        CheckBuyButtons();
    }

    public void FadeOut()
    {
        fadeOutPannel.SetActive(true);
    }

    public void EndGame(int scene)
    {
        SaveData();
        PlayerPrefs.SetInt("LastScene", scene);
        SceneManager.LoadScene(loadScene);
    }

    void SetRecap(Data data)
    {
        SetExperienceSlider();
        SetExpenses();
        SetRemainingGold();
        SetGuildPosition(CalculateGuildPosition());
    }

    /// <summary>
    /// Sets the stats of the guild from the saved data. If there's no data it will set the default stats.
    /// </summary>
    void SetGuildStats(bool dataExists)
    {
        if(dataExists)
        {
            Data data = dataManager.LoadData();

            SetGuildLevel(data.guildLevel);
            SetGuildGold(data.guildGold);
            SetGuildDay(data.day);
            SetCurrentGuildExp(data.currentGuildExp);
            SetGuildName(data.guildName);
            SetGuildHappiness(data.happiness);
            SetGuildPosition(data.guildPosition);
        }
        else
        {
            SetGuildLevel(1);
            SetGuildGold(0);
            SetGuildDay(1);
            SetCurrentGuildExp(0);
            SetGuildHappiness(0);
            SetGuildName("guildName");
            SetGuildPosition(100);
        }
    }

    /// <summary>
    /// Sets the stats from the adventurer to the stats UI.
    /// </summary>
    public void SetUIStats(Adventurer adventurer)
    {
        levelText.text = "Lvl: " + adventurer.GetLevel();
        healthText.text = "HP: " + adventurer.GetHealth();
        defenseText.text = "Def: " + adventurer.GetDefense();
        attackText.text = "Atk: " + adventurer.GetAttack();
        evasionText.text = "Eva: " + adventurer.GetEvasion();
        manaText.text = "MP: " + adventurer.GetMana();
        magicAttakText.text = "M.Atk: " + adventurer.GetMagicAttack();
        classText.text = "Class: " + adventurer.GetClassType();
    }

    /// <summary>
    /// Shows or hides the UI stats panel.
    /// </summary>
    public void ShowUIStats(bool active)
    {
        statsUI.SetActive(active);
    }

    /// <summary>
    /// Sets and creates the number of quests, increases by 3 for each guild level.
    /// </summary>
    void SetQuests()
    {
        int questNumber = 5;
        for(int i = 0; i < questNumber; i++) Instantiate(questPreset, contents.transform);
    }

    public void BuyQuests()
    {
        if(guildGold >= missionCost)
        {
            SetGuildGold(GetGuildGold() - missionCost);
            SetQuests();
            CheckBuyButtons();
        }
    }

    /// <summary>
    /// Checks if the buy buttons can be interactuated.
    /// </summary>
    void CheckBuyButtons()
    {
        if(guildGold >= missionCost) buyQuestsButton.interactable = true;
        else buyQuestsButton.interactable = false;
    }

    public void ToggleMissionUI()
    {
        show = !show;
        missionAnimator.SetBool("Show", show);
    }

    /// <summary>
    /// Calls the next adventurer.
    ///</summary>
    public void Next()
    {
        if(currentAdventurer != null) Destroy(currentAdventurer.gameObject);
        Instantiate(adventurerPreset);
    }

    /// <summary>
    /// Continues to the next day.
    /// </summary>
    public void Continue()
    {
        SetGuildGold(remainingGold);
        AddDay();
        EndGame(1);
    }

    void AddLevel()
    {
        SetGuildLevel(GetGuildLevel() + 1);
        currentGuildExp -= requieredExp;
        SetMissionCost();
    }

    void AddDay()
    {
        SetGuildDay(GetGuildDay() + 1);
    }

    /// <summary>
    /// Calculates and returns the current position for the guild.
    /// </summary>
    int CalculateGuildPosition()
    {
        int happiness = GetGuildHappiness();

        if(happiness > 0) return GetGuildPosition() - Random.Range(1, happiness/2);
        else if(happiness < 0) return GetGuildPosition() + Random.Range(happiness/2, 0);
        else return 0;
    }

    //// GETTERS & SETTERS ////
    
    // Adventurers //

    public void SetCurrentAdventurer(Adventurer adventurer)
    {
        currentAdventurer = adventurer;
        SetUIStats(adventurer);
    }

    public Adventurer GetCurrentAdventurer()
    {
        return currentAdventurer;
    }

    // Quests //

    public Quest GetSelectedQuest()
    {
        return this.selectedQuest;
    }

    public void SetSelectedQuest(Quest selectedQuest)
    {
        this.selectedQuest = selectedQuest;
    }

    // Guilds //

    public int GetGuildLevel()
    {
        return guildLevel;
    }

    public void SetGuildLevel(int guildLevel)
    {
        Debug.Log("guild level set: " + guildLevel);
        this.guildLevel = guildLevel;
        guildLevelText.text = "Lvl. " + guildLevel;
        requieredExp = 100 + (87 * GetGuildLevel());
    }

    public string GetGuildName()
    {
        return guildName;
    }

    public void SetGuildName(string guildName)
    {
        this.guildName = guildName;
        guildNameText.text = guildName;
    }

    public int GetGuildGold()
    {
        return guildGold;
    }

    public void SetGuildGold(int guildGold)
    {
        this.guildGold = guildGold;
        if(guildGoldText != null)
        {
            guildGoldText.text = "Gold: " + guildGold + " G.";
            CheckBuyButtons();
        }
        else totalGuildGoldText.text = "Total guild gold: " + guildGold + " G.";
        
    }

    public int GetGuildDay()
    {
        return guildDay;
    }

    public void SetGuildDay(int day)
    {
        this.guildDay = day;
        guildDayText.text = "Day: " + guildDay;
    }

    public void SetCurrentGuildExp(int exp)
    {
        this.currentGuildExp = exp;
    }

    public int GetCurrentGuildExp()
    {
        return currentGuildExp;
    }

    public void SetGuildHappiness(int happiness)
    {
        this.guildHappiness += happiness;
        this.guildHappiness = Mathf.Clamp(guildHappiness, -10, 10);
    }

    public int GetGuildHappiness()
    {
        return guildHappiness;
    }

    public void SetMissionCost()
    {
        missionCost = (5 * GetGuildLevel()) - (2 * GetGuildLevel());
        if(buttonBuyText != null) buttonBuyText.text = "Buy quests (" + GetMissionCost() + "g.)";
    }

    public int GetMissionCost()
    {
        return missionCost;
    }

    public void SetExpenses()
    {
        expenses = Random.Range(2 * GetGuildLevel(), 5 * GetGuildLevel());
        expensesText.text = "Today's expenses: -" + GetExpenses() + " G." ;
    }

    public int GetExpenses()
    {
        return expenses;
    }

    public void SetRemainingGold()
    {
        remainingGold = GetGuildGold() - GetExpenses();
        remainingGoldText.text = "Remaining guild gold: " + remainingGold + " G.";
    }

    public void SetGuildPosition(int guildPosition)
    {
        if(guildPositionText != null)
        {
            guildPositionText.text = "#" + guildPosition;
        }
        this.guildPosition = guildPosition;
    }

    public int GetGuildPosition()
    {
        return guildPosition;
    }

    public void SetExperienceSlider()
    {
        Debug.Log("current exp: " + currentGuildExp);
        Debug.Log("requiered: " + requieredExp);
        if(currentGuildExp > requieredExp)
        {
            AddLevel();
        }
        experienceSlider.minValue = 0;
        experienceSlider.maxValue = requieredExp;

        experienceSlider.value = currentGuildExp;
    }
}
