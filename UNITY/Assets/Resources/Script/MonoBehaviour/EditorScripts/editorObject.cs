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

    public bool removeColliderOnStart = false;

    private void Start()
    {
        if (removeColliderOnStart && GetComponent<BoxCollider>())
            Destroy(GetComponent<BoxCollider>());
    }
}
