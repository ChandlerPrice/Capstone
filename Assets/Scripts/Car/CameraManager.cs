using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject focusObject;
    public bool isPlayer = true;
    public float distance;
    public float height;
    public float dampening;

    private void Update()
    {
        if(isPlayer)
        {
            this.gameObject.transform.localPosition = new Vector3(0, 1, 0);
            if(focusObject != null && focusObject.tag == "Car")
            {
                isPlayer = false;
            }
        }
        else if(focusObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, focusObject.transform.position + focusObject.transform.TransformDirection(new Vector3(0f, height, distance)), Time.deltaTime);
            transform.LookAt(focusObject.transform);
        }
        else
        {
            isPlayer = true;
        }
    }
}
