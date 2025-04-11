using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwingAxe : MonoBehaviour
{
    private PlayerController controller;
    private Transform trap;

    private bool isRight = true;

    private float timeCount = 0f;

    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        trap = GetComponent<Transform>();
    }

    void Update()
    {
        if (isRight)
        {
            trap.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 75), Quaternion.Euler(0, 0, -75), timeCount);
            timeCount = timeCount + Time.deltaTime;
            if(trap.rotation == Quaternion.Euler(0, 0, -75))
            { isRight = false; timeCount = 0f; }
        }
        else {
            trap.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, -75), Quaternion.Euler(0, 0, 75), timeCount);
            timeCount = timeCount + Time.deltaTime;
            if (trap.rotation == Quaternion.Euler(0, 0, 75))
            { isRight = true; timeCount = 0f; }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controller.Damage(1);
        }
    }
}
