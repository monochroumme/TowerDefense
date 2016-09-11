using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static int health = 10;
    public int defaultHealth;
    public Text healthDisplay;
    public Image healthBar;

    void Start()
    {
        health = defaultHealth;
    }
    
    void Update()
    {
        healthDisplay.text = health.ToString();
        healthBar.rectTransform.localScale = new Vector3(health * .1f, healthBar.rectTransform.localScale.y, healthBar.rectTransform.localScale.z);

        if(health <= 0)
        {
            GameoverManager.Gameover();
        }
    }
}