using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttr : MonoBehaviour
{
	public int maxNum;
	public int maxSpeed;
    public int hp;
	public int exp;
	public int gold;
	public GameObject diePrefab;
	public GameObject goldPrefab;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Border")
		{
            GameObject.Destroy(this.gameObject);
		}
	}

	public void TakeDamage(int value)
    {
		hp -= value;
        if (hp <= 0)
        {
			GameController.Instance.exp += exp;
			GameController.Instance.gold += gold;
			GameObject die = Instantiate(diePrefab);
            die.transform.SetParent(gameObject.transform.parent);
            die.transform.position = gameObject.transform.position;
            die.transform.rotation = gameObject.transform.rotation;
            die.transform.localScale = gameObject.transform.localScale;

            var ef = gameObject.GetComponent<EF_PlayEffect>();

			if (ef!=null)
            {
                ef.PlayEffect();
            }

            GameObject goldGO = Instantiate(goldPrefab);
            goldGO.transform.SetParent(gameObject.transform.parent);
            goldGO.transform.position = gameObject.transform.position;
            goldGO.transform.rotation = gameObject.transform.rotation;
            goldGO.transform.localScale = gameObject.transform.localScale;

			Destroy(gameObject);

        }

    }
}
  