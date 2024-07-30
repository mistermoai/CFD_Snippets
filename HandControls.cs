using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(InputData))]
public class HandControls : MonoBehaviour
{
    public GameObject leftHandController;
    public GameObject rightHandController;
    public GameObject XRRig;

    private InputData _inputData;

    private void Start()
    {
        _inputData = GetComponent<InputData>();
    }

    
    void Update()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _leftPos))
        {
            var headsetPos = XRRig.transform.position;
            _leftPos = new Vector3(_leftPos.x, _leftPos.y, _leftPos.z);
            leftHandController.transform.position = headsetPos + _leftPos;

        }

        

        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _rightPos))
        {
            var headsetPos = XRRig.transform.position;
            _rightPos = new Vector3(_rightPos.x, _rightPos.y, _rightPos.z);
            rightHandController.transform.position = headsetPos + _rightPos;
        }
    }
}
