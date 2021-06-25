using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_MoveTo : MonoBehaviour
{
    GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("GoldCollect");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,go.transform.position,10 * Time.deltaTime);
    }
}
