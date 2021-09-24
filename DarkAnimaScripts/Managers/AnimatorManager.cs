using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;

    private void Awake(){

        horizontal = Animator.StringToHash("Horizontal"); // Gets horizontal id from animator.
        vertical = Animator.StringToHash("Vertical"); // Gets vertical id from animator.

    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isCrouching, bool isConcentrating){

        // Animation Snapping (smooth transition between blend trees's states).
        float snpapedHorizontal;
        float snappedVertical;
        
        #region Snapped Horizontal
            if(horizontalMovement > 0 && horizontalMovement < 0.55f){
                snpapedHorizontal = 0.5f;
            }
            else if(horizontalMovement > 0.55f){
                snpapedHorizontal = 1f;
            }
            else if(horizontalMovement < 0f && horizontalMovement > -0.55f){
                snpapedHorizontal = -0.5f;
            }
            else if(horizontalMovement < -0.55f){
                snpapedHorizontal = -1f;
            }
            else{
                snpapedHorizontal = 0f;
            }
        #endregion
        #region Snapped Vertical
            if(verticalMovement > 0 && verticalMovement < 0.55f){
                snappedVertical = 0.5f;
            }
            else if(verticalMovement > 0.55f){
                snappedVertical = 1f;
            }
            else if(verticalMovement < 0f && verticalMovement > -0.55f){
                snappedVertical = -0.5f;
            }
            else if(verticalMovement < -0.55f){
                snappedVertical = -1f;
            }
            else{
                snappedVertical = 0f;
            }
        #endregion

        animator.SetFloat(horizontal, snpapedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);

        animator.SetBool("isCrouching", isCrouching);

        animator.SetBool("isConcentrating", isConcentrating);

    }
}
