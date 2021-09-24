using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    PlayerHabilities playerHabilities;
    private float lifeTime;
    
    void Start()
    {
        playerHabilities = GameObject.FindObjectOfType<PlayerHabilities>().GetComponent<PlayerHabilities>();
        GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward + Vector3.up / 2f) * playerHabilities.waveForce, ForceMode.Impulse);
        lifeTime = Time.time + 5f;
    }

    void OnTriggerEnter(Collider collider){

        if(!collider.gameObject.CompareTag("Interactuable") && collider.gameObject.name != "Parabola Particles(Clone)" && collider.gameObject.name != "Drone"){

            GetComponent<MeshRenderer>().enabled = false;

        }

    }

    void Update(){

        if(Time.time >= lifeTime){
            Destroy(this.gameObject);
        }  

    }

}
