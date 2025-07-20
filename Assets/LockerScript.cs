using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockerScript : MonoBehaviour
{
    public GameObject anchorPoint;
    public GameObject door;
    public bool canRotate = false;
    private float rotationSpeed = 10f;
    private float lastRotationY = 0f;
    private float currentRotationY = 0f;
    private float lastInput = 0f;
    private bool letGoOffDoor = false;
    void Start()
    {
        lastRotationY = anchorPoint.transform.localEulerAngles.y;
    }
    public void SetRoationState(bool rotationState)
    {
        canRotate = rotationState;
    }

    void Update()
    {
        if (canRotate || (lastRotationY != currentRotationY))
        {
            RotateDoor();
        }
    }

    void RotateDoor()
    {
        float formattedInput = 0f;

        if (canRotate)
        {
            float mouseY = Mouse.current.delta.ReadValue().y;
            formattedInput = -mouseY;
            lastInput = formattedInput;
        }
        else if (!canRotate && !letGoOffDoor)
        {
            letGoOffDoor = true;
            StartCoroutine(RotatioSpeedReducing(0.1f, 0.1f));
        }
        else if (!canRotate)
        {
            formattedInput = lastInput;
        }

        float angle = formattedInput * rotationSpeed * Time.deltaTime;
        currentRotationY = anchorPoint.transform.localEulerAngles.y;
        float targetY = currentRotationY + angle;

        targetY = Mathf.Repeat(targetY, 360);

        if (targetY >= 0 && targetY <= 160)
        {
            anchorPoint.transform.localRotation = Quaternion.Euler(0, targetY, 0);
        }

    }

    IEnumerator RotatioSpeedReducing(float speedSubstracted, float speed) {
        float iterationDelay = speed;

        while (rotationSpeed > 0 && !canRotate)
        {
            yield return new WaitForSeconds(iterationDelay);
            if (rotationSpeed - speedSubstracted <= 0f)
            {
                rotationSpeed = 0;
            }
            else
            {
                rotationSpeed -= speedSubstracted;
            }

        }   
        lastRotationY = currentRotationY;
        letGoOffDoor = false;
        rotationSpeed = 10;
    }
    
}
