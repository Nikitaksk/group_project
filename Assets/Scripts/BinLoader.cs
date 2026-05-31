using UnityEngine;

public class BinLoader : MonoBehaviour
{
    public GameObject[] bins;

    void Start()
    {
        if (GameData.CurrentGameMode == GameData.GameMode.MultiBin)
        {
            SetAllBinsActive(true);
            return;
        }

        int index = GameData.SelectedBinIndex;
        if (index < 0 || index >= bins.Length)
        {
            Debug.LogError("BinLoader: Invalid bin index " + index);
            return;
        }

        SetAllBinsActive(false);
        bins[index].SetActive(true);
    }

    void SetAllBinsActive(bool active)
    {
        foreach (GameObject bin in bins)
        {
            if (bin != null)
                bin.SetActive(active);
        }
    }
}
