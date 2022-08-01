using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GAME_STATE
{
    /// <summary>等待開始</summary>
    IDLE,
    /// <summary>開始</summary>
    START,
    /// <summary>轉動</summary>
    RANDOM,
    /// <summary>決定</summary>
    DECIDE,
    /// <summary>數值計算</summary>
    COUND,
    /// <summary>決鬥 </summary>
    FIGHTING,
    /// <summary>獲勝</summary>
    WIN,
    /// <summary>失敗</summary>
    FAIL
}
//腳本
public class Main : MonoBehaviour
{
    public Image imgMenubackGround;
    public Image imgWinBackground;

    private GAME_STATE state;
    /// <summary>關卡</summary>
    private int ntLevel = 0;
    /// <summary>選擇的位置</summary>
    public Image imgAim;
    /// <summary>開始轉動間隔</summary>
    public float fltStart = 0;
    /// <summary>停止轉動間隔</summary>
    public float fltStop = 0;
    /// <summary>當前動作的骰子</summary>
    private int ntNowIndex = 0;
    /// <summary>各種時間計算</summary>
    private float time = 0;
    /// <summary>最大時間</summary>
    public float nmTimeMax;
    /// <summary>剩餘時間顯示</summary>
    public Text textTime;

    public Hero Hero;
    public Role nowEnemy;
    public Role[] Enemy;

    /// <summary>玩家計算後的攻擊力</summary>
    private int ntHeroAttack;
    /// <summary>玩家攻擊力顯示</summary>
    public Text textHeroAttack;
    /// <summary>敵人的血量</summary>
    private int ntEmemyHp;
    /// <summary>敵人血量顯示</summary>
    public Text textEmemy;
    /// <summary>答案的位置</summary>
    public Text[] textDecide;
    /// <summary>答案的排列</summary>
    public int[] AnswerDice;
    /// <summary>骰子按鈕</summary>
    public Dince[] DiceImg;
    /// <summary>骰子開關</summary>
    private bool isDiceOpen;
    /// <summary>總思考時間</summary>
    private float fltDecideTotalTime = 0;
    /// <summary>總思考時間顯示</summary>
    public Text textDecideTotalTime;

    void Awake()
    {
        imgMenubackGround.gameObject.SetActive(true);
        imgWinBackground.gameObject.SetActive(false);
        state = GAME_STATE.IDLE;
        isDiceOpen = false;
        textTime.text = "" + nmTimeMax;
        fltDecideTotalTime = 0;
    }

    void Update()
    {
        onStart();
        onRandom();
        onDecideTime();
        onCound();
        onFighting();
    }

    #region 生成怪物
    /// <summary>生成怪物</summary>
    private void onMonsterSpawn()
    {
        nowEnemy = Instantiate(Enemy[ntLevel]);
        nowEnemy.controler = this;
        ntEmemyHp = Enemy[ntLevel].getHp();
        textEmemy.text = "" + ntEmemyHp;
    }
    #endregion

    #region 開始遊戲
    /// <summary>開始遊戲</summary>
    public void btnStartGame()
    {
        if (state != GAME_STATE.IDLE) return;
        state = GAME_STATE.START;
        Hero.onRecovery();
        imgMenubackGround.gameObject.SetActive(false);
        for (int i = 0; i < DiceImg.Length; i++)
        {
            DiceImg[i].onEnable(false);
        }

        onMonsterSpawn();
    }
    #endregion

    #region 開始執骰
    /// <summary>開始執骰</summary>
    public void onStart()
    {
        if (state != GAME_STATE.START) return;
        time += 1 * Time.deltaTime;
        if (ntNowIndex < 3)
        {
            if (time >= fltStart)
            {
                DiceImg[ntNowIndex].setState(STATE.CHANGE);
                ntNowIndex++;
                time = 0;
                onStart();
            }
        }
        else
        {
            state = GAME_STATE.RANDOM;
            time = 0;
            ntNowIndex = 0;
        }
    }
    #endregion

    #region 隨機給予數字
    /// <summary>隨機給予數字</summary>
    public void onRandom()
    {
        if (state != GAME_STATE.RANDOM) return;
        time += 1 * Time.deltaTime;
        if (ntNowIndex < 3)
        {
            if (time >= fltStop)
            {
                int ran = Random.Range(0, 6);
                DiceImg[ntNowIndex].Decide(ran);
                ntNowIndex++;
                time = 0;
                onRandom();
            }
        }
        else
        {
            state = GAME_STATE.DECIDE;
            ntNowIndex = 0;
            time = 0;
            isDiceOpen = true;
        }
    }
    #endregion

    #region 玩家開始決定
    /// <summary>玩家決定時間</summary>
    private void onDecideTime()
    {
        if (state != GAME_STATE.DECIDE) return;
        fltDecideTotalTime += 1 * Time.deltaTime;
        if (isDiceOpen)
        {
            time = 5;
            textTime.text = "" + time;
            for (int i = 0; i < DiceImg.Length; i++)
            {
                DiceImg[i].onEnable(true);
            }
            isDiceOpen = false;
        }

        if (time > 0)
        {
            time -= 1 * Time.deltaTime;
        }
        else
        {
            time = 0;
            state = GAME_STATE.COUND;
            AnswerZero();
        }
        textTime.text = "" + time;
    }
    #endregion

    #region 決定數字(玩家點擊按鈕)
    /// <summary>
    /// 決定數字(玩家點擊按鈕)
    /// </summary>
    /// <param name="value"></param>
    public void onDecide(int value)
    {
        if (state != GAME_STATE.DECIDE) return;
        if (ntNowIndex > 3) return;

        textDecide[ntNowIndex].text = "" + value;

        AnswerDice[ntNowIndex] = value;
        ntNowIndex++;

        if(ntNowIndex >= textDecide.Length)
        {
            state = GAME_STATE.COUND;
            return;
        }
        imgAim.rectTransform.position = textDecide[ntNowIndex].rectTransform.position;

    }
    #endregion

    #region 玩家答案清空
    /// <summary>玩家答案清空</summary>
    private void AnswerEmpty()
    {
        for (int i = 0; i < AnswerDice.Length; i++)
        {
            AnswerDice[i] = 0;
            textDecide[i].text = "";
        }
    }
    #endregion

    #region 玩家答案歸零
    /// <summary>玩家答案歸零</summary>
    private void AnswerZero()
    {
        for (int i = 0; i < AnswerDice.Length; i++)
        {
            AnswerDice[i] = 0;
            textDecide[i].text = "" + AnswerDice[i];
        }
    }
    #endregion

    #region 數值計算決鬥
    /// <summary>數值計算決鬥</summary>
    private void onCound()
    {
        if (state != GAME_STATE.COUND) return;
        ntHeroAttack = AnswerDice[0] * AnswerDice[1] + AnswerDice[2];
        textHeroAttack.text = "" + ntHeroAttack;

        state = GAME_STATE.FIGHTING;
    }
    #endregion

    #region 戰鬥表演
    /// <summary>戰鬥表演</summary>
    private void onFighting()
    {
        if (state != GAME_STATE.FIGHTING) return;

        if(ntHeroAttack >= ntEmemyHp)
        {
            state = GAME_STATE.WIN;
            Hero.onAttack();
        }
        else
        {
            state = GAME_STATE.FAIL;
            nowEnemy.onAttack();
        }
    }
    #endregion

    #region 攻擊判定(玩家)
    /// <summary>攻擊判定(玩家)</summary>
    public void onHeroAttackHit()
    {
        nowEnemy.onHit();
    }
    #endregion

    #region 攻擊判定(敵人)
    /// <summary>攻擊判定(敵人)</summary>
    public void onEnemyAttackHit()
    {
        Hero.onHurt();
        if(Hero.hp != 0)
        {
            Hero.onHit();
            time = 0;
            textTime.text = "" + time;
            ntHeroAttack = 0;
            textHeroAttack.text = "" + ntHeroAttack;
            imgAim.rectTransform.position = textDecide[0].rectTransform.position;
            ntNowIndex = 0;
            AnswerEmpty();
            state = GAME_STATE.START;
        }
    }
    #endregion

    #region 獲勝
    /// <summary>獲勝</summary>
    public void onWin()
    {
        if (state != GAME_STATE.WIN) return;
        if(ntLevel == 3)
        {
            imgWinBackground.gameObject.SetActive(true);
            textDecideTotalTime.text = "" + fltDecideTotalTime + "s";
            return;
        }
        time = 0;
        textTime.text = "" + time;
        ntHeroAttack = 0;
        textHeroAttack.text = "" + ntHeroAttack;
        imgAim.rectTransform.position = textDecide[0].rectTransform.position;
        ntNowIndex = 0;
        AnswerEmpty();
        state = GAME_STATE.IDLE;
        ntLevel++;
        btnStartGame();
    }
    #endregion

    #region 失敗
    /// <summary>失敗</summary>
    public void onFail()
    {
        if (state != GAME_STATE.FAIL) return;
        SceneManager.LoadScene(1);
    }
    #endregion

    #region 重新開始遊戲
    /// <summary>重新開始遊戲</summary>
    public void btnReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion

}
