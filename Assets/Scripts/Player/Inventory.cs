using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> inventoryBar;
    public GameObject inventoryBarUI;
    public int selectedSlot;
    public GameObject selectedObj;

    private void Start()
    {
        inventoryBar = new List<GameObject>();
        selectedSlot = 0;
    }

    void Update()
    {
        inventoryBarUI.transform.GetChild(selectedSlot).GetComponent<Image>().color = new Color(233, 231, 00);
    }

    public void SlotUp(InputAction.CallbackContext context)
    {
        print("Up");
    }

    public void SlotDown()
    {
        print("Down");
    }
}
