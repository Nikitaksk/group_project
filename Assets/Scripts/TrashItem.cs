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
        Metal
    }

    // This variable will hold the type for each specific trash prefab.
    // You will set this in the Inspector for each of your prefabs.
    public TrashType trashType;
}
