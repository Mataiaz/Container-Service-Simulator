using UnityEngine.InputSystem;
using UnityEngine;

public partial class PlayerScript : MonoBehaviour
{
    private int toolEquipped = -1;
    private int lastToolEquipped = -1;
    private bool IsGrounded = true;
    public LayerMask groundLayers;
    public enum GroundType { Grass, Stone, Cement, Void}
    private GroundType? lastGroundType = null;
    private ScannerScript scannerScript;
    private bool isLockedInIntraction = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        scannerScript = GetComponentInChildren<ScannerScript>();
        InitAudio();
    }

    private void FixedUpdate()
    {
        if (isLockedInIntraction) { return; }
        LateHandleInput();
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        CanInteract();
        InteractWithObject(gameObject);

        if (isLockedInIntraction) { return; }
        HandleToolSelection();
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            scannerScript.SetScanPos(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            scannerScript.SetScanPos(false);
        }



    }

    private void LateHandleInput()
    {
        int horizontal = Keyboard.current.dKey.isPressed ? 1 : Keyboard.current.aKey.isPressed ? -1 : 0;
        int vertical = Keyboard.current.wKey.isPressed ? 1 : Keyboard.current.sKey.isPressed ? -1 : 0;
        PlayMovementAudio(horizontal, vertical);
        HandlePlayerMovement(vertical, horizontal);

        if (Keyboard.current.spaceKey.isPressed && IsGrounded)
        {
            Jump();
        }

        HandleCamera();

    }

    void HandleToolSelection()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            toolEquipped = 0;
            if (toolEquipped == lastToolEquipped)
            {
                tools[toolEquipped].gameObject.GetComponent<ScannerScript>().ShowScanner(false);
                toolEquipped = -1;
            }
            else
            {
                tools[toolEquipped].gameObject.GetComponent<ScannerScript>().ShowScanner(true);
            }
            lastToolEquipped = toolEquipped;
        }

    }

    GroundType GetGroundTypeFromLayer(int layer)
{
    switch (layer)
    {
        case 15: return GroundType.Stone;
        case 16: return GroundType.Grass;
        case 17: return GroundType.Cement;
        default: return GroundType.Void;
    }
}

    /*
    * validate what ground layer you standing on and if you are grounded
    */
    void OnTriggerStay(Collider other) {

        if (((1 << other.gameObject.layer) & groundLayers) != 0)
        {
            IsGrounded = true;
            GroundType type = GetGroundTypeFromLayer(other.gameObject.layer);
            
            if (validateFall && IsGrounded)
            {
                validateFall = false;
                SetMovementAudioClip(type, true);
            }
            else if (lastGroundType == null || type != lastGroundType)
            {
                lastGroundType = type;
                SetMovementAudioClip(type, false);

            }
        }
        else
        {
            IsGrounded = false;
        }
    }

}