using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public static bool gameover;

    void Start()
    {
        gameover = false;
    }

    void Update()
    {
        if (!gameover && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        menu.SetActive(!menu.activeInHierarchy);
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}