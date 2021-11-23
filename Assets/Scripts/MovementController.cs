using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private FixedJoystick _joystick;

    [SerializeField]
    private SpriteRenderer _playerSprite;

    [SerializeField]
    private float _speed = 1.0f;

    [SerializeField]
    private float _range = 1.0f;

    #endregion

    #region MonoBehaviour

    public void FixedUpdate()
    {
        if (Mathf.Abs(_joystick.Horizontal) > 0 || Mathf.Abs(_joystick.Vertical) > 0)
        {
            _playerSprite.gameObject.SetActive(true);

            _playerSprite.transform.position = new Vector3(_joystick.Horizontal * _range + transform.position.x, _playerSprite.transform.position.y, _joystick.Vertical * _range + transform.position.z);

            transform.LookAt(new Vector3(_playerSprite.transform.position.x, transform.position.y, _playerSprite.transform.position.z));

            var distanceBetweenPlayerAndSprite = (_playerSprite.transform.position - transform.position).magnitude;

            transform.Translate(Vector3.forward * Time.fixedDeltaTime * _speed * distanceBetweenPlayerAndSprite);
        }
        else
        {
            _playerSprite.gameObject.SetActive(false);
        }
    }

    #endregion
}
