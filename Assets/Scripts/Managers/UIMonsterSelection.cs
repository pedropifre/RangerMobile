using UnityEngine;
using UnityEngine.EventSystems;

public class UIMonsterSelection : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform box1;
    public RectTransform box2;
    public RectTransform box3;
    // add more boxes as needed

    private RectTransform imageTransform;
    private Vector2 startPosition;

    private void Start()
    {
        imageTransform = GetComponent<RectTransform>();
        startPosition = imageTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        imageTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            Debug.Log(eventData.position);
        if (RectTransformUtility.RectangleContainsScreenPoint(box1, eventData.position, Camera.main))
        {
            imageTransform.SetParent(box1);
            imageTransform.anchoredPosition = Vector2.zero;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(box2, eventData.position, Camera.main))
        {
            imageTransform.SetParent(box2);
            imageTransform.anchoredPosition = Vector2.zero;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(box3, eventData.position, Camera.main))
        {
            imageTransform.SetParent(box3);
            imageTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            imageTransform.anchoredPosition = startPosition;
        }
    }
}