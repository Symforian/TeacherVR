﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlaySoundOnInteract : MonoBehaviour
{
    public SamplesList Sound;

    public ActionList Action;

    public float Volume = 1;
    public Vector2 RandomLoopDelay;
    public bool OnlyWhenGameInProgress = false;

    private VRTK_InteractableObject io;
    private VRTK_SnapDropZone sdz;
    private Rigidbody rb;

    private const float ThrowVelocity = 6;

    private float LastTime;
    private float Delay;

    public enum ActionList
    {
        Snaped,
        Unsnaped,
        Touched,
        Untouched,
        Grabbed,
        Ungrabbed,
        Enable,
        Disable,
        Throw,
        RandomLoop
    }

    /*void Awake()
    {
        if (Action == ActionList.Throw) rb = GetComponent<Rigidbody>();
    }*/

    void OnEnable()
    {
        if (Action == ActionList.Enable) Play();
    }

    void OnDisable()
    {
        if (Action == ActionList.Disable) Play();
    }

    private void OnDestroy()
    {
        switch (Action)
        {
            case ActionList.Snaped:
                if (sdz != null) sdz.ObjectSnappedToDropZone -= PlaySDZ;
                break;
            case ActionList.Unsnaped:
                if (sdz != null) sdz.ObjectUnsnappedFromDropZone -= PlaySDZ;
                break;
            case ActionList.Touched:
                if (io != null) io.InteractableObjectTouched -= PlayIO;
                break;
            case ActionList.Untouched:
                if (io != null) io.InteractableObjectUntouched -= PlayIO;
                break;
            case ActionList.Grabbed:
                if (io != null) io.InteractableObjectGrabbed -= PlayIO;
                break;
            case ActionList.Ungrabbed:
                if (io != null) io.InteractableObjectUngrabbed -= PlayIO;
                break;
        }
    }

    void Start()
    {
        io = GetComponent<VRTK_InteractableObject>();
        sdz = GetComponent<VRTK_SnapDropZone>();
        LastTime = 0;
        Delay = 6;

        if (Action == ActionList.RandomLoop) Delay = Random.Range(RandomLoopDelay.x, RandomLoopDelay.y);

        switch (Action)
        {
            case ActionList.Snaped:
                if (sdz != null) sdz.ObjectSnappedToDropZone += PlaySDZ;
                break;
            case ActionList.Unsnaped:
                if (sdz != null) sdz.ObjectUnsnappedFromDropZone += PlaySDZ;
                break;
            case ActionList.Touched:
                if (io != null) io.InteractableObjectTouched += PlayIO;
                break;
            case ActionList.Untouched:
                if (io != null) io.InteractableObjectUntouched += PlayIO;
                break;
            case ActionList.Grabbed:
                if (io != null) io.InteractableObjectGrabbed += PlayIO;
                break;
            case ActionList.Ungrabbed:
                if (io != null) io.InteractableObjectUngrabbed += PlayIO;
                break;
            case ActionList.Throw:
                rb = GetComponent<Rigidbody>();
                break;
        }
    }

    void Update()
    {
        if (Action == ActionList.Throw && rb != null && rb.velocity.magnitude > ThrowVelocity)
        {
            if (Time.time > LastTime + Delay)
            {
                LastTime = Time.time;
                Play();
            }
        }
        if (Action == ActionList.RandomLoop)
        {
            if (Time.time > LastTime + Delay)
            {
                LastTime = Time.time;
                Play();
            }
        }
    }

    private void Play()
    {
        if (OnlyWhenGameInProgress && GameController.Instance.IsGameInProgress() || !OnlyWhenGameInProgress)
            GameController.Instance.SoundManager.Play3DAt(Sound, transform.position, Volume);
    }

    private void PlaySDZ(object sender, SnapDropZoneEventArgs e)
    {
        Play();
    }

    private void PlayIO(object sender, InteractableObjectEventArgs e)
    {
        Play();
    }
}