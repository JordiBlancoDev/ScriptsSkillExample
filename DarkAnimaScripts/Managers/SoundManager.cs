using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]private AudioSource currentStepSound;
    InputManager inputManager;

    void Awake(){
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        if(inputManager.moveAmount > 0f && !currentStepSound.isPlaying && inputManager.isConcentrating == 0f){
            if(inputManager.isRunning == 1f && inputManager.isCrouching == 0f){
                currentStepSound.pitch = 1.5f;
            }else if(inputManager.isCrouching == 1f){
                currentStepSound.pitch = 0.65f;
            }else{
                currentStepSound.pitch = 0.91f;
            } 
            currentStepSound.Play();
            
        }
    }

    public void SetCurrentStepSound(AudioSource sound){
        currentStepSound = sound;
    }
}
