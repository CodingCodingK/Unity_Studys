using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform TargetTransform;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - TargetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = TargetTransform.position + offset;
        Debug.Log(new Vector3(0.001f, 0.00f, 0.00f));
    }
}
