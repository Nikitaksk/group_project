using UnityEngine;
using UnityEngine.EventSystems;

public class RoomCleanerManager : MonoBehaviour
{
    [Header("Cleaning Mode Assets")]
    [Tooltip("Drag all trash items from the scene that should be part of the cleaning mode here.")]
    public TrashItem[] trashItemsInRoom;

    [Header("Testing")]
    [Tooltip("If true, setup runs even when opening the scene directly from the editor.")]
    public bool forceInitialization = true;

    void Start()
    {
        if (forceInitialization || GameData.CurrentGameMode == GameData.GameMode.MultiBin)
            SetupRoomCleaning();
    }

    [ContextMenu("Setup Cleaning Mode")]
    public void SetupRoomCleaning()
    {
        GameData.CurrentGameMode = GameData.GameMode.MultiBin;

        if (trashItemsInRoom == null || trashItemsInRoom.Length == 0)
        {
            Debug.LogWarning("RoomCleanerManager: No trash items assigned.");
            return;
        }

        if (Camera.main != null && Camera.main.GetComponent<Physics2DRaycaster>() == null)
            Debug.LogWarning("RoomCleanerManager: Main Camera needs a Physics2DRaycaster for dragging.");

        int initializedCount = 0;

        foreach (TrashItem item in trashItemsInRoom)
        {
            if (item == null)
                continue;

            item.SetStartPosition(item.transform.position);

            if (item.GetComponent<Collider2D>() == null)
                item.gameObject.AddComponent<BoxCollider2D>();

            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            initializedCount++;
        }

        foreach (ObjectSpawner spawner in FindObjectsOfType<ObjectSpawner>())
            spawner.enabled = false;

        if (UIManager.instance != null)
            UIManager.instance.ConfigureForRoomCleaning();

        Debug.Log($"RoomCleanerManager: Initialized {initializedCount} items.");
    }
}
