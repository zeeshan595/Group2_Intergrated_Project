using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<BoxCollider>())
            GetComponent<BoxCollider>().enabled = false;
    }
}