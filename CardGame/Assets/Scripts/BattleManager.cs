using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 對戰管理器實體物件
    /// </summary>
    public static BattleManager instance;

    [Header("金幣")]
    public Rigidbody coin;
    [Header("遊戲畫面")]
    public GameObject gameView;
    [Header("畫布")]
    public Transform canvas;
    [Header("手牌區域")]
    public Transform handArea;
    [Header("水晶"), Tooltip("水晶圖片，用來顯示的 10 張")]
    public GameObject[] crystalObject;
    [Header("水晶數量介面")]
    public Text textCrystal;
    [Header("擲金幣畫面")]
    public GameObject coinView;

    /// <summary>
    /// 水晶數量
    /// </summary>
    public int crystal;

    [Header("手牌卡牌資訊")]
    /// <summary>
    /// 對戰用牌組：手牌
    /// </summary>
    public List<CardData> battleDeck = new List<CardData>();
    [Header("手牌卡牌遊戲物件")]
    public List<GameObject> handGameObject = new List<GameObject>();

    /// <summary>
    /// 先後攻
    /// true 先
    /// false 後
    /// </summary>
    public bool firstAttack;

    private bool myTurn;
    protected int crystalTotal;

    protected virtual void Start()
    {
        instance = this;
    }

    /// <summary>
    /// 我方結束
    /// </summary>
    public void EndTurn()
    {
        myTurn = false;
    }

    /// <summary>
    /// 對方結束：水晶 +1
    /// </summary>
    public void StartTurn()
    {
        myTurn = true;
        crystalTotal++;
        crystalTotal = Mathf.Clamp(crystalTotal, 1, 10);    // 夾住最大水晶數量
        crystal = crystalTotal;
        Crystal();
        StartCoroutine(GetCard(1, DeckManager.instance, -200, -275));
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartBattle()
    {
        gameView.SetActive(true);   // 顯示遊戲畫面

        ThrowCoin();
    }

    /// <summary>
    /// 擲金幣
    /// </summary>
    private void ThrowCoin()
    {
        coin.AddForce(0, Random.Range(200, 400), 0);            // 推力
        coin.AddTorque(Random.Range(200, 500), 0, 0);           // 旋轉

        Invoke("CheckCoin", 3);                                 // 延遲呼叫檢查方法
        NPCBattleManager.instanceNPC.Invoke("CheckCoin", 3.5f); // NPC 檢查金幣正反面
    }

    /// <summary>
    /// 檢查金幣正反面
    /// rotation.x 為 -1 - 背面
    /// rotation.x 為 0  - 正面
    /// </summary>
    protected virtual void CheckCoin()
    {
        // 三元運算子
        // 先後攻 = 布林運算 ? 成立 : 不成立
        // 錢號 Y > 0.25f 正面
        // print(coin.transform.GetChild(0).position.y);

        firstAttack = coin.transform.GetChild(0).position.y > 0.25f ? true : false;

        coinView.SetActive(false);      // 隱藏金幣畫面

        // 如果 先攻 水晶 1 顆，卡牌 4 張
        int card = 3;

        if (firstAttack)
        {
            crystalTotal = 1;
            crystal = 1;
            card = 4;
        }

        Crystal();

        StartCoroutine(GetCard(card, DeckManager.instance, -200, -275));
    }

    /// <summary>
    /// 處理水晶數量
    /// </summary>
    protected void Crystal()
    {
        // 顯示目前有幾顆水晶
        for (int i = 0; i < crystal; i++)
        {
            crystalObject[i].SetActive(true);
        }

        textCrystal.text = crystal + " / 10";
    }

    /// <summary>
    /// 更新水晶介面與圖片
    /// </summary>
    public void UpdateCrystal()
    {
        for (int i = 0; i < crystalObject.Length; i++)
        {
            if (i < crystal) continue;  // 如果 迴圈編號 < 目前水晶數量 就繼續 (跳過此次)

            crystalObject[i].SetActive(false);
        }

        textCrystal.text = crystal + " / 10";
    }

    /// <summary>
    /// 抽牌組卡牌到手上牌組
    /// </summary>
    protected IEnumerator GetCard(int count, DeckManager deck, int rightY, int handY)
    {
        for (int i = 0; i < count; i++)
        {
            // 抽牌組第一張 放到 手牌 第一張
            battleDeck.Add(deck.deck[0]);
            // 刪除 牌組第一張
            deck.deck.RemoveAt(0);
            // 抽牌組第一張卡牌物件 放到 手牌 第一張
            handGameObject.Add(deck.deckGameObject[0]);
            // 刪除 牌組第一張遊戲物件
            deck.deckGameObject.RemoveAt(0);

            // 等待協程執行結束
            yield return StartCoroutine(MoveCard(rightY, handY));
        }
    }

    /// <summary>
    /// 手牌數量
    /// </summary>
    public int handCardCount;

    /// <summary>
    /// 顯示卡牌再移動到手上
    /// </summary>
    private IEnumerator MoveCard(int rightY, int handY)
    {
        RectTransform card = handGameObject[handGameObject.Count - 1].GetComponent<RectTransform>();       // 取得手牌最後一張[數量 - 1]

        // 進入右手邊中間位置
        card.SetParent(canvas);                 // 將父物件設為畫布
        card.anchorMin = Vector2.one * 0.5f;    // 設定中心點
        card.anchorMax = Vector2.one * 0.5f;    // 設定中心點

        while (card.anchoredPosition.x > 501)   // 當 X > 501 執行移動
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(500, rightY), 0.5f * Time.deltaTime * 50);   // 卡片位置前往 500, 0

            yield return null;                  // 等待一個影格
        }

        yield return new WaitForSeconds(0.2f); // 停留 0.35 秒

        // 爆牌
        if (handCardCount == 10)
        {
            card.GetChild(1).GetComponent<Image>().material = Instantiate(card.GetChild(1).GetComponent<Image>().material);
            card.GetChild(0).GetChild(0).GetComponent<Image>().material = Instantiate(card.GetChild(0).GetChild(0).GetComponent<Image>().material);

            Material m = card.GetChild(1).GetComponent<Image>().material;                   // 取得材質
            Material m0 = card.GetChild(0).GetChild(0).GetComponent<Image>().material;      // 取得材質

            m.SetFloat("Switch", 1);                        // 設定布林值
            m0.SetFloat("Switch", 1);                       // 設定布林值
            float a = 0;                                    // 透明度

            // 隱藏所有文字子物件
            Text[] texts = card.GetComponentsInChildren<Text>();

            for (int i = 0; i < texts.Length; i++) texts[i].enabled = false;

            while (m.GetFloat("AlphaClip") < 4)             // 當 透明度 < 4
            {
                a += 0.1f;                                  // 透明度 遞增
                m.SetFloat("AlphaClip", a);                 // 設定浮點數
                m0.SetFloat("AlphaClip", a);                // 設定浮點數
                yield return null;
            }

            Destroy(card.gameObject);
            battleDeck.RemoveAt(battleDeck.Count - 1);
            handGameObject.RemoveAt(handGameObject.Count - 1);
        }
        // 進入手牌
        else
        {
            card.localScale = Vector3.one * 0.5f;   // 縮小

            bool con = true;

            while (con)   // 當 Y > -275 執行移動
            {
                if (handY < 0) con = card.anchoredPosition.y > handY + 1;
                else con = card.anchoredPosition.y < handY - 1;

                card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, handY), 0.5f * Time.deltaTime * 50);   // 卡片位置前往 500, 0

                yield return null;                  // 等待一個影格
            }

            card.SetParent(handArea);                   // 設定父物件為手牌區域
            card.gameObject.AddComponent<HandCard>();   // 添加手牌腳本 - 可拖拉
            handCardCount++;                            // 手牌數量遞增
        }
    }
}
