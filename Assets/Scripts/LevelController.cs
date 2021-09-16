using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private void Update()
    {
        //reload scene
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("Sandbox");
        }

        //Quit application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
