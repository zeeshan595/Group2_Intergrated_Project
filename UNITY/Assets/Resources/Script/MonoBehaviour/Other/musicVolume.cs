using UnityEngine;
using System.Collections;

public class musicVolume : MonoBehaviour
{
    private void LateUpdate()
    {
        audio.volume = Settings.MusicVolume;
    }
}