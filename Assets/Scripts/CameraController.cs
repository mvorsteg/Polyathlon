using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    const float xMin = 0f;
    const float xMax = 60f;
    const float aimMin = -90f;
    const float aimMax = 90f;

    public Transform player;
    public Transform cameraTransform;
    public Transform characterMesh;
    private Animator anim;

    private Quaternion cameraRotation;
    private Vector3 cameraOffset;

    private int invert = -1;
    private float sensitivity = 0.10f;

    void Start()
    {
        cameraRotation = transform.localRotation;
        cameraOffset = cameraTransform.localPosition;
        characterMesh = player.GetChild(0);
        anim = transform.parent.GetChild(0).GetComponent<Animator>();
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
}
