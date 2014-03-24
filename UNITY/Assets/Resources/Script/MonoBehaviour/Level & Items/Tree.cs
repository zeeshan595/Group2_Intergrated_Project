using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{
    public GameObject[] leaves;
    public GameObject lantern;

    private Vector3[] noramlPos;

    private float sinWave = 0;

    private void Start()
    {
        noramlPos = new Vector3[leaves.Length];

        if (Random.Range(0, 3) > 1)
            Destroy(lantern);

        for (int x = 0; x < leaves.Length; x++)
        {
            noramlPos[x] = leaves[x].transform.position;
        }

        if (GetComponent<BoxCollider>())
            GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        for (int x = 0; x < leaves.Length; x++)
        {
            leaves[x].transform.position = new Vector3(noramlPos[x].x + (Mathf.Sin((x * 0.3f) + sinWave) * 0.03f), noramlPos[x].y + (Mathf.Sin((x * 0.25f) + sinWave) * 0.01f), noramlPos[x].z);
            leaves[x].transform.eulerAngles = new Vector3(0, 180, Mathf.Sin(sinWave) * -0.5f);
        }

        if (lantern != null)
            lantern.transform.eulerAngles = new Vector3(0, 180, Mathf.Sin(sinWave) * -5);

        sinWave += Time.deltaTime;
    }
}