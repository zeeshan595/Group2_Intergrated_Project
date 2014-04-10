using UnityEngine;
using System.Collections;

public class editorObject : MonoBehaviour
{
    public bool position = false;
    public bool zAxies = false;
    public bool rotation = false;
    public bool size = false;
    public bool yZSizeSwap = false;
    public bool ratioScale = true;
    public bool backOnReset = false;

    public bool removeColliderOnStart = false;
    [System.NonSerialized]
    public Vector3 originalPosition = Vector3.zero;

    private void Start()
    {
        originalPosition = transform.position;
        if (removeColliderOnStart && GetComponent<BoxCollider>())
            Destroy(GetComponent<BoxCollider>());
    }
}
