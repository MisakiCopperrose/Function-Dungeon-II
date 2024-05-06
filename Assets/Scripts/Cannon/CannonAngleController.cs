using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAngleController : MonoBehaviour
{
    [SerializeField]
    private GameObject _barrelObject;
    [SerializeField]
    private float a;

    public void Rotate()
    {
        _barrelObject.transform.rotation = Quaternion.Euler(0f, 0f, GetAngle(a));
    }

    private float GetAngle(float a)
    {
        float x = 1;
        return Mathf.Atan2(a * x, x) * Mathf.Rad2Deg;
    }
}
