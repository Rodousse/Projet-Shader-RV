using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    Texture2D fadeTexture;
    [SerializeField, Range(0, 1)]
    float fadeSpeed = 0.8f;

    float alpha = 1.0f;
    int fadeDir = -1;

    Animator m_anim;

    void Start()
    {
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.black);
        fadeTexture.Apply();

        m_anim = GetComponent<Animator>();
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

    public void Open()
    {
        BeginFade(1);
        m_anim.SetTrigger("Open");
    }

    void BeginFade(int direction)
    {
        fadeDir = direction;
    }

    void EndGame()
    {
        Debug.Log("C'est fini");
        //Application.Quit();
    }
}
