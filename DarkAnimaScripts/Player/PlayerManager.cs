using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;
    PlayerHabilities playerHabilities;
    PhotoreceptionSystem photoreception;
    GameManager gameManager;
    SoundManager soundManager;
    [Header("Life Settings")]
    public float maxHealth;
    public float health;
    public float recoveryRate;
    public float damage;
    [Range(0.05f, 0.2f)]public float lightDamageValue;
    [Header("Material Settings")]
    public SkinnedMeshRenderer skin;
    public Material defMaterial;
    public Material shadowMaterial;

    private void Awake(){
    
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerHabilities = GetComponent<PlayerHabilities>();
        photoreception = FindObjectOfType<PhotoreceptionSystem>();
        soundManager = FindObjectOfType<SoundManager>();
        gameManager = FindObjectOfType<GameManager>();

        health = maxHealth;

    }

    private void Update(){

        inputManager.HandleAllInputs();
        playerHabilities.HandleAllHabilities();
        Heal();
        MaterialChange();

    }

    private void FixedUpdate(){

        playerLocomotion.HandleAllMovement();

    }

    private void LateUpdate(){

        cameraManager.HandleAllCameraMovement();

    }

    private void Heal(){

        if(photoreception.lightValue >= lightDamageValue){

            health -= damage * Time.deltaTime;

        }else if(health < maxHealth){
            health += recoveryRate * Time.deltaTime;
        }

    }

    private void MaterialChange(){

        if(photoreception.lightValue >= inputManager.lightLevel){

            skin.material = defMaterial;

        }else{

            skin.material = shadowMaterial;

        }

    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag == "SoundCollider"){
            soundManager.SetCurrentStepSound(collider.GetComponent<AudioSource>());
        }else if(collider.gameObject.tag == "Tutorial"){
            gameManager.setHelperText(collider.GetComponent<Tutorial>().GetText());
            Destroy(collider.gameObject);
        }else if(collider.gameObject.tag == "CheckPoint"){
            gameManager.SetCheckPoint();
            Destroy(collider.gameObject);
        }
    }
}
