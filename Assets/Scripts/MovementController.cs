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

    private InputActions test;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        test = new InputActions();
    }

    private void OnEnable()
    {
        test.Enable();
    }
    private void OnDisable()
    {
        test.Disable();
    }

    public void FixedUpdate()
    {
        //var direction = test.ActionMap.Move.ReadValue<Vector2>(); //пока не надо

        Move(new Vector2(_joystick.Horizontal, _joystick.Vertical));
    }

    private void Move(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > 0 || Mathf.Abs(direction.y) > 0)
        {
            _playerSprite.gameObject.SetActive(true);

            _playerSprite.transform.position = new Vector3(direction.x * _range + transform.position.x, _playerSprite.transform.position.y, direction.y * _range + transform.position.z);

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
