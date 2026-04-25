using UnityEngine;

public class BinLoader : MonoBehaviour
{
    [Header("Array of Bins")]
    public GameObject[] bins;

    void Start()
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
        {
            foreach (GameObject bin in bins)
            {
                if (bin != null) bin.SetActive(true);
            }
            Debug.Log("Activated all bins for MultiBin mode.");
        }
        else
        {
            // get index from static variable
            int indexToActivate = GameData.SelectedBinIndex;

            if (indexToActivate < bins.Length && indexToActivate >= 0)
            {
                // Deactivate all first (optional but safer if some are pre-active)
                foreach (GameObject bin in bins)
                {
                    if (bin != null) bin.SetActive(false);
                }

                bins[indexToActivate].SetActive(true);
                Debug.Log("Activated bin: " + indexToActivate);
            }
            else
            {
                Debug.LogError("Error while activating bin!");
            }
        }
    }
}