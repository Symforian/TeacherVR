﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurmursManagement : MonoBehaviour {


    static public AudioSource MurmursSource;
    static public bool murmurs;
	// Use this for initialization
	void Start () {
        murmurs = false;
        MurmursSource = GetComponentInChildren<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if(!murmurs && GameController.Instance.EventsManager.ListOfEvents[0].name == "Noise")
        {
            MurmursSource.volume = 0.25f;
            MurmursSource.Play();
            murmurs = true;
        }
	}

}