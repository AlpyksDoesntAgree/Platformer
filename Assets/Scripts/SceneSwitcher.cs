using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    //Player Components
    private Transform player;
    private PlayerController playerCon;
    private Rigidbody2D playerRb;
    //Pipe Components
    [SerializeField] private string nameScene;
    private Transform endPos;
    [SerializeField]private float _speed;
    private bool pressed = false;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerCon = GameObject.Find("Player").GetComponent<PlayerController>();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        endPos = GetComponent<Transform>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "pipe":
                    if (Input.GetKeyDown(KeyCode.Space) && !pressed)
                    {
                        StartCoroutine(SlideIntoPipe());
                    }
                    break;
                case "door":
                    if (Input.GetKeyDown(KeyCode.Space) && !pressed)
                    {
                        SceneManager.LoadScene(nameScene);
                    }
                    break;
            }
        }
    }

    IEnumerator SlideIntoPipe()
    {
        pressed = true;
        PlayerPrefs.SetString("NameScene", nameScene);
        playerCon.isMovingEnabled = false;
        playerRb.bodyType = RigidbodyType2D.Kinematic;

        while (Vector3.Distance(player.position, endPos.position) > 1f)
        {
            player.position = Vector3.Lerp(player.position, endPos.position, _speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        playerCon.isMovingEnabled=true;
        SceneManager.LoadScene(nameScene);
    }
}
