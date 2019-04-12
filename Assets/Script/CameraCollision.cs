using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public float minDistance = 1.0f;
    public float maxDistance = 5.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    // Use this for initialization
    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        // (optional) put layers you don't want to collide with  here (probably things like enemies)
        const int ignoreLayer1 = 1 ;
        const int ignoreLayer2 = 2;
        int layerMask = ignoreLayer1 | ignoreLayer2;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, layerMask))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        transform.localPosition = dollyDir * distance;
    }
}
