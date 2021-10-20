using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public bool canInteract = false;
    public GameObject interactableObject = null;
    public int waitDuration = 10;
    public float wait = 0;
    public void InteractWithObject()
    {
        if (wait <= 0)
        {
            if (canInteract && interactableObject != null)
            {
                Debug.Log("Interact");
                Debug.Log(interactableObject.tag);
                if (interactableObject.tag == "Car")
                {

                    this.gameObject.transform.position = interactableObject.transform.position;
                    this.gameObject.SetActive(false);
                    wait = waitDuration;
                }
                if (interactableObject.tag == "Grabbable")
                {

                }
            }
        }
        else
        {
            wait -= Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        canInteract = true;
        interactableObject = other.gameObject;
        Debug.Log("Enter");
    }

    public void OnTriggerExit(Collider other)
    {
        canInteract = false;
        interactableObject = null;
        Debug.Log("Exit");
    }
}
