using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    const float defaultXMin = -40f;
    const float defaultXMax = 60f;

    public Transform player;
    public Transform cameraTransform;
    public Transform characterHips;

    private Quaternion cameraRotation;
    private Vector3 cameraOffset;

    private int invert = -1;
    private float sensitivity = 0.10f;

    private float xMin;
    private float xMax;

    private bool following = false;

    void Start()
    {
        cameraRotation = transform.localRotation;
        cameraOffset = cameraTransform.localPosition;
        characterHips = player.GetComponent<Racer>().GetHips();
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
        if (!following && Physics.Linecast(transform.position, transform.position + transform.localRotation * cameraOffset, out hit))
        {
            cameraTransform.localPosition = new Vector3(0, 0, -Vector3.Distance(transform.position, hit.point) + 0.2f);
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

    // Start this to have the camera follow the character when they ragdoll
    // Start this again to stop following
    public IEnumerator FollowRagdoll()
    {
        //Debug.Log("FollowRagdoll");
        if (following)
        {
            //Debug.Log("Stop Following!");
            following = false;
            yield break;
        }
        following = true;
        while (following)
        {
            
            transform.position = characterHips.position;
            //Debug.Log("our pos: " + transform.position);
            //Debug.Log("character pos: " + characterHips.position);
            yield return null;
        }
        //Debug.Log("End!");
        transform.localPosition = new Vector3(0, 1.5f, 0);
    }
}
