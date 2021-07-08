using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour
{
    public Model model;
    public View view;
    
    // Start is called before the first frame update
    void Awake()
    {
        model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
        view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
