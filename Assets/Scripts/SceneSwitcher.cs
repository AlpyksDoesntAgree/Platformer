using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string nameScene;
    private bool pressed = false;
    void Start()
    {

    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(gameObject.tag)
        {
            case "pipe":
                if (Input.GetKeyDown(KeyCode.W) && !pressed)
                {
                    pressed = true;
                    PlayerPrefs.SetString("NameScene", nameScene);
                    SceneManager.LoadScene(nameScene);
                }
                break;
        }
    }
}
