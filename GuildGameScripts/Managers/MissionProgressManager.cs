using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionProgressManager : MonoBehaviour
{
    List<Quest> quests;
    List<Adventurer> adventurers;
    public Animator panelProgress;
    public Animator missionProgress;

    public Text countMissionText;
    public Text goldAddText;
    public Text questSuccessText;
    public Animator questSuccessAnim;
    public Animator goldAnim;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        quests = new List<Quest>();
        adventurers = new List<Adventurer>();
        UpdateAnimator(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateProgress()
    {
        if(quests.Count > 0)
        {
            UpdateAnimator(true);

            if(quests.Count > 1)
            {
                countMissionText.enabled = true;
                countMissionText.text = (quests.Count - 1).ToString();
            }
            else
            {
                countMissionText.enabled = false;
            }
        }
        else UpdateAnimator(false);
    }

    void UpdateAnimator(bool hasMission)
    {
        panelProgress.SetBool("hasMission", hasMission);
        missionProgress.SetBool("hasMission", hasMission);
    }

    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
        UpdateProgress();
    }

    public void AddAdventurer(Adventurer adventurer)
    {
        adventurers.Add(adventurer);
    }

    public void FinishQuest()
    {
        bool success = CalculateSuccess(quests[0], adventurers[0]);

        if(success)
        {
            gameManager.SetGuildGold(gameManager.GetGuildGold() + quests[0].gold);
            gameManager.SetCurrentGuildExp(gameManager.GetCurrentGuildExp() + quests[0].experience);
            gameManager.SetGuildHappiness(1);
            goldAddText.text = "+" + quests[0].gold + "g";
            goldAnim.Play("GoldAdd");
            questSuccessText.text = "QUEST SUCCESSFUL";
        }
        else
        {
            gameManager.SetGuildHappiness(-2);
            questSuccessText.text = "QUEST FAILED";
        }
        questSuccessAnim.Play("QuestResult");
        quests.RemoveAt(0);
        adventurers.RemoveAt(0);
        UpdateProgress();
    }

    /// <summary>
    /// Calculates the success of a quest. Returns true or false.
    /// </summary>
    bool CalculateSuccess(Quest quest, Adventurer adventurer)
    {
        if(adventurer.GetLevel() >= quest.level) return true;  
        else return SuccessChance(quest.level, adventurer.GetLevel());
    }

    /// <summary>
    /// Calculates the chances of an adventurer passing a quest of higher level. Returns true or false.
    /// </summary>
    bool SuccessChance(int questLevel, int advLevel)
    {
        float baseReq = (float) advLevel / (float) questLevel * 100;
        int minSuccessNumber = Random.Range(1, 101);
        if(minSuccessNumber > baseReq) return false;
        else return true;
    }
}
