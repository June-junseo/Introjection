using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ActivationArea Å¬¸¯µÊ!");
    }
}