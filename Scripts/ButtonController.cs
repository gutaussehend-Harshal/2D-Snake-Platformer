using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void OnPlayButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
