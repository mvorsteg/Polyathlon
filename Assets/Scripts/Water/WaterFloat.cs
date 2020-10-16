using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloat : MonoBehaviour
{

    public float airDrag = 1f;
    public float waterDrag = 10f;
    public bool attachToSurface = true;
    public Transform[] floatPoints;

    private Rigidbody rb;
    private Waves waves;

    private float waterLine;
    private Vector3[] waterLinePoints;

    private Vector3 centerOffset;
    private Vector3 smoothVectorRotation;
    private Vector3 targetUp;

    public Vector3 Center { get { return transform.position + centerOffset; } }

    void Awake()
    {
        waves = FindObjectOfType<Waves>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        waterLinePoints = new Vector3[floatPoints.Length];
        for (int i = 0; i < floatPoints.Length; i++)
        {
            waterLinePoints[i] = floatPoints[i].position;
        }
        centerOffset = GetCenter(waterLinePoints) - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // default waterline
        float newWaterLine = 0f;
        bool pointUnderwater = false;   

        // set waterLinePoints and waterLine
        for (int i = 0; i < floatPoints.Length; i++)
        {
            // height
            waterLinePoints[i] = floatPoints[i].position;
            waterLinePoints[i].y = waves.GetHeight(floatPoints[i].position);
            newWaterLine += waterLinePoints[i].y / floatPoints.Length;
            if (waterLinePoints[i].y > floatPoints[i].position.y)
            {
                pointUnderwater = true;
            }
        }

        float waterLineDelta = newWaterLine - waterLine;
        waterLine = newWaterLine;

        // gravity
        Vector3 gravity = Physics.gravity;
        rb.drag = airDrag;
        if (waterLine > Center.y)
        {
            rb.drag = waterDrag;
            if (attachToSurface)
            {
                // attach to water surface
                rb.position = new Vector3(rb.position.x, waterLine - centerOffset.y, rb.position.z);
            }
            else
            {
                // go up
                gravity = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }
        }
        rb.AddForce(gravity * Mathf.Clamp(Mathf.Abs(waterLine - Center.y), 0, 1));

        // compute up vector
        targetUp = GetNormal(waterLinePoints);

        //rotation
        if (pointUnderwater)
        {
            // attach to water surface
            targetUp = Vector3.SmoothDamp(transform.up, targetUp, ref smoothVectorRotation, 0.2f);
            rb.rotation = Quaternion.FromToRotation(transform.up, targetUp) * rb.rotation;
        }
    }

    public static Vector3 GetCenter(Vector3[] points)
    {
        Vector3 center = Vector3.zero;
        for (int i = 0; i < points.Length; i++)
        {
            center += points[i] / points.Length;
        }
        return center;
    }

    public static Vector3 GetNormal(Vector3[] points)
        {
            //https://www.ilikebigbits.com/2015_03_04_plane_from_points.html
            if (points.Length < 3)
                return Vector3.up;

            var center = GetCenter(points);

            float xx = 0f, xy = 0f, xz = 0f, yy = 0f, yz = 0f, zz = 0f;

            for (int i = 0; i < points.Length; i++)
            {
                var r = points[i] - center;
                xx += r.x * r.x;
                xy += r.x * r.y;
                xz += r.x * r.z;
                yy += r.y * r.y;
                yz += r.y * r.z;
                zz += r.z * r.z;
            }

            var det_x = yy * zz - yz * yz;
            var det_y = xx * zz - xz * xz;
            var det_z = xx * yy - xy * xy;

            if (det_x > det_y && det_x > det_z)
                return new Vector3(det_x, xz * yz - xy * zz, xy * yz - xz * yy).normalized;
            if (det_y > det_z)
                return new Vector3(xz * yz - xy * zz, det_y, xy * xz - yz * xx).normalized;
            else
                return new Vector3(xy * yz - xz * yy, xy * xz - yz * xx, det_z).normalized;

        }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (floatPoints == null)
            return;

        for (int i = 0; i < floatPoints.Length; i++)
        {
            if (floatPoints[i] == null)
                continue;

            if (waves != null)
            {

                //draw cube
                Gizmos.color = Color.red;
                Gizmos.DrawCube(waterLinePoints[i], Vector3.one * 0.3f);
            }

            //draw sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(floatPoints[i].position, 0.1f);

        }

        //draw center
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(Center.x, waterLine, Center.z), Vector3.one * 1f);
        }
    }
}
