using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    public GameObject[] players;
    public Camera[] cameras;

    private int currentIndex;
    TutorialController m_tutorialController;

    void Start()
    {
        m_tutorialController = GetComponent<TutorialController>();


        // setting the index to 0
        currentIndex = 0;

        // activating the current camera and the current player
        for(int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
            if (players[i].tag == "Player")
            {
                players[i].gameObject.GetComponent<PlayerController>().enabled = false;
            }
            else
            {
                players[i].gameObject.GetComponent<PetController>().enabled = false;
            }
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && m_tutorialController.petSwitch == true)
        {
            currentIndex++;
            int cur = (currentIndex) % cameras.Length;
            int prev = (currentIndex-1) % cameras.Length;
            cameras[prev].gameObject.SetActive(false);
            cameras[cur].gameObject.SetActive(true);
            if (players[cur].tag == "Player")
            {
                players[cur].gameObject.GetComponent<PlayerController>().enabled = true;
                players[prev].gameObject.GetComponent<PetController>().enabled = false;
            }
            else
            {
                players[cur].gameObject.GetComponent<PetController>().enabled = true;
                players[prev].gameObject.GetComponent<PlayerController>().enabled = false;
            }
            currentIndex = cur;
        }
    }

    public GameObject CheckActive()
    {
        return players[currentIndex % cameras.Length].gameObject;
    }
}
