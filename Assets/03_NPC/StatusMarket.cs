using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusMarket : MonoBehaviour
{
    GameObject NPC_UI;
    bool UI_Activate;
    public void SetNPC_UIState()
    {
        if (NPC_UI.gameObject.activeSelf == false)
        {
            UI_Activate = NPC_UI.active = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject.Find("Player").GetComponent<PlayerController>().SetIsPlayerMove(true);
        }
        else
        {
            UI_Activate = NPC_UI.active = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameObject.Find("Player").GetComponent<PlayerController>().SetIsPlayerMove(false);

        }
    }

    public bool GetNPC_UIState() { return UI_Activate; }
    void Start()
    {
        NPC_UI = GameObject.Find("NPC_UI");
        NPC_UI.SetActive(false);
        UI_Activate = NPC_UI.active;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

  
}
