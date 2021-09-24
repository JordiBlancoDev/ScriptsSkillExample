using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSelector : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == "Quest") gameManager.SetSelectedQuest(collider2D.gameObject.GetComponent<Quest>());
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.tag == "Quest") gameManager.SetSelectedQuest(null);
    }
}
