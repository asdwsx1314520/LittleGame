using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Role
{
    /// <summary>玩家血量</summary>
    public Image[] HeroHp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region 血量全部回復
    /// <summary>血量全部回復</summary>
    public void onRecovery()
    {
        hp = 3;
        for (int i = 0; i < HeroHp.Length; i++)
        {
            HeroHp[i].gameObject.SetActive(true);
        }
    }

    #endregion

    #region 受傷
    /// <summary>受傷</summary>
    public void onHurt()
    {
        hp--;
        if(hp <= 0)
        {
            hp = 0;
            onDeath();
        }
        HeroHp[hp].gameObject.SetActive(false);
    }
    #endregion

    #region 遊戲結束
    /// <summary>遊戲結束</summary>
    public void evtGameOver()
    {
        controler.onFail();
    }
    #endregion

}
