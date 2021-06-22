using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttr : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject webPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Border")
        {
            Destroy(gameObject);
        }
        else if(collision.tag == "Fish")
        {
            var web = Instantiate(webPrefab);
            web.transform.SetParent(gameObject.transform.parent,false);
            web.transform.position = gameObject.transform.position;
            web.GetComponent<WebAttr>().damage = damage;
            Destroy(gameObject);
        }
    }
}
