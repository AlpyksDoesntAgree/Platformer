using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    void Start()
    {
        
    }

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

}
