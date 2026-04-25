using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject modeSelectPanel;

    public void OpenModeSelection()
    {
        modeSelectPanel.SetActive(true);
    }

    public void CloseModeSelection()
    {
        modeSelectPanel.SetActive(false);
    }

    public void LoadDropper(int binIndex)
    {
        GameData.CurrentGameMode = GameData.GameMode.SingleBin;
        GameData.SelectedBinIndex = binIndex;

        SceneManager.LoadScene("Dropper");
    }

    public void LoadCleanTheRoom()
    {
        GameData.CurrentGameMode = GameData.GameMode.MultiBin;
        SceneManager.LoadScene("CleanTheRoom");
    }
}
