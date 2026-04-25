using UnityEngine;

public static class GameData
{
    public enum GameMode
    {
        SingleBin,
        MultiBin
    }

    public static int SelectedBinIndex = 0;
    public static GameMode CurrentGameMode = GameMode.SingleBin;
}