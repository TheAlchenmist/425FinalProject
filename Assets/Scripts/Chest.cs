using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animation animator = default;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animation>();
    }

    public void OpenChest()
    {
        if (!isOpen) { 
            animator.Play();
            isOpen = true;
        }
    }
}
