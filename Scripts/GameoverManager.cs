using UnityEngine;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviour
{
    public GameObject gameoverGUI;
    public Text wavesSurvived;
    static GameObject sGameoverGUI;
    static Text sWavesSurvived;

    void Start()
    {
        sGameoverGUI = gameoverGUI;
        sWavesSurvived = wavesSurvived;
    }

    public static void Gameover()
    {
        MenuManager.gameover = true;
        Time.timeScale = 0f;
        sWavesSurvived.text = "Waves survived: " + WaveSpawner.waveIndex;
        sGameoverGUI.SetActive(true);
    }
}