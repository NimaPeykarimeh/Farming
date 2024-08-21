using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    Camera mainCamera;
    Transform _transform;
    float sensitivityX;
    float sensitivityY;
    Rigidbody rb;

    [SerializeField] float defaultSensitivityX = 75f;
    [SerializeField] float defaultSensitivityY = 75f;

    float zoomedSensitivityY;
    float zoomedSensitivityX;

    [SerializeField] float yRotation;
    [SerializeField] float xRotation;

    [Header("ZoomSettings")]

    [SerializeField] float zoomedValue = 20f;
    [SerializeField] float defaultValue = 60;
    [SerializeField] float currentValue;
    float targetValue;
    [SerializeField] bool isZooming;
    [SerializeField] bool isZoomed;
    [SerializeField] float zoomDuration = 0.3f;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        zoomedSensitivityX = defaultSensitivityX / (defaultValue / zoomedValue);
        zoomedSensitivityY = defaultSensitivityY / (defaultValue / zoomedValue);
        sensitivityX = defaultSensitivityX;
        sensitivityY = defaultSensitivityY;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ZoomTheCamera(bool _isZoomingIn)
    {
        if (_isZoomingIn && !isZoomed)
        {
            isZooming = true;
            sensitivityY = zoomedSensitivityY;
            sensitivityX = zoomedSensitivityX;
            targetValue = zoomedValue;
        }
        else if(!_isZoomingIn && isZoomed)
        {
            isZooming = true;
            sensitivityX = defaultSensitivityX;
            sensitivityY = defaultSensitivityY;
            targetValue = defaultValue;
        }
        isZoomed = _isZoomingIn;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")  * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation,-90f,90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation,0, 0);
        
        //_transform.Rotate(Vector3.up,mouseX );
        rb.rotation = Quaternion.Euler(0, yRotation, 0);

        //Vector3 _rot = new Vector3(0, mouseX, 0);
        //transform.rotation = Quaternion.Euler(0, yRotation, 0);

        ////ZOOMING
        //if (Input.mouseScrollDelta.y > 0)
        //{
        //    ZoomTheCamera(true);
        //}
        //else if (Input.mouseScrollDelta.y < 0)
        //{
        //    ZoomTheCamera(false);
        //}
        //float _dist = defaultValue - zoomDuration;
        //if (isZooming)
        //{
        //    if (currentValue != targetValue)
        //    {

        //        currentValue = Mathf.MoveTowards(currentValue, targetValue, (1f / zoomDuration) * Time.deltaTime * _dist);
        //        mainCamera.fieldOfView = currentValue;
        //    }
        //    else
        //    {
        //        isZooming = false;
        //    }
        //}
    }
}