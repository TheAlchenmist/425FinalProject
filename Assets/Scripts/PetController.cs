using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PetInputHandler))]
public class PetController : MonoBehaviour
{
    [Header("References")]
    public Camera petCamera;

    [Header("Movement")]
    public float maxSpeedInAir = 10f;
    public float accelerationSpeedInAir = 25f;
    public float airFriction = .2f;

    [Header("Rotation")]
    public float rotationSpeed = 200f;
    public float zRotation = .1f;

    [Header("Camera")]
    public float cameraHeightRatio = 0.9f;
    public float capsuleHeightStanding = 1.8f;

    public Vector3 characterVelocity { get; set; }
    public bool isGrounded { get; private set; }
    public bool hasJumpedThisFrame { get; private set; }
    public bool isDead { get; private set; }
    public bool isCrouching { get; private set; }
    public float RotationMultiplier = 1f;

    PetInputHandler m_InputHandler;
    CharacterController m_Controller;
    Vector3 m_CharacterVelocity;

    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<PetInputHandler>();
        m_Controller.enableOverlapRecovery = true;

    }


    void Update()
    {
        hasJumpedThisFrame = false;

        HandleCharacterMovement();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                /*if (hit.collider.GetComponent<BoxCollider>())
                {
                    var door = hit.collider.gameObject.GetComponent<PowerCellHolder>();
                    if (door != null)
                    {
                        door.StartCoroutine(door.OpenDoor());
                    }
                    var chest = hit.collider.gameObject.GetComponent<Chest>();
                    if (chest != null)
                    {
                        chest.OpenChest();
                    }
                }*/
            }
        }
    }



    void HandleCharacterMovement()
    {
        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3((m_InputHandler.GetLookInputsVertical() * rotationSpeed * RotationMultiplier), (m_InputHandler.GetLookInputsHorizontal() * rotationSpeed * RotationMultiplier), 0f), Space.Self);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        }

        {
            // Allows players to reorient based on horizontal arrow keys
            // transform.Rotate(new Vector3(0f, 0f, -m_InputHandler.GetRotateInput() * zRotation), Space.Self);

            // converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

            // used to add air friction for a more slippery movement in the air
            Vector3 changeInMove = Vector3.zero;
            if (worldspaceMoveInput != Vector3.zero)
            {
                changeInMove = worldspaceMoveInput * maxSpeedInAir;
            }
            else if (characterVelocity != Vector3.zero)
            {
                changeInMove = characterVelocity * -airFriction;
            }

            // target velocity
            Vector3 targetVelocity = characterVelocity + changeInMove;
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, maxSpeedInAir);

            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, accelerationSpeedInAir * Time.deltaTime);

            m_Controller.Move(characterVelocity * Time.deltaTime);
        }
    }

}