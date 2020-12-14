using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] texts;
    private int textIndex = 0;
    public bool openDoors = false;
    public bool petSwitch = false;
    public GameObject player, pet, door, chest;
    PlayerInputHandler m_PlayerInputHandler;
    PetInputHandler m_PetInputHandler;
    Chest m_chest;

    private void Start()
    {
        m_PlayerInputHandler = player.GetComponent<PlayerInputHandler>();
        m_PetInputHandler = pet.GetComponent<PetInputHandler>();
        m_chest = chest.GetComponent<Chest>();
    }

    void Update()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (i == textIndex)
            {
                texts[i].SetActive(true);
            }
            else
            {
                texts[i].SetActive(false);
            }
        }
        // movement tutorial
        if(textIndex == 0)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                textIndex++;
            }
        }
        else if (textIndex == 1 && Input.GetKeyDown(KeyCode.V))
        {
            textIndex++;
        }
        // jump tutorial
        else if (textIndex == 2)
        {
            if (m_PlayerInputHandler.GetJumpInput())
            {
                textIndex++;
                petSwitch = true;
            }
        }
        else if (textIndex == 3 && Input.GetKeyDown(KeyCode.V))
        {
            textIndex++;
        }
        // switch to drone control
        else if (textIndex == 4)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                textIndex++;
                openDoors = true;
            }
        }
        else if (textIndex == 5 && Input.GetKeyDown(KeyCode.V))
        {
            textIndex++;
        }
        // open Panels and Doors
        else if (textIndex == 6)
        {
            if (door.GetComponent<EntryConsoleHandler>().tutOpen)
            {
                textIndex++;
            }
        }
        // Get the Wrench
        else if (textIndex == 7)
        {
            if (m_chest.tutOpen)
            {
                textIndex++;
            }
        }
        else if (textIndex == 8 && Input.GetKeyDown(KeyCode.V))
        {
            textIndex++;
        }
        else if (textIndex == 9 && Input.GetKeyDown(KeyCode.V))
        {
            textIndex++;
        }
        else if (textIndex == 10 && Input.GetKeyDown(KeyCode.V))
        {
            textIndex++;
        }
        else if (textIndex == 11 && Input.GetKeyDown(KeyCode.V))
        {
            textIndex++;
        }
    }
}
