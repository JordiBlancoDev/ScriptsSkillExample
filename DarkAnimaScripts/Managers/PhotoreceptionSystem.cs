using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoreceptionSystem : MonoBehaviour
{
    [Header("Settings")]
    public Camera lightScanner; // The camera that will scan the light.
    public bool logLightValue; // When true will show light value (debug purposes).
    public float updateTime; // Time between scans (default = 0.1f).

    [HideInInspector] public float lightValue;

    private const int textureSize = 1;

    private Texture2D texLight;
    private RenderTexture texTemp;
    private Rect rectLight;
    private Color lightPixel;

    private void Start(){
        StartLightDetection();
    }

    // Prepare all needed variables and start the light detection coroutine.
    private void StartLightDetection(){

        texLight = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, false);
        texTemp = new RenderTexture(textureSize, textureSize, 24);
        rectLight = new Rect(0f, 0f, textureSize, textureSize);

        StartCoroutine(LightDetectionUpdate(updateTime));

    }

    // Updates the light value every x seconds (x = updateTime).
    private IEnumerator LightDetectionUpdate(float updateTime){

        while(true){

            // Set the target texture of the camera.
            lightScanner.targetTexture = texTemp;
            //Render into the set target texture
            lightScanner.Render();

            // Set the target texture as the active rendered texture.
            RenderTexture.active = texTemp;
            // Read the active rendered texture.
            texLight.ReadPixels(rectLight, 0, 0);

            // Reset the active rendered texture.
            RenderTexture.active = null;
            // Reset the target texture of the cam.
            lightScanner.targetTexture = null;

            // Read the pixel in the middle of the texture.
            lightPixel = texLight.GetPixel(textureSize / 2, textureSize / 2);

            // Calculate light value, based on color intensity (from 0 to 1).
            lightValue = (lightPixel.r + lightPixel.g + lightPixel.b) / 3f;

            if(logLightValue){
                Debug.Log("Light Value: " + lightValue);
            }

            yield return new WaitForSeconds(updateTime);

        }

    }
    
}
