using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform activationArea;
    public RectTransform background;
    public RectTransform handle;         

    private float radius;
    public Vector2 Input { get; private set; }

    private bool isActive = false; 

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(activationArea, eventData.position, eventData.pressEventCamera))
        {
            isActive = true;
            background.position = eventData.position;
            background.gameObject.SetActive(true);

            Debug.Log("조이스틱 시작!");
        }
        else
        {
            isActive = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isActive)
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out Vector2 localPos))
        {
            Vector2 delta = Vector2.ClampMagnitude(localPos, radius);

            handle.anchoredPosition = delta;

            Input = delta / radius;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isActive)
        {
            return;
        }

        Input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);

        Debug.Log("조이스틱 종료");
    }

    private void Start()
    {
        radius = background.rect.width * 0.5f;
        background.gameObject.SetActive(false); 
    }
}
