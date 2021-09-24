using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script for the camera movement.
public class CameraManager : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed;
    public bool vertical;
    public bool horizontal;

    void FixedUpdate()
    {
        // Follows player vertically.
        if(vertical && !horizontal) transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, player.position.y, transform.position.z), smoothSpeed * Time.deltaTime);
        // Follows player horizontally.
        if(horizontal && !vertical) transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, transform.position.y, transform.position.z), smoothSpeed * Time.deltaTime);
        // Follows player in both axis.
        if(horizontal && vertical) transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), smoothSpeed * Time.deltaTime);
    }
}
