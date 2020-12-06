using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    Rigidbody rb;
    public float sensitivity = 50;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void characterActions()
    {

    }
}
