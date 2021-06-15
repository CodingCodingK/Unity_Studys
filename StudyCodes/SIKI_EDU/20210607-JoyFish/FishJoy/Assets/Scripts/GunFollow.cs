using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFollow : MonoBehaviour
{
    public RectTransform UGUICanvas;
    public Camera mainCamera;
    private Vector3 mouseConvertedPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 取屏幕鼠标位置并转换为局部坐标系UGUICanvas下的Vector
        RectTransformUtility.ScreenPointToWorldPointInRectangle
		    (UGUICanvas,new Vector2(Input.mousePosition.x, Input.mousePosition.y),mainCamera, out mouseConvertedPosition);
        // 取 鼠标位置与炮台点连成的线 与 垂直线 的角度
        var z =
	        mouseConvertedPosition.x > transform.position.x
		        ? -Vector3.Angle(Vector3.up, mouseConvertedPosition - transform.position)
		        : Vector3.Angle(Vector3.up, mouseConvertedPosition - transform.position);
        // 转动炮台角度
        transform.localRotation = Quaternion.Euler(0, 0, z);

    }

	private void OnCollisionEnter(Collision collision)
	{
		
	}
}
