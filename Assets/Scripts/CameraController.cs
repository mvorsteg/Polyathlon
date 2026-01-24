using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    const float defaultXMin = -40f;
    const float defaultXMax = 60f;

    private Transform characterHips;
    public Transform cameraTransform;
    public LayerMask layerMask; // probably want to ignore other players and lasers.

    private Quaternion cameraRotation;
    private Vector3 cameraOffset;
    [SerializeField]
    private float zoomedOutFOV = 75f;
    private float zoomTime = 0.25f;
    [SerializeField] 
    private new Camera camera;

    private int invert = -1;
    private float sensitivity = 0.10f;

    private float xMin;
    private float xMax;

    private bool following = false;
    private float defaultFOV;

    void Start()
    {
        cameraRotation = transform.localRotation;
        cameraOffset = cameraTransform.localPosition;
        Racer player = GetComponentInParent<Racer>();
        if (player != null)
        {
            characterHips = player.GetHips();
        }
        defaultFOV = camera.fieldOfView;
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
        if (Physics.Linecast(transform.position, transform.position + transform.localRotation * cameraOffset, out hit, layerMask))
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
    public IEnumerator FollowTransform(Transform target, float verticalOffset = 0f)
    {
        if (target == null)
            target = characterHips;

        if (following)
        {

            following = false;
            yield break;
        }
        following = true;
        while (following)
        {
            
            transform.position = target.position + new Vector3(0, verticalOffset, 0);

            yield return null;
        }
        transform.localPosition = new Vector3(0, 1.5f, 0);
    }

    public void SetZoom(bool zoomOut)
    {
        StartCoroutine(ZoomCoroutine(zoomOut));
    }

    private IEnumerator ZoomCoroutine(bool zoomOut)
    {
        float startFOV = camera.fieldOfView;
        float endFOV = zoomOut ? zoomedOutFOV : defaultFOV;

        float elapsedTime = 0f;
        while (elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;
            camera.fieldOfView = Mathf.Lerp(startFOV, endFOV, elapsedTime / zoomTime);
            yield return null;
        }

        camera.fieldOfView = endFOV;
    }
}
