using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float speed = 2f;

    [SerializeField]
    private Transform unit;

    private void Awake()
    {
        if (!unit) unit = FindObjectOfType<Hero>().transform;
    }

    private void Update()
    {
        Vector3 position = unit.position;
        position.z = -10f;
        transform.position = Vector3.Lerp(transform.position,position, speed * Time.deltaTime);
    }
}
