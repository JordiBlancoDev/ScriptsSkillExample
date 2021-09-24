using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Script to handle the achievements.
public class AchievementManager : MonoBehaviour
{
    public Text titleUI;
    public Text descriptionUI;
    public Animator achievementAnimation;
    public AudioSource achievementSound;
    GameManager gameManager;
    List<Achievement> achievements = new List<Achievement>();
    public GameObject achievementUI;
    public GameObject achievementTemplate;

    void Awake(){
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start(){
        achievementAnimation.SetBool("Achievement", false);
        SetAchievemnts();
    }

    void SetAchievemnts(){
        //Achievement list      |           TITLE           DESCRIPTION             ACHIEVED         TYPE        AMOUNT
        achievements.Add(new Achievement("Baby steps", "Jump for the first time.", false, Achievement.jump, 1));
        achievements.Add(new Achievement("Just stretching", "Jump 50 times.", false, Achievement.jump, 50));
        achievements.Add(new Achievement("Getting used to it", "Jump 100 times.", false, Achievement.jump, 100));
        achievements.Add(new Achievement("Legs hurting", "Jump 200 times.", false, Achievement.jump, 200));
        achievements.Add(new Achievement("Do I even have legs?", "Jump 500 times.", false, Achievement.jump, 500));
        achievements.Add(new Achievement("Jumping maniac", "Jump 1000 times.", false, Achievement.jump, 1000));

        achievements.Add(new Achievement("Getting started", "Jump a total of 50 meters.", false, Achievement.climb, 50));
        achievements.Add(new Achievement("A little climb", "Jump a total of 100 meters.", false, Achievement.climb, 100));
        achievements.Add(new Achievement("Little by little", "Jump a total of 200 meters.", false, Achievement.climb, 200));
        achievements.Add(new Achievement("Sky's the limit", "Jump a total of 300 meters.", false, Achievement.climb, 300));
        achievements.Add(new Achievement("CUBIE TO THE MOON!!", "Jump a total of 500 meters.", false, Achievement.climb, 500));
        achievements.Add(new Achievement("Even further beyond", "Jump a total of 1000 meters.", false, Achievement.climb, 1000));

        achievements.Add(new Achievement("Not a big deal", "Fall 20 meters.", false, Achievement.fall, 20));
        achievements.Add(new Achievement("It's okay", "Fall 50 meters.", false, Achievement.fall, 50));
        achievements.Add(new Achievement("Why does gravity exists", "Fall 100 meters.", false, Achievement.fall, 100));
        achievements.Add(new Achievement("Why am I here", "Fall 200 meters.", false, Achievement.fall, 200));
        achievements.Add(new Achievement("Just pure suffering", "Fall 300 meters.", false, Achievement.fall, 300));

        achievements.Add(new Achievement("Ouch", "Bump 10 times into something.", false, Achievement.bump, 10));
        achievements.Add(new Achievement("I can't feel anything", "Bump 50 times into something.", false, Achievement.bump, 50));

        achievements.Add(new Achievement("Playtime is over", "Get pass the tutorial.", false, Achievement.progress, 31));
        achievements.Add(new Achievement("Half way there...?", "Reach half the level.", false, Achievement.progress, 50));
        achievements.Add(new Achievement("Almost there", "Get to the third part of the level.", false, Achievement.progress, 75));
        achievements.Add(new Achievement("Was it worth?", "Escape the tower.", false, Achievement.progress, 99));
    }

    /// <summary>
    /// Checks if the requierements for any achievement have been met, if so, gives the achievements.
    /// </summary>
    /// <param name="type"> Achievement type, use "Achievement.[type]".
    /// <param name="amount"> Amount to check with the achievement.
    public void GetAchievement(int type, int amount){
        if(gameManager.gameStart == true){
            foreach(Achievement a in achievements){
                if(a.GetType() == type){
                    if(amount >= a.GetAmount() && a.GetAchieved() == false){
                        achievementAnimation.SetBool("Achievement", true);
                        AchievementNotice(a);
                    } 
                }
            }
            achievementAnimation.SetBool("Achievement", false);
        }
    }
    /// <summary>
    /// Makes an ingame notice of the achieved achievement.
    /// </summary>
    /// <param name="achievement"> The achievement the notice will be from.
    void AchievementNotice(Achievement achievement){
        //If the animation is playing makes the next achievement wait.
        while(achievementAnimation.GetCurrentAnimatorStateInfo(0).IsName("Achievement")){
            if(achievementAnimation.GetCurrentAnimatorStateInfo(0).IsName("Achievement")) return;
        }
        achievement.SetAchieved(true);
        titleUI.text = achievement.GetTitle();
        descriptionUI.text = achievement.GetDescription();
        achievementAnimation.Play("Achievement");
        achievementSound.Play();
        AddAchievement(achievement);
    }
    /// <summary>
    /// Adds achievement to the achievement UI list.
    /// </summary>
    /// <param name="achievement"> The achievement that will be added.
    void AddAchievement(Achievement achievement){
        foreach(Text t in achievementTemplate.GetComponentsInChildren<Text>()){
            if(t.name == "Title"){
                t.text = achievement.GetTitle();
            }
            if(t.name == "Desc"){
                t.text = achievement.GetDescription();
            }
        }
        Instantiate(achievementTemplate, achievementUI.transform);
    }
}
