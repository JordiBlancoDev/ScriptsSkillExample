using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Data
{
    // Remember to add the variables to the SaveData from GameManager aswell.
    public string guildName {get; set;}
    public int guildLevel {get; set;}
    public int currentGuildExp {get; set;}
    public int day {get; set;}
    public int guildGold {get; set;}
    public int happiness {get; set;}
    public int guildPosition {get; set;}

}
