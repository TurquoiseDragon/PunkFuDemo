using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookBehavior : MonoBehaviour
{
    [Header("Pre-Required for scripts")]
    [SerializeField] Transform orientation;
    [Tooltip("Place the orient's transform into here")]
    [SerializeField] Transform cameraPosition;
    [Tooltip("Place the CameraPos transform into here")]


    [Header("Mouse Sensitivity")]
    [SerializeField] public float mouseSensitivityX = 100f;
    [SerializeField] public float mouseSensitivityY = 100f;

    float xRotation = 0f;
    float yRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //applies the mouse sensitivity
        float MouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float MouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        xRotation -= MouseY;
        yRotation += MouseX;

        //provents the player from looking too far up or down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //rotates the player camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        //moves the camera to the CameraPos object in the scene
        transform.position = cameraPosition.position;
    }
}
