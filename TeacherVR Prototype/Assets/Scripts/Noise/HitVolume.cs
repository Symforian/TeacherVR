﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVolume : MonoBehaviour {
   // public static int minLevelOfLoudness = 6;


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " " + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > GameController.Instance.MicInput.minSilencingForce)
        {
            //sound //particle
            Noise.doneSomethingLoudEnough = true;
        }
    }
}
