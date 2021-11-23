using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothTime = 0.3F;

    [SerializeField]
    private float distanceToTarget = -10.0f;

    private float _currentVelocity = 0;
    private float _startXPos;
    private float _startYPos;

    void Awake()
    {
        _startXPos = transform.position.x;
        _startYPos = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float targetZPos = target.transform.position.z + distanceToTarget;
        float currentZpos = Mathf.SmoothDamp(transform.position.z, targetZPos, ref _currentVelocity, smoothTime);

        transform.position = new Vector3(_startXPos, _startYPos, currentZpos);
    }
}
