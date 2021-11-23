using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private Transform _barrel;

    [SerializeField]
    private float _maxDistance = 1.0f;

    [SerializeField]
    private int _bulletPerTime = 4;

    [SerializeField]
    private float _timeBetweenBullets = 0.1f;

    [SerializeField]
    private int _bulletPoolAmount = 12;

    [SerializeField]
    private float _bulletSpeed = 1.0f;

    [SerializeField]
    private Transform _bulletsContainer;

    #endregion


    private List<GameObject> _bulletPool = new List<GameObject>();
    private List<GameObject> _activeBullets = new List<GameObject>();



    #region MonoBehaviour

    private void OnEnable()
    {
        for (int i = 0; i < _bulletPoolAmount; i++)
        {
            var bullet = GameObject.Instantiate(_bulletPrefab);
            bullet.SetActive(false);
            _bulletPool.Add(bullet);
            bullet.transform.parent = _bulletsContainer;
        }
        _activeBullets = new List<GameObject>();
    }

    #endregion

    #region Methods
    public void Attack()
    {
        for (int i = 0; i < _bulletPerTime; i++)
        {
            var bullet = GetFreeElementFromPool();

            if (bullet is null)
            {
                Debug.Log("Not enough amo");
                return;
            }

            _activeBullets.Add(bullet);
        }

        StartCoroutine("Shoot");
    }

    private GameObject GetFreeElementFromPool()
    {
        foreach (var k in _bulletPool.Except(_activeBullets))
        {
            return k;
        }

        return null;
    }

    private void ReturnElementToPool(GameObject returnedElement)
    {
        if (!_activeBullets.Contains(returnedElement))
            return;

        returnedElement.GetComponent<Rigidbody>().velocity = Vector3.zero;
        returnedElement.SetActive(false);

        _activeBullets.Remove(returnedElement);

    }

    private IEnumerator Shoot()
    {
        if (_activeBullets.Count != _bulletPerTime)
            yield return null;

        var copyActiveBullets = new List<GameObject>(_activeBullets);

        for (int i = 0; i < _bulletPerTime; i++)
        {
            copyActiveBullets[i].SetActive(true);
            copyActiveBullets[i].transform.position = _barrel.position;
            copyActiveBullets[i].GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;
            StartCoroutine("Reshoot", copyActiveBullets[i]);
            yield return new WaitForSeconds(_timeBetweenBullets);
        }
    }

    private IEnumerator Reshoot(GameObject returnedElement)
    {
        yield return new WaitForSeconds(_maxDistance / _bulletSpeed);
        ReturnElementToPool(returnedElement);
    }

    #endregion
}
