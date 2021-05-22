using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Rigidbody rd;
    public int score;
    private Text scoreText;
    private GameObject completeGO;
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        completeGO = GameObject.Find("CompleteText");
        //var x = AssetDatabase.LoadAssetAtPath<GameObject>(@"Assets\Prefabs\\ScoreText.prefab");
        //scoreText = x.GetComponent<Text>();
        scoreText.text = "Score : 0";
        completeGO.GetComponent<Text>().text = "Congratulations !";
        completeGO.SetActive(false);
        //AssetDatabase.LoadAssetAtPath
    }
    
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        rd.AddForce(new Vector3(h,0,v) * 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            score++;
            scoreText.text = $"Score : {score}";
            if (score >= 8) completeGO.SetActive(true);
        }
            
    }

}
