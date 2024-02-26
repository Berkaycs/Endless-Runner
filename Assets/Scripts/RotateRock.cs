using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRock : MonoBehaviour
{
    private float _rotationSpeed = 150f;
    private Vector3 _movementSpeed = new Vector3(0,0,-1);
    private float _speedMultiplier = 0.3f;

    void Update()
    {
        transform.position += _movementSpeed * _speedMultiplier;
        transform.Rotate(Vector3.left, _rotationSpeed * Time.deltaTime);
    }
}
