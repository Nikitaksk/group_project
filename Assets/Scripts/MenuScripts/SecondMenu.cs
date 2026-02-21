using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class SecondMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject modeSelectPanel;

    public TransitionSettings transition;
    public float loadDelay;


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

        TransitionManager.Instance().Transition("Dropper", transition, loadDelay);
    }
}
