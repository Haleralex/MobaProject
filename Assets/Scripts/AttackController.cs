using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private FixedJoystick _joystick;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private Gun _gun;

    [SerializeField]
    private float _range = 1.0f;

    [SerializeField]
    private float TrailDistance = 1.0f;

    [SerializeField]
    private LayerMask layerMask;

    #endregion

    private Vector3 LookAtPoint;

    #region MonoBehaviour
    private void OnEnable()
    {
        _joystick.PointerUpped += CheckConditionBeforeAttack;
    }
    private void OnDisable()
    {
        _joystick.PointerUpped -= CheckConditionBeforeAttack;
    }

    public void FixedUpdate()
    {
        if (Mathf.Abs(_joystick.Horizontal) > 0 || Mathf.Abs(_joystick.Vertical) > 0)
        {
            _lineRenderer.gameObject.SetActive(true);

            LookAtPoint = new Vector3(_joystick.Horizontal * _range + transform.position.x, _lineRenderer.transform.position.y, _joystick.Vertical * _range + transform.position.z);

            transform.LookAt(new Vector3(LookAtPoint.x, transform.position.y, LookAtPoint.z));

            _lineRenderer.SetPosition(0, transform.position);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, TrailDistance, layerMask))
            {
                _lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                _lineRenderer.SetPosition(1, transform.position + transform.forward * TrailDistance);
                _lineRenderer.SetPosition(1, new Vector3(_lineRenderer.GetPosition(1).x, _lineRenderer.transform.position.y, _lineRenderer.GetPosition(1).z));
            }
        }
        else
        {
            _lineRenderer.gameObject.SetActive(false);
        }
    }

    #endregion

    public void CheckConditionBeforeAttack()
    {
        if (Mathf.Pow(_joystick.Horizontal, 2) + Mathf.Pow(_joystick.Vertical, 2) > 0.9f)
        {
            _gun.Attack();
        }
    }
}
