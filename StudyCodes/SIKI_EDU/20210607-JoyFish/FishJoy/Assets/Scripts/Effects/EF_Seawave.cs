using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_Seawave : MonoBehaviour
{
    Vector3 targetPos;
    void Start()
    {
        targetPos = - gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position =
            Vector3.MoveTowards(gameObject.transform.position, targetPos, 5f * Time.deltaTime);
    }
}
