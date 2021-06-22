using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text oneShootCostText;

    public int lv;

    public Transform bulletHolder;
    public GameObject[] gunGos;
    public GameObject[] bullet1Gos;
    public GameObject[] bullet2Gos;
    public GameObject[] bullet3Gos;
    public GameObject[] bullet4Gos;
    public GameObject[] bullet5Gos;

    private int costIndex;
    private const int costBlock = 4;

    private int[] oneShootCosts = { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };


    // Start is called before the first frame update
    void Start()
    {
        // init
        lv = 0;
        costIndex = 0;
        gunGos[costIndex].SetActive(true);
        oneShootCostText.text = $"$ {oneShootCosts[costIndex]}";

    }

    // Update is called once per frame
    void Update()
    {
        ChangeBulletCost();
        Fire();
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject[] selecteBulletGos;

            switch (costIndex / costBlock) 
            {
                case 1:selecteBulletGos = bullet1Gos;break;
                case 2:selecteBulletGos = bullet2Gos;break;
                case 3:selecteBulletGos = bullet3Gos;break;
                case 4:selecteBulletGos = bullet4Gos;break;
                case 5:selecteBulletGos = bullet5Gos;break;
                default :selecteBulletGos = bullet1Gos; break;
            }

            // IsPointerOverGameObject会检测是否点击到了UGUI上
            if (selecteBulletGos != null && !EventSystem.current.IsPointerOverGameObject())
            {
                GameObject bullet = GameObject.Instantiate(selecteBulletGos[lv % selecteBulletGos.Length]);
                bullet.transform.SetParent(bulletHolder, false);
                bullet.transform.position = gunGos[costIndex / costBlock].transform.Find("FirePos").position;
                bullet.transform.rotation = gunGos[costIndex / costBlock].transform.Find("FirePos").rotation;

                var move = bullet.AddComponent<EF_AutoMove>();
                var attr = bullet.GetComponent<BulletAttr>();
                attr.damage = oneShootCosts[costIndex];
                move.dir = Vector3.up;
                move.speed = attr.speed;
            }
        }
    }

    void ChangeBulletCost()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            OnButtonMDown();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            OnButtonPDown();
        }
    }

    #region change gun
    public void OnButtonPDown()
    {
        gunGos[costIndex++ / costBlock].SetActive(false);
        costIndex = costIndex >= oneShootCosts.Length ? 0 : costIndex;
        gunGos[costIndex / costBlock].SetActive(true);
        oneShootCostText.text = $"$ {oneShootCosts[costIndex]}";
    }

    public void OnButtonMDown()
    {
        gunGos[costIndex-- / costBlock].SetActive(false);
        costIndex = costIndex >= 0 ? costIndex : oneShootCosts.Length - 1;
        gunGos[costIndex / costBlock].SetActive(true);
        oneShootCostText.text = $"$ {oneShootCosts[costIndex]}";
    }
    #endregion

}
