using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public Animator panelAnimator;

    public void OpenModeSelection()
    {
        panelAnimator.SetTrigger("Open");
    }

    public void CloseModeSelection()
    {
        panelAnimator.SetTrigger("Close");
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
