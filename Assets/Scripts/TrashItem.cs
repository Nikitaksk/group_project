using UnityEngine;
using UnityEngine.EventSystems;

public class TrashItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
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
    private bool isDragging;
    private bool startPositionSet;
    private const float DestroyY = -10f;
    private const float BinProbeRadius = 0.2f;

    public bool IsDragging => isDragging;

    void Start()
    {
        if (!startPositionSet)
        {
            startPosition = transform.position;
            startPositionSet = true;
        }
    }

    void Update()
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
            return;

        if (transform.position.y < DestroyY)
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
        if (IsRoomCleaningBlocked())
            return;

        isDragging = true;
        Vector3 pointerWorld = ScreenToWorld(eventData.position);
        dragOffset = transform.position - pointerWorld;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = ScreenToWorld(eventData.position) + dragOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        BinController bin = FindOverlappingBin();
        if (bin == null)
        {
            ReturnToStart();
            return;
        }

        if (bin.acceptedTrashType == trashType)
            HandleCorrectBin();
        else
            HandleWrongBin();
    }

    void HandleCorrectBin()
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
            UIManager.instance.HandleRoomCleaningResult(true, trashType);
        else
        {
            UIManager.instance.ShowFeedbackMessage("Dobrze!", Color.green);
            UIManager.instance.AddScore(1);
        }

        Destroy(gameObject);
    }

    void HandleWrongBin()
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
            UIManager.instance.HandleRoomCleaningResult(false, trashType);
        else
        {
            UIManager.instance.ShowFeedbackMessage("Zły Kosz!", Color.red);
            UIManager.instance.AddScore(-1);
            UIManager.instance.AddMistake(trashType);
        }

        ReturnToStart();
    }

    bool IsRoomCleaningBlocked()
    {
        return GameData.CurrentGameMode == GameData.GameMode.MultiBin
            && UIManager.instance != null
            && UIManager.instance.IsGameOver;
    }

    static Vector3 ScreenToWorld(Vector2 screenPosition)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(screenPosition);
        world.z = 0f;
        return world;
    }

    void ReturnToStart()
    {
        transform.position = startPosition;
    }

    BinController FindOverlappingBin()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, BinProbeRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            BinController bin = hit.GetComponent<BinController>();
            if (bin != null)
                return bin;
        }

        return null;
    }

    void CheckIfMissed()
    {
        foreach (BinController bin in FindObjectsOfType<BinController>())
        {
            if (!bin.gameObject.activeInHierarchy || trashType != bin.acceptedTrashType)
                continue;

            if (UIManager.instance != null)
            {
                UIManager.instance.ShowFeedbackMessage("Spadło!", Color.red);
                UIManager.instance.AddScore(-1);
            }

            return;
        }
    }
}
