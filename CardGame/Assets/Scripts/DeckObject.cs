using UnityEngine;
using UnityEngine.UI;

public class DeckObject : MonoBehaviour
{
    /// <summary>
    /// 牌組物件的編號
    /// </summary>
    public int index;

    private void Start()
    {
        transform.Find("增加").GetComponent<Button>().onClick.AddListener(AddCard);
        transform.Find("減少").GetComponent<Button>().onClick.AddListener(DeleteCard);
    }

    /// <summary>
    /// 牌組物件內的增加卡牌按鈕功能
    /// </summary>
    public void AddCard()
    {
        DeckManager.instance.AddCard(index, "玩家");
    }

    /// <summary>
    /// 牌組物件內的刪除卡牌按鈕功能
    /// </summary>
    public void DeleteCard()
    {
        DeckManager.instance.DeleteCard(index);
    }
}
