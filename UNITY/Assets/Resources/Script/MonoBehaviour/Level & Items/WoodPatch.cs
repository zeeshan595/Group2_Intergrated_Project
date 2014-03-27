using UnityEngine;
using System.Collections;

public class WoodPatch : MonoBehaviour
{
    private void Start()
    {
        Destroy(GetComponent<BoxCollider>());
    }
}