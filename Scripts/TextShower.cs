using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextShower : MonoBehaviour
{
    public Text error;
    public float fadingDuration;
    public static TextShower instance;
    bool fade = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one TextShower in scene!");
            return;
        }
        instance = this;
    }

    public void Show(string text)
    {
        error.text = text;
        error.CrossFadeColor(Color.red, 0f, true, true);
        Invoke("Fade", 1f);
    }

    void Fade()
    {
        fade = true;
    }

    void Update()
    {
        if (fade && error.canvasRenderer.GetColor() != new Color(1, 0, 0, 0))
            error.CrossFadeAlpha(0f, fadingDuration, false);
        else
            fade = false;
    }
}