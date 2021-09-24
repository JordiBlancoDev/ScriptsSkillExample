using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Quest : MonoBehaviour
{
    public Text difficultyText;
    public Text levelText;
    public Text goldText;
    public int level {set; get;}
    public string difficulty {set; get;}
    public int gold {set; get;}
    public int experience {set; get;}


    GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        RandomizeQuest();
        SetQuest();
    }

    void Update()
    {

    }

    void RandomizeQuest()
    {
        level = Random.Range(1 * gameManager.GetGuildLevel(), 3 * gameManager.GetGuildLevel());
        gold = Random.Range(1 * level/2, 3 * level/2);
        if(gold == 0) gold = 1;
        experience = Random.Range(5 * level/2, 10 * level/2);
        
        
        if(level < 15) difficulty = "F";         
        else if(level >= 16 && level < 30) difficulty = "D";
        else if(level >= 31 && level < 45) difficulty = "C";
        else if(level >= 46 && level < 60) difficulty = "B";
        else if(level >= 61 && level < 75) difficulty = "B";
        else if(level >= 76 && level < 90) difficulty = "A";
        else if(level >= 91) difficulty = "S";
    }

    void SetQuest()
    {
        levelText.text = "Lv. " + level;
        difficultyText.text = difficulty;
        goldText.text = gold + " G.";
    }
}
