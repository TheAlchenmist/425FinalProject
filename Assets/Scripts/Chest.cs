using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animation animator = default;
    private bool isOpen = false;
    public bool tutOpen = false;

    void Start()
    {
        animator = GetComponent<Animation>();
    }

    public bool OpenChest()
    {
        if (!isOpen) { 
            animator.Play();
            isOpen = true;
            tutOpen = true;

            return isOpen;
        }

        return false;
    }
}
