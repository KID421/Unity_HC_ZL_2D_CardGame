using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;        // 引用 UI 額外功能 API

// 要求元件(UI 線條渲染)
public class AttackCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private UILineRenderer line;     // UI 線條渲染
    private Transform arrow;
    private GameObject lineObj;
    private Vector2 original;

    private void Awake()
    {
        arrow = GameObject.Find("箭頭").transform;
        lineObj = GameObject.Find("線條");
        line = lineObj.GetComponent<UILineRenderer>();                  // 取得
        line.material = Resources.Load<Material>("線條材質");    // 設定材質
        line.sprite = Resources.Load<Sprite>("線條");
        line.Points = new Vector2[] { Vector2.zero, Vector2.zero };
        line.LineThickness = 100;
        line.ImproveResolution = ResolutionMode.PerSegment;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransform rect = GetComponent<RectTransform>();
        original = transform.position;
        original.x = transform.position.x + rect.sizeDelta.x * rect.localScale.x / 2 - Screen.width / 2;
        original.y = transform.position.y - rect.sizeDelta.y * rect.localScale.y / 2 - Screen.height / 2;

        //original.x = rect.anchorMin.x == 0.5f ? rect.position.x : rect.position.x + rect.sizeDelta.x / 2;
        //original.y = rect.anchorMin.y == 0.5f ? rect.position.y : rect.position.y - rect.sizeDelta.y / 2;
        line.enabled = true;
        line.Points[0] = original;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        pos.x = eventData.position.x - Screen.width / 2;
        pos.y = eventData.position.y - Screen.height / 2;
        line.Points[1] = pos;
        line.Resoloution = (pos - original).magnitude / 100;
        arrow.up = pos - original;
        arrow.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = false;
        arrow.position = Vector2.one * 3000;
    }
}
