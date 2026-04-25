using UnityEngine;
using UnityEngine.EventSystems;

public class TrashItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // Defines the possible types of trash. You can add more here!
    public enum TrashType
    {
        Paper,
        Plastic,
        Organic,
        Glass,
        Other
    }

    public TrashType trashType;
    
    private Vector3 startPosition;
    private Vector3 dragOffset;
    private bool isDragging = false;
    private bool startPositionSet = false;
    private float destroyY = -10f; // Lowered for room cleaning mode

    void Start()
    {
        // Save the initial position when the object starts
        if (!startPositionSet)
        {
            startPosition = transform.position;
            startPositionSet = true;
        }
    }

    void Update()
    {
        // Only apply falling logic if we are NOT in MultiBin (CleanTheRoom) mode
        // Or if the object has been moved far below the screen
        if (GameData.CurrentGameMode != GameData.GameMode.MultiBin && transform.position.y < destroyY)
        {
            CheckIfMissed();
            Destroy(gameObject);
        }
    }

    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
        transform.position = pos;
        startPositionSet = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0;
        dragOffset = transform.position - mousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0;
        transform.position = mousePos + dragOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        
        // Check if we are over a bin
        BinController bin = FindOverlappingBin();
        
        if (bin != null)
        {
            if (bin.acceptedTrashType == trashType)
            {
                UIManager.instance.ShowFeedbackMessage("Dobrze!", Color.green);
                UIManager.instance.AddScore(1);
                Destroy(gameObject);
            }
            else
            {
                UIManager.instance.ShowFeedbackMessage("Zły Kosz!", Color.red);
                UIManager.instance.AddScore(-1);
                UIManager.instance.AddMistake(trashType);
                ReturnToStart();
            }
        }
        else
        {
            // If dropped in the middle of nowhere, snap back
            ReturnToStart();
        }
    }

    private void ReturnToStart()
    {
        transform.position = startPosition;
    }

    private BinController FindOverlappingBin()
    {
        // Use a small overlap circle to see if we are over a bin's collider
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit != null)
        {
            return hit.GetComponent<BinController>();
        }
        return null;
    }

    void CheckIfMissed()
    {
        BinController[] allBins = FindObjectsOfType<BinController>();

        foreach (BinController bin in allBins)
        {
            if (bin.gameObject.activeInHierarchy && trashType == bin.acceptedTrashType)
            {
                if (UIManager.instance != null)
                {
                    UIManager.instance.ShowFeedbackMessage("Spadło!", Color.red);
                    UIManager.instance.AddScore(-1);
                }
                return; 
            }
        }
    }
}
