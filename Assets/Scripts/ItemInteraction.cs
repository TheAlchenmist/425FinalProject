using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public Transform playerTransform;
    public float thrustTime = 0.25f;

    private bool isThrusting = false;
    private Vector3 originalPosition, thrustForward;

    public void SetParentToPlayer()
    {
        // set player object as new parent
        transform.parent = playerTransform;

        // set position
        transform.localPosition = new Vector3(0.5f, 1f, 1);
        originalPosition = transform.localPosition;

        // reset rotation
        transform.rotation = playerTransform.rotation;

        // set rigidbody properties
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Drop()
    {
        if (!isThrusting)
        {
            // clear parent
            transform.parent = playerTransform.parent;

            // set rigidbody properties
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void Use()
    {
        thrustForward = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1);
        StartCoroutine(Thrust());
    }

    IEnumerator Thrust()
    {
        if (isThrusting)
        {
            yield break;
        }

        isThrusting = true;

        GetComponent<AudioSource>().Play();

        float interpolationParameter = 0;
        bool forward = true;

        while (isThrusting)
        {
            if (forward)
            {
                interpolationParameter += Time.deltaTime / (thrustTime / 2);

                if (interpolationParameter >= 1)
                {
                    interpolationParameter = 1;

                    forward = false;
                }
            }
            else
            {
                interpolationParameter -= Time.deltaTime / (thrustTime / 2);

                if (interpolationParameter <= 0)
                {
                    interpolationParameter = 0;

                    isThrusting = false;
                }
            }

            transform.localPosition = Vector3.Lerp(originalPosition, thrustForward, interpolationParameter);

            yield return null;
        }

        isThrusting = false;
    }
}
