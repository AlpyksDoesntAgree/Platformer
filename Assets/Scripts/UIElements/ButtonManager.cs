using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void ExitGame()
    {
        Application.Quit();
    }

    public void QuitMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PlayerPrefs.DeleteAll();
    }
    public void Restart()
    {
        SceneManager.LoadScene("StarterLvl");
        PlayerPrefs.DeleteAll();
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void OpenChatAnimation()
    {
        animator.SetFloat("Mult", 1f);
        animator.Play("OpenChat");
    }
    public void CloseChatAnimation()
    {
        animator.SetFloat("Mult", -1f);
        animator.Play("OpenChat");
    }
}
