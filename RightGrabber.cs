using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(InputData))]
public class RightGrabber : MonoBehaviour
{
    public GameObject grabber;

    //object to control
    public GameObject lastHit;

    //controller position and orientation
    public Vector3 _controllerPos;
    public Vector3 _controllerVelocity;
    public float _trigger;
    public Vector2 _controllerRotation;

    //spherecast parameters
    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;
    public GameObject currentHitObject;
    private Vector3 direction;
    private Vector3 origin;
    private float currentHitDistance;
    private GameObject planetoid;
    public Vector3 contactPoint;

    private InputData _inputData;

    //lerp
    private float start;
    private float des;
    private float fraction = 0;
    private float speed = 10f;

    //grabber var
    private Vector3 originalGrabberPos;

    private void Start()
    {
        _inputData = GetComponent<InputData>();
        start = 0;
        des = 1.5f;

    }

    private GameObject StickTo()
    {
        origin = transform.position;
        direction = transform.forward;
        RaycastHit spherehit;
        if (Physics.SphereCast(origin, sphereRadius, direction, out spherehit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = spherehit.transform.gameObject;
            currentHitDistance = spherehit.distance;
            planetoid = spherehit.collider.gameObject;
            contactPoint = spherehit.point;
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }
        return planetoid;
    }

    private void getControllerData()
    {
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _controllerPosition))
        {
            _controllerPos = _controllerPosition;
        }

        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.trigger, out float _triggerQuant))
        {
            _trigger = _triggerQuant;
        }

        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 _velocity))
        {
            _controllerVelocity = _velocity;
        }

        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 _leftAxis))
        {
            _controllerRotation = _leftAxis;
        }
    }

    void Update()
    {
        getControllerData();
        originalGrabberPos = grabber.transform.position;

        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.trigger, out float _triggerQuant) && _triggerQuant > 0)
        {
            print("pressed");
            grabber.transform.position = contactPoint;
            if (StickTo() != null)
            {
                Debug.Log("hit");
                if (currentHitObject != null)
                {
                    lastHit = currentHitObject.gameObject;
                }
            }

            lastHit.transform.position = lastHit.transform.position + _controllerVelocity;
            lastHit.transform.RotateAround(grabber.transform.position, _controllerRotation.x);

        }
        else if (_triggerQuant == 0)
        {
            print("left button up");
            grabber.transform.position = originalGrabberPos;
            
        }
    }
}
