using UnityEngine;
using System.Collections.Generic;

public class RoomCleanerManager : MonoBehaviour
{
    [Header("Cleaning Mode Assets")]
    [Tooltip("Drag all trash items from the scene that should be part of the cleaning mode here.")]
    public TrashItem[] trashItemsInRoom;

    [Header("Testing")]
    [Tooltip("If true, it will run setup even if the GameMode isn't set to MultiBin (useful for testing scene directly).")]
    public bool forceInitialization = true;

    private List<TrashItem> managedItems = new List<TrashItem>();

    void Start()
    {
        Debug.Log($"RoomCleanerManager: Start called. Current Mode: {GameData.CurrentGameMode}");

        if (forceInitialization || GameData.CurrentGameMode == GameData.GameMode.MultiBin)
        {
            SetupRoomCleaning();
        }
    }

    [ContextMenu("Setup Cleaning Mode")]
    public void SetupRoomCleaning()
    {
        managedItems.Clear();

        if (trashItemsInRoom == null || trashItemsInRoom.Length == 0)
        {
            Debug.LogWarning("RoomCleanerManager: No trash items assigned in the array! Initialization stopped.");
            return;
        }

        // Check for Raycaster on Camera (required for dragging)
        if (Camera.main != null && Camera.main.GetComponent<UnityEngine.EventSystems.Physics2DRaycaster>() == null)
        {
            Debug.LogWarning("RoomCleanerManager: Main Camera is missing a Physics2DRaycaster. Dragging will not work!");
        }

        foreach (TrashItem item in trashItemsInRoom)
        {
            if (item == null) continue;

            // 1. Save their exact position as their "start position"
            item.SetStartPosition(item.transform.position);
            managedItems.Add(item);

            // 2. Ensure they have a collider for dragging
            Collider2D col = item.GetComponent<Collider2D>();
            if (col == null)
            {
                col = item.gameObject.AddComponent<BoxCollider2D>();
                Debug.Log($"RoomCleanerManager: Added BoxCollider2D to {item.name}");
            }


            // 3. Force Rigidbody to Kinematic so they don't move unless dragged
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        // 4. Disable any spawners
        ObjectSpawner[] spawners = FindObjectsOfType<ObjectSpawner>();
        foreach (var spawner in spawners)
        {
            spawner.enabled = false;
        }

        Debug.Log($"RoomCleanerManager: Successfully initialized {managedItems.Count} items.");
    }
}
