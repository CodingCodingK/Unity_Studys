using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaker : MonoBehaviour
{
    public Transform fishHolder;
    public Transform[] genPositions;
    public GameObject[] fishPrefabs;


    // Start is called before the first frame update
    void Start()
    {
        
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
            
	    }
	    else
	    {

	    }
    }
}
