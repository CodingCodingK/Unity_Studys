using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null) _instance = GameObject.Find("ScriptsHolder").GetComponent<GameController>();
            return _instance;
        }
    }

    public Text oneShootCostText;
    public Text goldText;
    public Text lvText;
    public Text lvNameText;
    public Text smallCountdownText;
    public Text bigCountdownText;
    public Button bigCountdownButton;
    public Button backButton;
    public Button settingButton;
    public Slider expSlider;

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
    private string[] lvNames = {"新手", "青铜", "白银", "黄金", "白金", "钻石", "大师", "宗师", "王者", "荣耀王者"};

    public int lv = 0;
    public int exp = 0;
    public int gold = 500;
    public const int bigCountdown = 240;
    public const int smallCountdown = 60;
    public float bigTimer = bigCountdown;
    public float smallTimer = smallCountdown;

    private void Awake()
    {
        
    }


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

        UpdateDatas();
        UpdateUI();
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

    #region Click Event
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

    /// <summary>
    /// 点击“奖金”按钮
    /// </summary>
    public void OnBigCountdownButtonDown()
    {
        gold += 500;
        bigTimer = bigCountdown;
        bigCountdownText.gameObject.SetActive(true);
        bigCountdownButton.gameObject.SetActive(false);
    }
    #endregion




    void UpdateUI()
    {
        goldText.text = "$" + gold;
        lvText.text = lv.ToString();
        if (lv / 10 <= 9)
        {
            lvNameText.text = lvNames[lv / 10];
        }
        else
        {
            lvNameText.text = lvNames[9];
        }

        smallCountdownText.text = (int) smallTimer / 10 + "  " + (int) smallTimer % 10;
        bigCountdownText.text = (int) bigTimer + "s";
        expSlider.value = (float)exp / (1000 + 200 * lv);
    }

    void UpdateDatas()
    {
        // Lv and Exp
        while (exp >= 1000 + 200 * lv)
        {
            lv++;
            exp -= (1000 + 200 * lv);
        }

        //time
        bigTimer -= Time.deltaTime;
        smallTimer -= Time.deltaTime;

        if (smallTimer <= 0)
        {
            smallTimer = smallCountdown;
            gold += 50;
        }

        if (bigTimer <= 0 && bigCountdownText.gameObject.activeSelf)
        {
            bigCountdownText.gameObject.SetActive(false);
            bigCountdownButton.gameObject.SetActive(true);
        }

    }
}
