using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;        // 引用 UI 額外功能 API
using System.Collections;

// 要求元件(UI 線條渲染)
public class AttackCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    private UILineRenderer line;     // UI 線條渲染
    private Transform arrow;
    private GameObject lineObj;
    
    private Vector2 posStart;
    private Vector2 posEnd;

    private static bool attack;
    private static Transform parent;
    private static Transform cardAttack;
    private Transform cardTarget;

    private void Awake()
    {
        arrow = GameObject.Find("箭頭").transform;
        lineObj = GameObject.Find("線條");
        line = lineObj.GetComponent<UILineRenderer>();
        line.material = Resources.Load<Material>("線條材質");
        line.sprite = Resources.Load<Sprite>("線條");
        line.Points = new Vector2[] { Vector2.zero, Vector2.zero };
        line.LineThickness = 100;
        line.ImproveResolution = ResolutionMode.PerSegment;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posStart.x = eventData.position.x - Screen.width / 2;
        posStart.y = eventData.position.y - Screen.height / 2;
        line.Points[0] = posStart;
        line.enabled = true;
        attack = false;
        parent = transform.parent;
        cardAttack = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        posEnd.x = eventData.position.x - Screen.width / 2;
        posEnd.y = eventData.position.y - Screen.height / 2;
        line.Points[1] = posEnd;

        line.Resoloution = (posEnd - posStart).magnitude / 100;
        
        arrow.up = posEnd - posStart;
        arrow.GetComponent<RectTransform>().anchoredPosition = posEnd;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = false;
        arrow.position = Vector2.one * 3000;
        attack = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (attack && transform.parent != parent)
        {
            attack = false;
            cardTarget = transform;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        parent.SetAsLastSibling();

        Vector3 pos = cardAttack.position;

        while (cardAttack.position.y != cardTarget.position.y)
        {
            cardAttack.position = Vector3.Lerp(cardAttack.position, cardTarget.position, 0.8f * Time.deltaTime * 10);
            yield return null;
        }

        cardTarget = null;
        cardAttack.position = pos;
    }
}
