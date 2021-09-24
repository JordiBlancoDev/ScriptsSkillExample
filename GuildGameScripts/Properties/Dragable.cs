using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IPointerDownHandler
{
    public int heightThreshold; // Minimal height the user needs to drag for the quest to be dragable.
    GameObject contentsParent;
    GameObject parentUI;

    bool startDragging;
    bool touched;

    void Start()
    {
        contentsParent = GameObject.Find("Contents");
        parentUI = GameObject.Find("UI");
    }

    void Update()
    {
        if(touched) // Checks if the object is touched.
        {
            if(Input.touchCount > 0) // Checks if we are still touching the screen.
            {
                Touch touch = Input.GetTouch(0);
                // Transforms the touch position and the object position from pixels to units.
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 objectPosition = Camera.main.ScreenToWorldPoint(transform.position);
                // Checks if the touch is being dragged more than the threshold for the object to start following.
                if(touchPosition.y > objectPosition.y + heightThreshold) startDragging = true;
                // Checks if the touch has been released.
                if(touch.phase == TouchPhase.Ended) Released();
            }
            
        }

        if(startDragging) Drag();
    }

    /// <summary>
    /// Drags the object this script is attached to to the touch position.
    /// </summary>
    void Drag()
    {
        transform.position = Input.mousePosition; // Mouse position == Touch position
        transform.SetParent(parentUI.transform); 
    }

    /// <summary>
    /// Is called when the touch occurs inside the object
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.gray;
        touched = true;
    }

    /// <summary>
    /// Resets the object to the content container
    /// </summary>
    void Released()
    {
        GetComponent<Image>().color = Color.white;
        touched = false;
        startDragging = false;
        transform.SetParent(contentsParent.transform);
    }
}
