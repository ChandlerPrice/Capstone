using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject focusObject;
    public float distance;
    public float height;
    public float dampening;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, focusObject.transform.position + focusObject.transform.TransformDirection(new Vector3(0f, height, distance)), Time.deltaTime);
        transform.LookAt(focusObject.transform);
    }
}
