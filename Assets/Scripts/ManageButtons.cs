using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtons : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    public void StartWordGame()
    {
        SceneManager.LoadScene("wordGame");
    }

    public void Restart()
    {
        SceneManager.LoadScene("wordGameStart");
    }

}