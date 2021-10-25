using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Interact : MonoBehaviour
{
    public FPSController fpsController;
    public CameraManager cameraManager;
    public CharacterController characterController;
    public Inventory inventory;
    public bool canInteract = false;
    public bool isInteracting = false;
    public GameObject interactableObject = null;
    public int waitDuration = 1;
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
                    if(isInteracting)
                    {
                        this.gameObject.transform.position = new Vector3(interactableObject.transform.position.x, interactableObject.transform.position.y + 1, interactableObject.transform.position.z);
                        cameraManager.focusObject = null;
                        cameraManager.height = 1;
                        cameraManager.distance = 0.25f;
                        characterController.enabled = true;
                        fpsController.enabled = true;
                        fpsController.rotateView = true;
                        isInteracting = false;
                    }
                    else
                    {
                        this.gameObject.transform.position = new Vector3(interactableObject.transform.position.x, interactableObject.transform.position.y + 1, interactableObject.transform.position.z);
                        cameraManager.focusObject = interactableObject;
                        cameraManager.height = 2;
                        cameraManager.distance = -4;
                        characterController.enabled = false;
                        fpsController.enabled = false;
                        fpsController.rotateView = false;
                        isInteracting = true;
                    }
                    wait = waitDuration;
                }
                if (interactableObject.tag == "Grabbable")
                {
                    if(inventory.selectedObj == null)
                    {
                        inventory.inventoryBar.Insert(inventory.selectedSlot, interactableObject.gameObject);
                        inventory.selectedObj = interactableObject.gameObject;
                        interactableObject.gameObject.transform.parent = this.gameObject.transform;
                        interactableObject.gameObject.SetActive(false);
                    }
                    else
                    {
                        inventory.inventoryBar.RemoveAt(inventory.selectedSlot);
                        inventory.selectedObj = null;
                        interactableObject.gameObject.transform.parent = null;
                        interactableObject.gameObject.transform.Translate(0, 1, 1);
                        interactableObject.gameObject.SetActive(true);
                    }
                    wait = waitDuration;
                }
            }
        }
    }
    private void Update()
    {
        wait -= Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        canInteract = true;
        interactableObject = other.gameObject;
        Debug.Log("Enter");
        interactableObject.GetComponent<Outline>().enabled = true;
    }

    public void OnTriggerExit(Collider other)
    {
        canInteract = false;
        interactableObject.GetComponent<Outline>().enabled = false;
        interactableObject = null;
        Debug.Log("Exit");
    }
}
