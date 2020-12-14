using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Ground")]
    public float gravityDownForce = 20f;
    public LayerMask groundCheckLayers = -1;
    public float groundCheckDistance = 0.05f;

    [Header("Movement")]
    public float maxSpeedOnGround = 10f;
    public float movementSharpnessOnGround = 15;
    public float maxSpeedInAir = 10f;
    public float accelerationSpeedInAir = 25f;

    [Header("Rotation")]
    public float rotationSpeed = 200f;
    [Range(0.1f, 1f)]
    public float aimingRotationMultiplier = 0.4f;

    [Header("Jump")]
    public float jumpForce = 9f;

    [Header("Camera")]
    public float cameraHeightRatio = 0.9f;
    public float capsuleHeightStanding = 1.8f;

    public Vector3 characterVelocity { get; set; }
    public bool isGrounded { get; private set; }
    public bool hasJumpedThisFrame { get; private set; }
    public bool isDead { get; private set; }
    public bool isCrouching { get; private set; }
    public float RotationMultiplier = 1f;

    PlayerInputHandler m_InputHandler;
    CharacterController m_Controller;
    Vector3 m_GroundNormal;
    Vector3 m_CharacterVelocity;
    Vector3 m_LatestImpactSpeed;
    float m_LastTimeJumped = 0f;
    float m_CameraVerticalAngle = 0f;
    float m_TargetCharacterHeight = 1f;

    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.07f;

    bool chestIsOpen = false;

    void Start()
    {
        m_TargetCharacterHeight = capsuleHeightStanding;
        m_Controller = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<PlayerInputHandler>();
        m_Controller.enableOverlapRecovery = true;

        UpdateCharacterHeight(true);
    }

    
    void Update()
    {
        hasJumpedThisFrame = false;

        bool wasGrounded = isGrounded;
        GroundCheck();

        UpdateCharacterHeight(false);

        HandleCharacterMovement();

        bool hasWrench = false;
        GameObject wrench = GameObject.Find("DWMU");
        if (wrench.transform.parent == gameObject.transform)
        {
            hasWrench = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                // if click and ray hits chest and chest is open then set wrench parent to player object
                if (hit.collider.gameObject.name == "Chest" && chestIsOpen)
                {
                    if (!hasWrench) {
                        wrench.GetComponent<ItemInteraction>().SetParentToPlayer();
                    }
                }

                if (hit.collider.GetComponent<BoxCollider>())
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
                        chestIsOpen = true;
                    }
                    var wrenchScript = hit.collider.gameObject.GetComponent<ItemInteraction>();
                    if (wrenchScript != null)
                    {
                        wrenchScript.SetParentToPlayer();
                    }
                }
            }
        }
        
        // item interaction
        if (hasWrench) {
            if (Input.GetMouseButtonDown(1))
            {
                wrench.GetComponent<ItemInteraction>().Drop();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                wrench.GetComponent<ItemInteraction>().Use();
            }
        }
    }

    void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance = isGrounded ? (m_Controller.skinWidth + groundCheckDistance) : k_GroundCheckDistanceInAir;

        // reset values before the ground check
        isGrounded = false;
        m_GroundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height), m_Controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                // storing the upward direction for the surface found
                m_GroundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(m_GroundNormal))
                {
                    isGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > m_Controller.skinWidth)
                    {
                        m_Controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            m_Controller.height = m_TargetCharacterHeight;
            m_Controller.center = Vector3.up * m_Controller.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.up * m_TargetCharacterHeight * cameraHeightRatio;
        }
    }

    void HandleCharacterMovement()
    {
        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * rotationSpeed * RotationMultiplier), 0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * rotationSpeed * RotationMultiplier;

            // limit the camera's vertical angle to min/max
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            playerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
        }

        {

            // converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

            // handle grounded movement
            if (isGrounded)
            {
                // calculate the desired velocity from inputs, max speed, and current slope
                Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround;
                // reduce speed if crouching by crouch speed ratio

                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;

                // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);

                // jumping
                if (isGrounded && m_InputHandler.GetJumpInput())
                {
                    characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

                    // then, add the jumpSpeed value upwards
                    characterVelocity += Vector3.up * jumpForce;


                    // remember last time we jumped because we need to prevent snapping to ground for a short time
                    m_LastTimeJumped = Time.time;
                    hasJumpedThisFrame = true;

                    // Force grounding to false
                    isGrounded = false;
                    m_GroundNormal = Vector3.up;
                }
            }
            // handle air movement
            else
            {
                // add air acceleration
                characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;

                // limit air speed to a maximum, but only horizontally
                float verticalVelocity = characterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir);
                characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // apply the gravity to the velocity
                characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
            }

            Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(m_Controller.height);
            m_Controller.Move(characterVelocity * Time.deltaTime);
        }
    }

    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= m_Controller.slopeLimit;
    }

    // Gets the center point of the bottom hemisphere of the character controller capsule    
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * m_Controller.radius);
    }

    // Gets the center point of the top hemisphere of the character controller capsule    
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - m_Controller.radius));
    }

    // Gets a reoriented direction that is tangent to a given slope
    Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }
}
