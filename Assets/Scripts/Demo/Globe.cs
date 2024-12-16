using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GlobeRotationMode { Auto, Manual };

public class Globe : MonoBehaviour
{
    [SerializeField]
    GameObject GlobeSphere;

    [SerializeField]
    Transform SouthPivot;

    [SerializeField]
    Transform NorthPivot;

    [SerializeField]
    float RotationSpeed = 10.0f;

    [SerializeField]
    Texture2D BorderMaskIndia;

    private Vector3 rotationAxis;
    private Vector3 pivotPoint;

    private GlobeRotationMode rotationMode = GlobeRotationMode.Auto;

    private Vector3? lastProjectionPoint = null;
    private bool grabbed = false;

    void Awake()
    {
        rotationAxis = (NorthPivot.position - SouthPivot.position).normalized;
        pivotPoint = SouthPivot.position + (NorthPivot.position - SouthPivot.position)/2;
    }

    void Update()
    {
        if (rotationMode == GlobeRotationMode.Auto)
        {
            GlobeSphere.transform.RotateAround(pivotPoint, rotationAxis, Time.deltaTime * RotationSpeed);
        }

        /*
        if (Input.GetMouseButtonDown(0))
        {
            grabbed = true;
        }
        */

        if (Input.GetMouseButtonUp(0))
        {
            //grabbed = false;
            lastProjectionPoint = null;
        }

        if (Input.GetMouseButton(0))
        {
            CheckCountryHit(Input.mousePosition, BorderMaskIndia);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == GlobeSphere.GetComponent<Collider>())
                {
                    rotationMode = GlobeRotationMode.Manual;

                    Vector3 v = hit.point - pivotPoint;
                    float d = Vector3.Dot(v, rotationAxis);

                    Vector3 projectionPoint = hit.point - d * rotationAxis;

                    if(lastProjectionPoint != null)
                    {
                        var v1 = projectionPoint - pivotPoint;
                        var v2 = lastProjectionPoint.Value - pivotPoint;
                        float angle = Vector3.Angle(v1, v2);

                        Vector3 orthonormal = Vector3.Cross(v1.normalized, rotationAxis);
                        int sign = Vector3.Dot(orthonormal, v2) < 0 ? -1 : 1;

                        GlobeSphere.transform.RotateAround(pivotPoint, rotationAxis, angle*sign);
                    }

                    lastProjectionPoint = projectionPoint;
                }
            }
        }
    }

    public void CheckCountryHit(Vector3 mousePosition, Texture2D borderMask)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == GlobeSphere.GetComponent<Collider>())
            {
                Vector2 textureCoordinate = hit.textureCoord;

                Color maskSample = borderMask.GetPixelBilinear(textureCoordinate[0], textureCoordinate[1]);

                if(maskSample.grayscale >= 0.5)
                {
                    Debug.Log("Country hit!");
                }
                else
                {
                    Debug.Log("Country not hit!");
                }
            }
        }
    }
}
