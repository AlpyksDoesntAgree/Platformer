using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.005f;
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 offset;
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, -10) + offset;
            Vector3 smoothedPositin = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPositin;
        }
    }
}
