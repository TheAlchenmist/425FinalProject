using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public GameObject playerObject;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            playerObject.GetComponent<Inventory>().AddToInventory(gameObject);
            Destroy(gameObject);
        }
    }
}
