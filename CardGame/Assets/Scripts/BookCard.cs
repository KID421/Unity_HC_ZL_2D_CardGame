using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 圖鑑內的卡牌
/// </summary>
public class BookCard : MonoBehaviour
{
    /// <summary>
    /// 圖鑑內卡牌的編號
    /// </summary>
    public int index;

    private void Start()
    {
        // 取得按鈕元件.點擊事件.添加監聽器(選取此圖鑑卡牌)
        GetComponent<Button>().onClick.AddListener(ChooseCard);
    }

    /// <summary>
    /// 選取此圖鑑卡牌
    /// </summary>
    public void ChooseCard()
    {
        DeckManager.instance.AddCard(index, "玩家");
    }
}
