using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCell : MonoBehaviour
{
    public GameObject playerObject, padOne, padTwo;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && padOne.GetComponent<EntryConsoleHandler>().IsOn() && padTwo.GetComponent<EntryConsoleHandler>().IsOn())
        {
            playerObject.GetComponent<Inventory>().AddToInventory(gameObject);
            Destroy(gameObject);
        }
    }
}
