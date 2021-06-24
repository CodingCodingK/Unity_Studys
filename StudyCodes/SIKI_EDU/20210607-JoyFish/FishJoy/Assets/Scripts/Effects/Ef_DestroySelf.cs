using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_DestroySelf : MonoBehaviour
{
    public float delay = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
