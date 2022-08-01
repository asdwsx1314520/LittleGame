using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    public Animator anim;
    public Main controler;
    public int hp;
    public string strAnAttack;
    public string strAnHit;
    public string strAnDeath;

    #region 判斷是否成功被殺死
    /// <summary>
    /// 判斷是否成功被殺死
    /// </summary>
    /// <param name="value">計算數值</param>
    /// <returns></returns>
    protected bool onJudgeDeath(int value)
    {
        bool isDeath = false;

        if (value >= hp)
        {
            isDeath = true;
        }

        return isDeath;
    }
    #endregion

    #region hp設定
    /// <summary>hp設定</summary>
    public int getHp()
    {
        return hp;
    }
    #endregion

    #region 攻擊
    /// <summary>攻擊</summary>
    public void onAttack()
    {
        anim.SetTrigger(strAnAttack);
    }
    #endregion

    #region 主角擊中判定(動畫標籤)
    /// <summary>擊中判定(動畫標籤)</summary>
    protected void evtHeroAttackHit()
    {
        controler.onHeroAttackHit();
    }
    #endregion

    #region 敵人擊中判定(動畫標籤)
    /// <summary>擊中判定(動畫標籤)</summary>
    protected void evtEnemyAttackHit()
    {
        controler.onEnemyAttackHit();
    }
    #endregion

    #region 受傷
    /// <summary>受傷</summary>
    public void onHit()
    {
        anim.SetTrigger(strAnHit);
    }
    #endregion

    #region 死亡
    /// <summary>死亡</summary>
    protected void onDeath()
    {
        anim.SetTrigger(strAnDeath);
    }
    #endregion

    #region 消滅自己
    /// <summary>消滅自己</summary>
    public void evtDestroy()
    {
        controler.onWin();
        Destroy(this);
    }
    #endregion
}
