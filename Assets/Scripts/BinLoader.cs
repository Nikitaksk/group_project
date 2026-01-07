using UnityEngine;

public class BinLoader : MonoBehaviour
{
    [Header("Array of Bins")]
    public GameObject[] bins;

    void Start()
    {
        // get index from static variable
        int indexToActivate = GameData.SelectedBinIndex;

        if (indexToActivate < bins.Length && indexToActivate >= 0)
        {
            bins[indexToActivate].SetActive(true);

            Debug.Log("Activated bin: " + indexToActivate);
        }
        else
        {
            Debug.LogError("Error while activating bin!");
        }
    }
}