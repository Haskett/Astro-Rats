using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Triple_Shot : MonoBehaviour
{
    [SerializeField] private float _tripleShotSpeed;
    void Update()
    {
        transform.Translate(Vector3.up * _tripleShotSpeed * Time.deltaTime);
    }
}