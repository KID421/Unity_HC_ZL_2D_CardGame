using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;        // 引用 UI 額外功能 API

// 要求元件(UI 線條渲染)
[RequireComponent(typeof(UILineRenderer))]
public class AttackCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UILineRenderer line;     // UI 線條渲染

    private void Awake()
    {
        line = GetComponent<UILineRenderer>();                  // 取得
        line.material = Resources.Load<Material>("線條材質");    // 設定材質
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
