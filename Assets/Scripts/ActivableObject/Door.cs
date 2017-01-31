using UnityEngine;
using System.Collections;
using System;

public class Door : IActivable
{
    Texture2D fadeTexture;
    [SerializeField, Range(0, 1)]
    float fadeSpeed = 0.8f;

    float alpha = 1.0f;
    int fadeDir = -1;

    void Start()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.black);
        fadeTexture.Apply();
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        if (alpha >= 1)
            EndGame();

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }

    void BeginFade(int direction)
    {
        fadeDir = direction;
    }

    void EndGame()
    {
        Application.Quit();
    }

    protected override void Refresh()
    {
        BeginFade(1);
        GetComponent<Animator>().SetTrigger("Open");
    }
}
