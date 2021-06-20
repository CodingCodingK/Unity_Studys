using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_AutoRotate : MonoBehaviour
{
	public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
	    transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
