using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInputHandler : MonoBehaviour
{
    public float lookSensitivity = 1f;
    PetController m_PetController;
    public bool m_OrderWasHeld;

    void Start()
    {
        m_PetController = GetComponent<PetController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        m_OrderWasHeld = GetOrderDown();
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            move = Vector3.ClampMagnitude(move, 1);
            // Debug.Log(move);
            return move * -1;
        }

        return Vector3.zero;
    }

    public float GetRotateInput()
    {
        if (CanProcessInput())
        {
            float rotate = Input.GetAxisRaw("Horizontal");
            return rotate;
        }

        return 0f;
    }

    public bool GetOrderInput()
    {
        return GetOrderDown() && !m_OrderWasHeld;
    }

    public bool GetOrderReleased()
    {
        return !GetOrderDown() && m_OrderWasHeld;
    }

    public bool GetOrderDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Fire1");
        }

        return false;
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseLookAxis("Mouse X");
    }

    public float GetLookInputsVertical()
    {
        return GetMouseLookAxis("Mouse Y");
    }

    float GetMouseLookAxis(string input)
    {
        if (CanProcessInput())
        {
            float i = Input.GetAxisRaw(input);
            i = i * lookSensitivity * 0.01f;
            return i;
        }
        return 0f;
    }
}

