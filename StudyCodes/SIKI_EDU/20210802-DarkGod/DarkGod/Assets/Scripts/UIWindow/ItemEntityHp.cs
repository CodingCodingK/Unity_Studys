using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血条物体
/// </summary>
public class ItemEntityHp : MonoBehaviour
{
    #region UI Define

    public Image imgHPGray;
    public Image imgHPRed;

    public Animation criticalAni;
    public Text txtCritical;
    
    public Animation dodgeAni;
    public Text txtDodge;
    
    public Animation damageAni;
    public Text txtDamage;
    
    #endregion

    #region Other Define

    

    #endregion

    public void SetCritical(int num)
    {
        criticalAni.Stop();
        txtCritical.text = "暴击 " + num;
        criticalAni.Play();
    }
    
    public void SetDodge()
    {
        dodgeAni.Stop();
        dodgeAni.Play();
    }
    
    public void SetHurt(int num)
    {
        damageAni.Stop();
        txtDamage.text = "-" + num;
        damageAni.Play();
    }

}