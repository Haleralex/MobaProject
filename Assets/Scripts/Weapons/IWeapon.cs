using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    float Damage { get; set; }
    void PerformAttack();
    void Reload();
}
