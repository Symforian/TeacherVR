﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerObjectsSpawner : MonoBehaviour
{
    public GameObject[] Objects;
    public float angle = 20;

    private List<GameObject> ObjectsInstances = new List<GameObject>();

    private GameObject RightHand;
    private GameObject LeftHand;

    void Start()
    {
        ObjectsInstances.Clear();
        StartCoroutine(FindDevices());
    }

    private IEnumerator FindDevices()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        RightHand = VRTK_DeviceFinder.GetControllerRightHand();
        LeftHand = VRTK_DeviceFinder.GetControllerLeftHand();

        RightHand.GetComponent<VRTK_ControllerEvents>().GripPressed += RightHand_GripPressed;
        LeftHand.GetComponent<VRTK_ControllerEvents>().GripPressed += LeftHand_GripPressed;
        RightHand.GetComponent<VRTK_ControllerEvents>().GripReleased += DeSpawn;
        LeftHand.GetComponent<VRTK_ControllerEvents>().GripReleased += DeSpawn;
        RightHand.GetComponent<VRTK_ControllerEvents>().StartMenuPressed += DeSpawn;
        LeftHand.GetComponent<VRTK_ControllerEvents>().StartMenuPressed += DeSpawn;
        RightHand.GetComponent<VRTK_ControllerEvents>().StartMenuReleased += DeSpawn;
        LeftHand.GetComponent<VRTK_ControllerEvents>().StartMenuReleased += DeSpawn;
    }

    private void Spawn(Transform HandTransform)
    {
        int sign = 1;
        int count = 0;
        foreach (GameObject obj in Objects)
        {
            sign *= -1;
            count++;
            GameObject instantiate =
                Instantiate(obj, HandTransform.position + HandTransform.forward, HandTransform.rotation);
            float correct = 0;
            if (Objects.Length % 2 == 0) correct = angle / 2;
            instantiate.transform.RotateAround(HandTransform.position, HandTransform.up,
                sign * (count / 2) * angle - correct);
            instantiate.transform.position -= HandTransform.forward * 6f / 7f;
            /*instantiate.transform.eulerAngles =
                new Vector3(obj.transform.eulerAngles.x, instantiate.transform.eulerAngles.y, obj.transform.rotation.z);*/
            Rigidbody rb = instantiate.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            ObjectsInstances.Add(instantiate);
        }
    }

    private void DeSpawn(object sender, ControllerInteractionEventArgs e)
    {
        foreach (GameObject obj in ObjectsInstances)
        {
            if (!obj.GetComponent<VRTK_InteractableObject>().IsGrabbed())
                Destroy(obj);
        }
        ObjectsInstances.Clear();
    }

    private void RightHand_GripPressed(object sender, ControllerInteractionEventArgs e)
    {
        Spawn(RightHand.transform);
    }

    private void LeftHand_GripPressed(object sender, ControllerInteractionEventArgs e)
    {
        Spawn(LeftHand.transform);
    }
}