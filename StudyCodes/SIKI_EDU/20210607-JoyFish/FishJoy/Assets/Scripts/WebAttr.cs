using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAttr : MonoBehaviour
{
    public float damage;
    public float liveTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, liveTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fish")
        {
            collision.SendMessage("TakeDamage",damage);
        }
    }
}
