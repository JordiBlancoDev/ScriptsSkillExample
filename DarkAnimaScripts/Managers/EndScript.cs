using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    GameManager gameManager;

    void Awake(){
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerStay(Collider collider){
        if(collider.gameObject.tag == "Player"){
            gameManager.EndGame();
        }
    }

    void OnTriggerExit(Collider collider){
        if(collider.gameObject.tag == "Player"){
            gameManager.HideEndGamePanel();
        }
    }
}
