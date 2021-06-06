using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform heroTF;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // Exercise Find Method !
        heroTF = GameObject.Find("dwarf_hero").GetComponent<Transform>();
        offset = this.transform.position - heroTF.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = heroTF.position + offset;
        
    }
}
