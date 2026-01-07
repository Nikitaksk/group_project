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
        GameData.SelectedBinIndex = binIndex;

        SceneManager.LoadScene("Dropper");
    }
}
