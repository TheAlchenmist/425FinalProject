using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float lookSensitivity = 1f;
    PlayerController m_PlayerController;
    public bool m_OrderWasHeld;

    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();

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
            return move;
        }

        return Vector3.zero;
    }

    public bool GetJumpInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        return false;
    }

    public bool GetJumpHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        return false;
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
        return GetMouseLookAxis("Mouse Y") * -1f;
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
