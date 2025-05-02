using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPlatform : MonoBehaviour
{
    private LastWinMenu lastWinMenu;
    void Start()
    {
        lastWinMenu = GameObject.Find("WinPanel").GetComponent<LastWinMenu>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if(collision.gameObject.CompareTag("Player"))
            lastWinMenu.WinGame();
    }
}
