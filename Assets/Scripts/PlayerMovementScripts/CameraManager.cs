using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    Transform targetTransform;
    public Transform cameraPivot;
    private float defaultPosition;
    private Transform cameraTransform;
    public LayerMask collisonLayers;
    private float cameraCollisonRadious = 0.2f;
    private Vector3 cameraVectorPostion;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraFollowSpeed = 0.2f;

    public float lookAngel;
    public float pivotAngle;

    public float cameraCollisonOffset = 0.2f; 
    public float cameraLookSpeed = 2f;
    public float cameraPivotSpeed = 2f;

    InputManager inputManager;
    private float minimumCollisonOffset=0.2f;

    private void Awake()
    {
     
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }
    public void Start()
    {
        //targetTransform = FindObjectOfType<PlayerManager>().transform;
        //inputManager = FindObjectOfType<InputManager>();
    }
    public void HandleAllCameraMovements()
    {
        FollowTarget();
        RotateCamera();
        HandleCameracollision();
    }
    private void FollowTarget()
    {
        if(targetTransform == null)
            targetTransform = FindAnyObjectByType<PlayerManager>().transform;
        Vector3 targetPostion = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPostion;
    }
    private void RotateCamera()
    {
        if (inputManager == null)
            inputManager = FindAnyObjectByType<InputManager>();
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngel = lookAngel + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, -35, 35);

        rotation = Vector3.zero;
        rotation.y = lookAngel;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation) ;
        cameraPivot.localRotation = targetRotation; 
    }
    
    private void HandleCameracollision()
    {
        float targetPostion = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraPivot.transform.position,cameraCollisonRadious,direction,out hit,Mathf.Abs(targetPostion),collisonLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPostion = -(distance - cameraCollisonOffset);
        }
        if(Mathf.Abs(targetPostion) < minimumCollisonOffset)
        {
            targetPostion = targetPostion - minimumCollisonOffset;  
           
        }
        cameraVectorPostion.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPostion, 0.2f);
        cameraTransform.localPosition = cameraVectorPostion;
    }
}
