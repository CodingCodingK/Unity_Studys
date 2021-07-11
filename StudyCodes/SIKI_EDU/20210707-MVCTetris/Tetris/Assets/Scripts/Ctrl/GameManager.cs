using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPause = true;

    private Shape currentShape = null;

    private Ctrl ctrl;
    
    /// <summary>
    /// Shape prefabs
    /// </summary>
    public Shape[] shapes;

    /// <summary>
    /// 控制新生成的Shapes的颜色
    /// </summary>
    public Color[] colors;
    
    // Start is called before the first frame update
    void Awake()
    {
        ctrl = GetComponent<Ctrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause) return;
        if (currentShape == null)
        {
            CreateShape();
        }
    }

    public void StartGame()
    {
        isPause = false;
        if (currentShape != null)
        {
            currentShape.Resume();
        }
    }
    
    public void PauseGame()
    {
        isPause = true;
        if (currentShape != null)
        {
            currentShape.Pause();
        }
    }
    
    void CreateShape()
    {
        int index = Random.Range(0, shapes.Length - 1);
        int indexColor = Random.Range(0, colors.Length - 1);
        
        currentShape = Instantiate(shapes[index]);
        currentShape.InitData(colors[indexColor],ctrl,this);
    }

    public void CurrentShapeComplete()
    {
        currentShape = null;
    }
}
