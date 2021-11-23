using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private FixedJoystick _joystick;

    [SerializeField]
    private GameObject _attackRangeMark;

    [SerializeField]
    private Gun _gun;

    [SerializeField]
    private float _range = 1.0f;

    [SerializeField]
    private float _trailDistance = 4.0f;

    [SerializeField]
    private LayerMask layerMask;

    #endregion

    private Vector3 _lookAtPoint;

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
            _attackRangeMark.gameObject.SetActive(true);

            _lookAtPoint = new Vector3(_joystick.Horizontal * _range + transform.position.x, _attackRangeMark.transform.position.y, _joystick.Vertical * _range + transform.position.z);

            transform.LookAt(new Vector3(_lookAtPoint.x, transform.position.y, _lookAtPoint.z));

            _attackRangeMark.transform.localScale = new Vector3(1, _trailDistance, 1);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _trailDistance, layerMask))
            {
                var k = (hit.point - transform.position).magnitude;

                _attackRangeMark.transform.localScale = new Vector3(1, k, 1);
            }
            else
            {
                var k = (transform.forward * _trailDistance).magnitude;

                _attackRangeMark.transform.localScale = new Vector3(1, k, 1);
            }
        }
        else
        {
            _attackRangeMark.gameObject.SetActive(false);
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
