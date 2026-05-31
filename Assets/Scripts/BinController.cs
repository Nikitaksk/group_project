using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class BinController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public TrashItem.TrashType acceptedTrashType;
    public SpriteRenderer binSpriteRenderer;

    private Vector3 dragOffset;
    private float minX;
    private float maxX;

    void Start()
    {
        if (binSpriteRenderer == null)
        {
            binSpriteRenderer = GetComponent<SpriteRenderer>();
            if (binSpriteRenderer == null)
            {
                Debug.LogError("BinController: SpriteRenderer required on " + gameObject.name);
                enabled = false;
                return;
            }
        }

        float screenHalfWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float binHalfWidth = binSpriteRenderer.bounds.extents.x;
        minX = -screenHalfWidth + binHalfWidth;
        maxX = screenHalfWidth - binHalfWidth;

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (!boxCollider.isTrigger)
            boxCollider.isTrigger = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
            return;

        Vector3 pointerWorld = Camera.main.ScreenToWorldPoint(eventData.position);
        dragOffset = transform.position - pointerWorld;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
            return;

        Vector3 pointerWorld = Camera.main.ScreenToWorldPoint(eventData.position);
        float newX = Mathf.Clamp(pointerWorld.x + dragOffset.x, minX, maxX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TrashItem trash = other.GetComponent<TrashItem>();
        if (trash == null)
            return;

        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin || trash.IsDragging)
            return;

        if (acceptedTrashType == trash.trashType)
        {
            UIManager.instance.ShowFeedbackMessage("Dobrze!", Color.green);
            UIManager.instance.AddScore(1);
            Destroy(other.gameObject);
        }
        else
        {
            UIManager.instance.ShowFeedbackMessage("Zły Kosz!", Color.red);
            UIManager.instance.AddScore(-1);
            UIManager.instance.AddMistake(trash.trashType);
        }
    }
}
