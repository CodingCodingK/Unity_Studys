using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaker : MonoBehaviour
{
    public Transform fishHolder;
    public Transform[] genPositions;
    public GameObject[] fishPrefabs;

    public float fishWaitTime = 50f;
    public float waveWaitTime = 100f;

    // Start is called before the first frame update
    void Start()
    {
		InvokeRepeating("MakeFishes", 0, waveWaitTime);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeFishes()
    {
	    var posIndex = Random.Range(0, genPositions.Length);
	    var preIndex = Random.Range(0, fishPrefabs.Length);
	    var maxNum = fishPrefabs[preIndex].GetComponent<FishAttr>().maxNum;
	    var maxSpeed = fishPrefabs[preIndex].GetComponent<FishAttr>().maxSpeed;
	    var num = Random.Range(maxNum/2 + 1, maxNum);
	    var speed = Random.Range(maxSpeed / 2,maxSpeed);

	    int moveType = Random.Range(0, 2);//0直走，1转弯
	    int angOffset;//直走角度
	    int angSpeed;//转弯角速度

	    if (moveType == 0)
	    {
		    angOffset = Random.Range(-22,22);
		    StartCoroutine(GenStraightFish(posIndex, preIndex, num, speed, angOffset));
	    }
	    else
	    {
		    if (Random.Range(0, 2) == 0)
		    {
			    angSpeed = Random.Range(-15, -9);
		    }
		    else
		    {
			    angSpeed = Random.Range(9, 15);
		    }

		    StartCoroutine(GenTurnFish(posIndex,preIndex,num,speed,angSpeed));
	    }


    }

    IEnumerator GenStraightFish(int posIndex,int preIndex,int num,int speed,int angOffset)
    {
	    for (int i = 0; i < num; i++)
	    {
			GameObject fish = Instantiate(fishPrefabs[preIndex]);
			fish.transform.SetParent(fishHolder,false);
			fish.transform.localPosition = genPositions[posIndex].localPosition;
			fish.transform.localRotation = genPositions[posIndex].localRotation;
			fish.transform.Rotate(0,0,angOffset);
			fish.GetComponent<SpriteRenderer>().sortingOrder += i;
			fish.AddComponent<EF_AutoMove>().speed = speed;
			yield return new WaitForSeconds(fishWaitTime);
	    }
    }

    IEnumerator GenTurnFish(int posIndex, int preIndex, int num, int speed, int angSpeed)
    {
	    for (int i = 0; i < num; i++)
	    {
		    GameObject fish = Instantiate(fishPrefabs[preIndex]);
		    fish.transform.SetParent(fishHolder, false);
		    fish.transform.localPosition = genPositions[posIndex].localPosition;
		    fish.transform.localRotation = genPositions[posIndex].localRotation;
		    fish.GetComponent<SpriteRenderer>().sortingOrder += i;
		    fish.AddComponent<EF_AutoMove>().speed = speed;
		    fish.AddComponent<EF_AutoRotate>().speed = angSpeed;
			yield return new WaitForSeconds(fishWaitTime);
	    }
    }
}
