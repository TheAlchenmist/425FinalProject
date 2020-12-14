using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    // Disable Rigid Body until the wrench touches
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<ItemInteraction>() && rigidBody.isKinematic)
        {
            rigidBody.isKinematic = false;
            GetComponent<AudioSource>().Play();
            StartCoroutine(DeleteAfterTwoSeconds());
        }
    }

    IEnumerator DeleteAfterTwoSeconds()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
