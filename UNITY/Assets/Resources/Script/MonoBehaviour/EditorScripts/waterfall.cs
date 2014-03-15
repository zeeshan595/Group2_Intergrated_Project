using UnityEngine;
using System.Collections;

public class waterfall : MonoBehaviour
{
	void Start ()
    {
        Destroy(GetComponent<BoxCollider>());
        Destroy(transform.GetChild(0).gameObject);
	}
}
