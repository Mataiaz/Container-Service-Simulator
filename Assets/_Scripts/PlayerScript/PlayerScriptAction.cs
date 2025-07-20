using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public partial class PlayerScript : MonoBehaviour
{
  public Transform interactRayPlacement;
  public GameObject[] tools;
  public LayerMask interactableLayers;
  public GameObject characterCamera;
  public TMPro.TMP_Text toolTipText;
  public bool validateFall;
  private Rigidbody rb;
  private Vector3 movementInput;
  private float cameraPitch = 0f;
  private float mouseSensitivity = 1f;
  private LayerMask lastLayaerLookedAt = -1;

  private GameObject interactableObject;

  void HandlePlayerMovement(int vertical, int horizontal)
  {

    Vector3 camForward = characterCamera.transform.forward;
    Vector3 camRight = characterCamera.transform.right;

    camForward.y = 0f;
    camForward.Normalize();

    camRight.y = 0f;
    camRight.Normalize();

    movementInput = (camForward * vertical + camRight * horizontal).normalized;
    rb.MovePosition(transform.position + movementInput * Time.deltaTime * 5f);

  }

  void HandleCamera()
  {
    if (isLockedInIntraction) return;
    transform.Rotate(Vector3.up * Mouse.current.delta.x.ReadValue() * mouseSensitivity);
    cameraPitch -= Mouse.current.delta.y.ReadValue() * mouseSensitivity;
    cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
    characterCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);

  }

  void Jump()
  {
    rb.AddForce(Vector3.up * 4f, ForceMode.Impulse);
    IsGrounded = false;
    validateFall = true;
  }

  public bool GetIsMoving()
  {
    bool isMoving = false;

    if (Keyboard.current.dKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed)
    {
      isMoving = true;
    }

    return isMoving;
  }

  void CanInteract()
  {
    Ray ray = new Ray(interactRayPlacement.position, interactRayPlacement.forward);
    RaycastHit hit;

    Debug.DrawRay(ray.origin, ray.direction * 2.5f, Color.green);

    if (Physics.Raycast(ray, out hit, 2.5f, interactableLayers))
    {
      GenerateToolTipText(hit.collider.gameObject);
      lastLayaerLookedAt = hit.collider.gameObject.layer;
    }
    else if(!isLockedInIntraction)
    {
      toolTipText.text = "";
      lastLayaerLookedAt = -1;
      interactableObject = null;
    }
  }

  /*
  * we generate the tool tip by checking if the layer is one we have listed here by name.
  * if it is the same name we were previously looking at then we don't bother generating a new text
  */
  void GenerateToolTipText(GameObject objectToInteractWith)
  {
    interactableObject = objectToInteractWith;

    if (lastLayaerLookedAt != objectToInteractWith.layer)
    {

      string layerName = LayerMask.LayerToName(objectToInteractWith.layer);
      switch (layerName)
      {
        case "Door":
          toolTipText.text = "[E] Enter"; break;
        case "LockerDoor":
          toolTipText.text = "[MOUSE 0] Open Locker"; break;
        case "Entrance":
          toolTipText.text = "I should finish my shift\nMy manager is already on my ass for the illeagel cock fight i had last saturday.."; break;
        case "ScannerProp":
          toolTipText.text = "[E] Pick Up Scanner"; break;
        default:
          Debug.LogError("Layer " + layerName + " does not exist");
          if (1 == 2) { }
          break;
      }
    }
  }

  void InteractWithObject(GameObject player)
  {
    if (Keyboard.current.eKey.wasPressedThisFrame)
    {
      if (toolTipText.text.Contains("Enter") && interactableObject)
      {
        string teleportingText = interactableObject.GetComponent<TeleporterScript>().TeleportPlayer(player);
        if (teleportingText == "true")
        {
          toolTipText.text = "";
        }
        else if (teleportingText != "false")
        {
          toolTipText.text = teleportingText;
        }
        else
        {
          toolTipText.text = "The door is jammed";
        }
        interactableObject = null;
      }
      else if (toolTipText.text.Contains("Pick Up Scanner") && interactableObject)
      {
        Destroy(interactableObject);
        tools[0].gameObject.GetComponent<ScannerScript>().hasBeenPickedUp = true;

      }
    }
    else if (Mouse.current.leftButton.isPressed && toolEquipped == -1)
    {
      if (toolTipText.text.Contains("Open Locker") && interactableObject)
      {
        isLockedInIntraction = true;
        interactableObject.GetComponent<LockerScript>().canRotate = true;
      }
    }
    else if (Mouse.current.leftButton.wasReleasedThisFrame && isLockedInIntraction)
    {
      if (interactableObject)
      {
        isLockedInIntraction = false;
        interactableObject.GetComponent<LockerScript>().canRotate = false;
        interactableObject = null;
      }
    }
  }

}