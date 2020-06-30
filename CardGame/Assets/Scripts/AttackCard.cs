using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;        // 引用 UI 額外功能 API
using System.Collections;

public class AttackCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private UILineRenderer line;        // UI 線條渲染
    private Transform arrow;            // 箭頭

    private Vector2 posBegin, posDrag;  // 開始拖拉位置，拖拉中的位置

    private static bool canAttack;      // 是否可以攻擊
    private static Transform parent;    // 被攻擊方父物件
    private static Transform target;    // 被攻擊方

    private void Awake()
    {
        line = GameObject.Find("線條").GetComponent<UILineRenderer>();    // 取得 線條
        arrow = GameObject.Find("箭頭").transform;                        // 取得 箭頭
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        line.enabled = true;                                    // 顯示線條

        // 介面.x = 螢幕.x - 螢幕.寬度 / 2
        posBegin.x = eventData.position.x - Screen.width / 2;
        // 介面.y = 螢幕.y - 螢幕.高度 / 2
        posBegin.y = eventData.position.y - Screen.height / 2;

        line.Points[0] = posBegin;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 介面.x = 螢幕.x - 螢幕.寬度 / 2
        posDrag.x = eventData.position.x - Screen.width / 2;
        // 介面.y = 螢幕.y - 螢幕.高度 / 2
        posDrag.y = eventData.position.y - Screen.height / 2;

        line.Points[1] = posDrag;

        line.Resoloution = (posDrag - posBegin).magnitude / 100;            // 解析度 = 拖拉點 - 起始點 / 100

        arrow.GetComponent<RectTransform>().anchoredPosition = posDrag;     // 箭頭.座標 = 拖拉中的位置

        arrow.up = posDrag - posBegin;                                      // 箭頭.前方 = 拖拉 - 起始 (向量)
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = false;                                   // 隱藏線條
        arrow.position = Vector2.one * 1000;                    // 箭頭 1000, 1000

        if (canAttack && transform.parent != parent)            // 如果 可以攻擊 並且 父物件 不等於 被攻擊方父物件
        {
            StartCoroutine(Attack());                           // 攻擊
        }
    }

    // 滑入被攻擊方
    public void OnPointerEnter(PointerEventData eventData)
    {
        canAttack = true;                   // 可以攻擊
        parent = transform.parent;          // 被攻擊方的父物件
        target = transform;                 // 目標為被攻擊方
    }

    // 滑出被攻擊方
    public void OnPointerExit(PointerEventData eventData)
    {
        canAttack = false;                  // 不可以攻擊
        parent = null;                      // 清空父物件
        target = null;                      // 清空目標物件
    }

    private IEnumerator Attack()
    {
        transform.parent.SetAsLastSibling();    // 變形.父物件.設為最後一個子物件

        Vector3 pos = transform.position;       // 原始位置

        while (transform.position.y != target.position.y)                                                           // 當(攻擊方.Y 不等於 被攻擊方.Y) 時執行
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.6f * Time.deltaTime * 10);     // 攻擊方前往被攻擊方位置
            yield return null;
        }

        transform.position = pos;               // 回到原始位置
    }
}
