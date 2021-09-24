using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    //// ADVENTURER STATS ////
    int level;
    int health;
    int defense;
    int attack;
    int evasion; 
    int mana;
    int magicAttack;
    [Header("Classes")]
    public string[] classes; // Array of string containing the different classes
    string classType;
    //// VISUAL APPEARANCE ////
    [Header("Hair")]
    public Sprite[] hairs; // Array of the different hair sprites.
    public SpriteRenderer hairRenderer; // The renderer of the hair.
    [Header("Eyes")]
    public SpriteRenderer eyesRenderer; // Renderer of the eyes.
    [Header("Body")]
    public SpriteRenderer bodyRenderer; // Renderer of the body.
    public Color[] colors; // Possible colors for the body.
    [Header("Clothes")]
    public Sprite[] clothes; // Array of the different cloth sprites.
    public SpriteRenderer clothRenderer; // Renderer of the cloth.
    [Header("Accessories")]
    public Sprite[] accessories; // Array of all the accessories.
    public SpriteRenderer accessoryRenderer; // Renderer of the accessory.
    [Header("Weapons")]
    public Sprite[] weapons; // Array of all weapons.
    public SpriteRenderer weaponRenderer; // Renderer of the weapons.
    

    //// OTHERS ////
    GameManager gameManager;
    bool showStatsUI;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        SetAspect();
        SetStats();

        gameManager.SetCurrentAdventurer(this);
        
        showStatsUI = false;
        gameManager.ShowUIStats(showStatsUI);
    }
    /// <summary>
    /// Set the stats of the adventurer whenever is instantiated.
    /// </summary>
    void SetStats()
    {
        level = Random.Range(1 * gameManager.GetGuildLevel(), 3 * gameManager.GetGuildLevel()); // Sets the level of the player randomly, increases with the guild level.

        health = Random.Range(10 * level, 50 * level); // Sets the health of the adventurer randomly, increases with its level.
        defense = Random.Range(1, 5 * level); 
        attack = Random.Range(5 * level, 20 * level); 
        evasion = Random.Range(0, 2 * level); 
        mana = Random.Range(15 * level, 42 * level); 
        magicAttack = Random.Range(1 * level, 17 * level); 

        classType = classes[Random.Range(0, classes.Length - 1)]; // Sets the class of the adventurer randomly between the designed classes.
    }
    /// <summary>
    /// Sets the aspect of the adventurer randomly.
    /// </summary>
    void SetAspect()
    {
        eyesRenderer.color = Random.ColorHSV(); // Chooses a random color and applies it to the sprite.
        bodyRenderer.color = colors[Random.Range(0, colors.Length - 1)]; // Chooses a random color from the array and gives it to the sprite.

        hairRenderer.sprite = hairs[Random.Range(0, hairs.Length - 1)]; // Chooses a random sprite from the array and gives it to the sprite renderer.
        hairRenderer.color = Random.ColorHSV();

        clothRenderer.sprite = clothes[Random.Range(0, clothes.Length - 1)];
        clothRenderer.color = Random.ColorHSV();
        
        accessoryRenderer.sprite = accessories[Random.Range(0, accessories.Length -1)];
        accessoryRenderer.color = Random.ColorHSV();

        weaponRenderer.sprite = weapons[Random.Range(0, weapons.Length - 1)];
        weaponRenderer.color = Random.ColorHSV();
    }

    void OnMouseDown()
    {
        showStatsUI = !showStatsUI;
        gameManager.ShowUIStats(showStatsUI);
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public int GetDefense()
    {
        return defense;
    }

    public void SetDefense(int defense)
    {
        this.defense = defense;
    }

    public int GetAttack()
    {
        return attack;
    }

    public void SetAttack(int attack)
    {
        this.attack = attack;
    }

    public int GetEvasion()
    {
        return evasion;
    }

    public void SetEvasion(int evasion)
    {
        this.evasion = evasion;
    }

    public int GetMana()
    {
        return mana;
    }

    public void SetMana(int mana)
    {
        this.mana = mana;
    }

    public int GetMagicAttack()
    {
        return magicAttack;
    }

    public void SetMagicAttack(int magicAttack)
    {
        this.magicAttack = magicAttack;
    }

    public string GetClassType()
    {
        return classType;
    }

    public void SetClassType(string classType)
    {
        this.classType = classType;
    }
}
