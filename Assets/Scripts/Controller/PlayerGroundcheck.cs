using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundcheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController._instance.gameObject)
            return;
        PlayerController._instance.JumpState(true);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController._instance.gameObject)
        {
            PlayerController._instance.JumpState(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == PlayerController._instance.gameObject)
            return;
        PlayerController._instance.JumpState(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == PlayerController._instance.gameObject)
            return;
        PlayerController._instance.JumpState(true);

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == PlayerController._instance.gameObject)
        {
            PlayerController._instance.JumpState(false);
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if(collisionInfo.gameObject == PlayerController._instance.gameObject)
            return;
        PlayerController._instance.JumpState(true);
    }
}
