using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    const float defaultXMin = -40f;
    const float defaultXMax = 60f;

    public Transform player;
    public Transform cameraTransform;
    public Transform characterMesh;

    private Quaternion cameraRotation;
    private Vector3 cameraOffset;

    private int invert = -1;
    private float sensitivity = 0.10f;

    private float xMin;
    private float xMax;

    void Start()
    {
        cameraRotation = transform.localRotation;
        cameraOffset = cameraTransform.localPosition;
        characterMesh = player.GetChild(0);
        ResetXMinMax();
    }
 
    /*  rotates the camera with the given rotation values */
    public void Rotate(float xRot, float yRot)
    {
        cameraRotation.x += yRot * sensitivity * invert;
        cameraRotation.y += xRot * sensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, xMin, xMax);
        transform.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, cameraRotation.z);
        
        RaycastHit hit;
        if (Physics.Linecast(transform.position, transform.position + transform.localRotation * cameraOffset, out hit))
        {
            cameraTransform.localPosition = new Vector3(0, 0, -Vector3.Distance(transform.position, hit.point));
        }
        else
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraOffset, Time.deltaTime);
        }
    }

    // Specify a new xMin and xMax to use for camera movement allowances
    public void SetXMinMax(float newXMin, float newXMax)
    {
        xMin = newXMin;
        xMax = newXMax;
    }

    // Reset xMin and xMax to the defaults use for camera movement allowances
    public void ResetXMinMax()
    {
        xMin = defaultXMin;
        xMax = defaultXMax;
    }
}
