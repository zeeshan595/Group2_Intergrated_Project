using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinemaEffect : MonoBehaviour
{
    public Texture whiteBlock;
    public Texture blackBlock;
    public float effectWidth;
    public float effectBorder;
    public float speed = 5;
    private List<float> effect;

    private int resolutionSetTo;

    private void Start()
    {
        resolutionSetTo = Screen.currentResolution.height;
        effect = new List<float>();
        for (int x = 0; x < (Mathf.RoundToInt(resolutionSetTo / 100)); x++)
            effect.Add(x * 100);
    }

    private void Update()
    {
        for (int x = 0; x < effect.Count; x++)
        {
            effect[x] += Time.deltaTime * speed;

            if (effect[x] > Screen.height)
                effect[x] = 0;
        }

        if (resolutionSetTo != Screen.currentResolution.height)
        {
            resolutionSetTo = Screen.currentResolution.height;
            effect.Clear();
            for (int x = 0; x < (Mathf.RoundToInt(resolutionSetTo / 100) + 1); x++)
                effect.Add(x * 100);
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(effectBorder, 0, effectWidth, Screen.height), whiteBlock);
        GUI.DrawTexture(new Rect(0, 0, effectBorder, Screen.height), blackBlock);
        GUI.DrawTexture(new Rect(effectWidth + effectBorder, 0, effectBorder, Screen.height), blackBlock);

        for (int x = 0; x < effect.Count; x++)
            GUI.DrawTexture(new Rect(effectBorder, effect[x], effectWidth, effectBorder), blackBlock);

        GUI.DrawTexture(new Rect(Screen.width - effectWidth - effectBorder, 0, effectWidth, Screen.height), whiteBlock);
        GUI.DrawTexture(new Rect(Screen.width - effectBorder, 0, effectBorder, Screen.height), blackBlock);
        GUI.DrawTexture(new Rect(Screen.width - effectWidth - effectBorder * 2, 0, effectBorder, Screen.height), blackBlock);

        for (int x = 0; x < effect.Count; x++)
            GUI.DrawTexture(new Rect(Screen.width - effectWidth - effectBorder, effect[x], effectWidth, effectBorder), blackBlock);
    }
}