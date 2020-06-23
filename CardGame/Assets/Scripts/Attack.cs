using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class Attack : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    UILineRenderer line;

    private void Start()
    {
        line = GetComponent<UILineRenderer>();
        line.material = Resources.Load<Material>("線條材質");
        Vector2[] p = { Vector2.zero, Vector2.zero };
        line.Points = p;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        line.Points[0] = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        line.Points[1] = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
