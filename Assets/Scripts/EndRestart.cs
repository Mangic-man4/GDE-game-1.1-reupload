using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRestart : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Level");
    }
}
