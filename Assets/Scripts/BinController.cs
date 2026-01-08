using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))] // Bins need a collider to detect trash
public class BinController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TrashItem.TrashType acceptedTrashType; // Set this in the Inspector for each bin
    public SpriteRenderer binSpriteRenderer;     // Assign this for boundary calculations

    private Vector3 dragOffset; // Offset for smooth dragging
    private float minX, maxX; // Boundaries for horizontal dragging

    void Start()
    {
        if (binSpriteRenderer == null)
        {
            binSpriteRenderer = GetComponent<SpriteRenderer>();
            if (binSpriteRenderer == null)
            {
                Debug.LogError("BinController: No SpriteRenderer found on this GameObject or assigned. Cannot calculate dragging boundaries.");
                enabled = false; // Disable script if no renderer
                return;
            }
        }

        // Calculate dragging boundaries based on screen edges and bin's width
        float screenHalfWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float binHalfWidth = binSpriteRenderer.bounds.extents.x;

        minX = -screenHalfWidth + binHalfWidth; // Left edge of screen
        maxX = screenHalfWidth - binHalfWidth;  // Right edge of screen

        // Ensure the collider is a trigger
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null && !boxCollider.isTrigger)
        {
            Debug.LogWarning("BinController: Collider on " + gameObject.name + " is not a trigger. Setting to trigger.");
            boxCollider.isTrigger = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Calculate the offset between the object's position and the pointer's world position.
        Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        dragOffset = transform.position - pointerWorldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Get the pointer's current world position and apply the offset.
        Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 targetPosition = pointerWorldPosition + dragOffset;

        // Clamp the movement to the horizontal axis and within screen bounds.
        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Dragging has finished. We don't need to do anything here because the
        // OnTriggerEnter2D handles the collection logic automatically.
        // You could add sound effects here if you wanted.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TrashItem trash = other.GetComponent<TrashItem>();
        if (trash != null)
        {
            if (acceptedTrashType == trash.trashType)
            {
                UIManager.instance.ShowFeedbackMessage("Dobrze!", Color.green);
            }
            else
            {
                UIManager.instance.ShowFeedbackMessage("Zły Kosz!", Color.red);
            }
            Destroy(other.gameObject);
        }
    }
}
