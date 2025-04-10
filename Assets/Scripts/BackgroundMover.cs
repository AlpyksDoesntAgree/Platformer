using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.005f;
    private Transform player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, 0);
        Vector3 smoothedPositin = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPositin;
    }
}
