using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// distancia en x del objetivo
    /// </summary>
    public float dstFromTarget = 2;
    /// <summary>
    /// Sensibilidad del mouse
    /// </summary>
    public float mouseSensitivity = 10;
    /// <summary>
    /// Suavisado de la sensibilidad
    /// </summary>
    public float rotationSmoothTime = .1f;
    /// <summary>
    /// Hasta donde baja y sube la camara
    /// </summary>
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    /// <summary>
    /// bloquear mouse
    /// </summary>
    public bool lockCursor;
    /// <summary>
    /// Objetivo a seguir (CameraFollow)
    /// </summary>
    public Transform target;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    // Start is called before the first frame update
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }
}
