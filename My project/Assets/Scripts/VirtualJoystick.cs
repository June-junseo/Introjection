using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform background;
    public RectTransform handle;

    private float radius;

    public Vector2 Input { get; private set; }

 
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Ω√¿€");
    }

    public void OnDrag(PointerEventData eventData)
    {
        var touchPosition = eventData.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, touchPosition, eventData.enterEventCamera, out Vector2 position))
        {
            var delta = position;
            delta = Vector2.ClampMagnitude(delta, radius);
            handle.anchoredPosition = delta;
            Input = delta / radius;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    private void Start()
    {
        radius = background.rect.width * 0.5f;
    }


}
