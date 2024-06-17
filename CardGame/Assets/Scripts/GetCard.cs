using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;   // 引用 網路連線 API
using System.Collections;

/// <summary>
/// 從 Google 資料庫下載卡牌資料
/// </summary>
public class GetCard : MonoBehaviour
{
    public CardData[] cards;

    [Header("卡牌物件")]
    public GameObject cardObject;
    [Header("卡牌內容")]
    public Transform contentCard;

    private CanvasGroup loadingPanel;
    private Image loading;

    /// <summary>
    /// 取得卡牌資料實體物件
    /// </summary>
    public static GetCard instance;

    /// <summary>
    /// 取得卡牌資料
    /// </summary>
    private IEnumerator GetCardData()
    {
        loadingPanel.alpha = 1;                 // 顯示
        loadingPanel.blocksRaycasts = true;     // 啟動遮擋

        // 網路要求 www = 網路要求.Post("網址", "")
        UnityWebRequest www = UnityWebRequest.PostWwwForm("https://script.google.com/macros/s/AKfycbzSXF5ruHhoOTfulCHmemuA0vgS9Psa0yqN3lxLxEx23UcWUVdWAy8-YzxUqzVh7hW_/exec", "");
        // 等待 網路要求時間
        // yield return www.SendWebRequest();

        // 網路要求
        www.SendWebRequest();

        // 當 載入進度 < 1
        while (www.downloadProgress < 1)
        {
            yield return null;
            loading.fillAmount = www.downloadProgress;  // 更新載入吧條
        }

        if (www.isHttpError || www.isNetworkError)
        {
            print("連線錯誤：" + www.error);
        }
        else
        {
            // 將 JSON 轉為陣列並儲存在 cards 內
            cards = JsonHelper.FromJson<CardData>(www.downloadHandler.text);
            // 呼叫建立卡牌方法
            CreateCard();
        }

        yield return new WaitForSeconds(0.5f);  // 等待
        loadingPanel.alpha = 0;                 // 隱藏
        loadingPanel.blocksRaycasts = false;    // 取消遮擋
    }

    /// <summary>
    /// 建立卡牌物件
    /// </summary>
    private void CreateCard()
    {
        // 迴圈執行 0 - 13 
        for (int i = 0; i < cards.Length; i++)
        {
            // 變形 = 生成(物件，父物件).變形
            Transform temp = Instantiate(cardObject, contentCard).transform;
            // 卡片資料
            CardData card = cards[i];
            // 尋找子物件並更新文字
            temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
            temp.Find("攻擊").GetComponent<Text>().text = card.attack.ToString();
            temp.Find("血量").GetComponent<Text>().text = card.hp.ToString();
            temp.Find("名稱").GetComponent<Text>().text = card.name;
            temp.Find("描述").GetComponent<Text>().text = card.description;
            // 尋找圖片子物件.圖片 = 來源.載入<圖片>(檔案名稱)
            temp.Find("遮色片").Find("圖片").GetComponent<Image>().sprite = Resources.Load<Sprite>(card.file);
            // 添加圖鑑卡牌 並設定 圖鑑卡牌編號
            temp.gameObject.AddComponent<BookCard>().index = card.index;
        }
    }

    private void Awake()
    {
        instance = this;

        loadingPanel = GameObject.Find("載入畫面").GetComponent<CanvasGroup>();
        loading = GameObject.Find("進度條").GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(GetCardData());
    }
}

/// <summary>
/// 卡片資料
/// </summary>
/// 序例化：讓資料可以顯示在屬性面板上
[System.Serializable]
public class CardData
{
    public int index;
    public string name;
    public string description;
    public int cost;
    public float attack;
    public float hp;
    public string file;
}


/// <summary>
/// 將 JSON 轉為陣列資料
/// </summary>
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}