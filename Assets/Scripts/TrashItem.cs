using UnityEngine;

public class TrashItem : MonoBehaviour
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

    // This variable will hold the type for each specific trash prefab.
    // You will set this in the Inspector for each of your prefabs.
    public TrashType trashType;

    private float destroyY = -5f;

   void Update()
    {
        if (transform.position.y < destroyY)
        {
            CheckIfMissed();
            Destroy(gameObject);
        }
    }

    void CheckIfMissed()
    {
        BinController activeBin = FindObjectOfType<BinController>();

        if (activeBin != null)
        {
            if (trashType == activeBin.acceptedTrashType)
            {
                if (UIManager.instance != null)
                {
                    UIManager.instance.ShowFeedbackMessage("Spadło!", Color.red);
                    UIManager.instance.AddScore(-1);
                }
            }
        }
    }
}
