using UnityEngine;
using System.Collections;

public class selectedCharacter : MonoBehaviour
{
    public GameObject[] characters = new GameObject[6];

    private void Update()
    {
        for (int x = 0; x < characters.Length; x++)
        {
            if (x == (int)Settings.carType)
                characters[x].SetActive(true);
            else
                characters[x].SetActive(false);
        }
    }
}