using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(InputData))]

public class Sticky : MonoBehaviour
{

    public float _trigger;

    public Collider[] sticky_arr;
    public Collider sticky_collider;
    public float speed = 20f;
    private InputData _inputData;

    private Vector2 _leftAxis;
    private Vector2 _rightAxis;
    private float oldDistance = 9999;

    //spherecast parameters
    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;
    public GameObject currentHitObject;
    private Vector3 direction;
    private Vector3 origin;
    private float currentHitDistance;

    //lerp parameters
    private float _current, _target;
    private float lerpSpeed = 1f;

    //quaternion conversions
    private Vector3 rotationGoal;
    private Vector3 rotationOrigin;

    void Start()
    {
        _inputData = GetComponent<InputData>();
        rotationOrigin = transform.eulerAngles;
    }

    private Collider stickTo()
    {
        origin = transform.position;
        direction = transform.up;
        RaycastHit spherehit;
        if (Physics.SphereCast(origin, sphereRadius, direction, out spherehit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = spherehit.transform.gameObject;
            currentHitDistance = spherehit.distance;
            sticky_collider = spherehit.collider;
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }
        return sticky_collider;
    }



    void CustomMovement()
    {
        var stuck = false;

        Vector3 new_p = transform.position;



        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 _rightAxis))
        {
            _rightAxis = new Vector2(_rightAxis.x, _rightAxis.y);
            transform.Rotate(transform.forward, _rightAxis.x * 5.0f, Space.World);
        }

        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.trigger, out float _triggerQuant))
        {
            _trigger = _triggerQuant;
        }

        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 _leftAxis) && _trigger == 0)
        {
            _leftAxis = new Vector2(_leftAxis.x, _leftAxis.y);
            transform.position += transform.up * _leftAxis.y * speed * Time.deltaTime;
        }

        sticky_collider = stickTo();
        
        Vector3 cp = sticky_collider.ClosestPoint(new_p);

        Ray r = new Ray(new_p, cp - new_p);

        if (sticky_collider.Raycast(r, out var hit, Mathf.Infinity))
        {
            if (stuck == false)
            {
                transform.position = cp;
                Quaternion OriginalRot = transform.rotation;
                rotationGoal = Vector3.Lerp(new_p, cp + hit.normal, 10f * Time.deltaTime);
                transform.LookAt(rotationGoal, transform.up);
                Quaternion NewRot = transform.rotation;
                transform.rotation = OriginalRot;
                transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, speed * Time.deltaTime);
                stuck = true;
            }

            else if (stuck == true)
            {
                transform.position = cp;
                Quaternion OriginalRot = transform.rotation;
                rotationGoal = Vector3.Lerp(new_p, cp + hit.normal, 10f * Time.deltaTime);
                transform.LookAt(rotationGoal, transform.up);
                Quaternion NewRot = transform.rotation;
                transform.rotation = OriginalRot;
                transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, speed * Time.deltaTime);
                stuck = false;
            }
        }
    }

    void Update()
    {
        stickTo();
        CustomMovement();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
