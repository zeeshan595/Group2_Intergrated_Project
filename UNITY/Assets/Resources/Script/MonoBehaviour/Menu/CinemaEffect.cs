using UnityEngine;
using System.Collections;

public class CinemaEffect : MonoBehaviour
{
    public Texture whiteBlock;
    public Texture blackBlock;
    public float effectWidth;
    public float effectBorder;
    public float speed = 5;
    private float[] effect;

    private void Start()
    {
        effect = new float[Mathf.RoundToInt(Screen.height / 100)];
        for (int x = 0; x < effect.Length; x++)
            effect[x] = x * 100;
    }

    private void Update()
    {
        for (int x = 0; x < effect.Length; x++)
        {
            effect[x] += Time.deltaTime * speed;

            if (effect[x] > Screen.height)
                effect[x] = 0;
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(effectBorder, 0, effectWidth, Screen.height), whiteBlock);
        GUI.DrawTexture(new Rect(0, 0, effectBorder, Screen.height), blackBlock);
        GUI.DrawTexture(new Rect(effectWidth + effectBorder, 0, effectBorder, Screen.height), blackBlock);

        for (int x = 0; x < effect.Length; x++)
            GUI.DrawTexture(new Rect(effectBorder, effect[x], effectWidth, effectBorder), blackBlock);

        GUI.DrawTexture(new Rect(Screen.width - effectWidth, 0, effectWidth, Screen.height), whiteBlock);
        GUI.DrawTexture(new Rect(Screen.width - effectBorder, 0, effectBorder, Screen.height), blackBlock);
        GUI.DrawTexture(new Rect(Screen.width - effectWidth - effectBorder, 0, effectBorder, Screen.height), blackBlock);

        for (int x = 0; x < effect.Length; x++)
            GUI.DrawTexture(new Rect(Screen.width - effectWidth, effect[x], effectWidth, effectBorder), blackBlock);
    }
}