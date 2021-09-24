using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Achievement object script.
public class Achievement
{
    private string title;
    private string description;
    private bool achieved;
    private int type;
    private int amount;
    public static int jump = 1;
    public static int climb = 2;
    public static int fall = 3;
    public static int bump = 4;
    public static int progress = 5;

    public Achievement(string title, string description, bool achieved, int type, int amount){
        this.title = title;
        this.description = description;
        this.achieved = achieved;
        this.type = type;
        this.amount = amount;
    }

    public void SetTitle(string title){
        this.title = title;
    }

    public string GetTitle(){
        return this.title;
    }

    public void SetDescription(string description){
        this.description = description;
    }

    public string GetDescription(){
        return this.description;
    }

    public void SetAchieved(bool achieved){
        this.achieved = achieved;
    }

    public bool GetAchieved(){
        return this.achieved;
    }

    public void SetType(int type){
        this.type = type;
    }

    public new int GetType(){
        return this.type;
    }

    public void SetAmount(int amount){
        this.amount = amount;
    }

    public int GetAmount(){
        return this.amount;
    }
}
