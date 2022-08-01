using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum STATE
{
    /// <summary>等待</summary>
    IDLE,
    /// <summary>開始</summary>
    START,
    /// <summary>轉動</summary>
    CHANGE,
    /// <summary>決定</summary>
    DECIDE
}

public class Dince : MonoBehaviour
{
    /// <summary>控制器</summary>
    public Main controler;
    /// <summary>第幾個骰子</summary>
    public int type;
    /// <summary>自己</summary>
    private Button btn;
    /// <summary>骰子狀態</summary>
    private STATE state;
    /// <summary>骰子動畫</summary>
    public Animator anim;
    private Color32[] clrDice;
    /// <summary>骰子的索引</summary>
    private int valueIndex;
    /// <summary>骰子的數值</summary>
    private int value;
    public Image imgDice;
    public Text textDice;
    /// <summary>改變的時間</summary>
    private float fltChange;
    /// <summary>顏色</summary>
    private int index = 0;
    private int[] nmPoint = new int[6] { 0, 1, 2, 3, 4, 5 };

    void Start()
    {
        btn = this.GetComponent<Button>();
        btn.interactable = false;

        index = 0;
        fltChange = 0.1f;
        this.state = STATE.IDLE;
        clrDice = new Color32[6];
        clrDice[0] = new Color32(255, 255, 255, 255);
        clrDice[1] = new Color32(255, 0, 13, 255);
        clrDice[2] = new Color32(82, 255, 0, 255);
        clrDice[3] = new Color32(0, 255, 255, 255);
        clrDice[4] = new Color32(0, 83, 255, 255);
        clrDice[5] = new Color32(238, 0, 255, 255);

        for(int i = 0; i < nmPoint.Length; i++)
        {
            int orig = nmPoint[i];
            int ran = Random.Range(0, 6);

            nmPoint[i] = nmPoint[ran];
            nmPoint[ran] = orig;
        }
    }

    void Update()
    {
        ChangeIng();
    }

    public void setState(STATE state)
    {
        this.state = state;
    }

    public void StartRole()
    {

    }

    #region 骰子職骰動畫
    /// <summary>骰子職骰動畫</summary>
    public void ChangeIng()
    {
        if(state == STATE.CHANGE)
        {
            fltChange += 1.0f * Time.deltaTime;
            if(fltChange > 0.1f) 
            {
                if(index > 5)
                {
                    index = 0;
                }
                imgDice.color = clrDice[nmPoint[index]];
                textDice.text = "" + (nmPoint[index] + 1);
                fltChange = 0;
                index++;
            }
        }
    }
    #endregion

    #region 決定骰子數字
    /// <summary>
    /// 決定骰子數字
    /// </summary>
    /// <param name="value">數值</param>
    public void Decide(int value)
    {
        if (state != STATE.CHANGE) return;

        setState(STATE.DECIDE);
        this.valueIndex = value;
        anim.SetTrigger("decied");
    }
    #endregion

    #region 決定數字的動畫標籤
    /// <summary>決定數字的動畫標籤</summary>
    public void evtDecide()
    {
        if (state != STATE.DECIDE) return;

        setState(STATE.DECIDE);
        imgDice.color = clrDice[nmPoint[valueIndex]];
        textDice.text = "" + (nmPoint[valueIndex] + 1);
        value = (nmPoint[valueIndex] + 1);
    }
    #endregion

    #region 開啟按鈕功能
    /// <summary>
    /// 開啟按鈕功能
    /// </summary>
    /// <param name="isOpen">是否開啟</param>
    public void onEnable(bool isOpen)
    {
        btn.interactable = isOpen;
    }
    #endregion

    #region 點擊按鈕
    /// <summary>點擊按鈕</summary>
    public void onHit()
    {
        controler.onDecide(value);
        onEnable(false);
    }
    #endregion
}
