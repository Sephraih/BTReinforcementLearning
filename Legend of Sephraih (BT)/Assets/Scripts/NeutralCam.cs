using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralCam : MonoBehaviour
{

    
    public Animator ShakeAnimation; // various actions in the game use a shake animation to simulate collision effects

    // follow target
    void LateUpdate()
    {
        
    }

    // plays a shake animation using an animator attached to the camera object, essentially altering the camera's position for the duration of  the animation
    public void CamShake()
    {
        ShakeAnimation.SetTrigger("shake");
    }

   


}
