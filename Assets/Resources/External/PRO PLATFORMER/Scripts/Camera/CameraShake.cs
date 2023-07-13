


                                                      // Made by VEOdev //

                              //                THANK YOU FOR USING OUR ASSET                 //
                              //     PLEASE RATE THE ASSET AND LEAVE A REVIEW IT HELP ALOT    //


                              // NOTE : This script need to be attached to the camera main //
                       // make sure to have the animation component on it with the animation clip //

using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
    }
    public void Shake()
    {
        anim.Play();
    }

    
}
