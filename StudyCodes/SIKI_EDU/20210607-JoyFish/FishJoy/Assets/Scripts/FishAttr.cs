using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttr : MonoBehaviour
{
	public int maxNum;
	public int maxSpeed;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Border")
		{
			Debug.Log(collision);
			GameObject.Destroy(this.gameObject);
		}
	}

	public void TakeDamage(int value)
    {
		// TODO
		Debug.Log($"Damage:{value}");
    }
}
