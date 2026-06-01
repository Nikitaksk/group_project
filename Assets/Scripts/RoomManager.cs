using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomContainers;

    void Start()
    {
        if (roomContainers.Length > 0)
        {
            int roomID = PlayerPrefs.GetInt("SelectedRoom", 0);
            for (int i = 0; i < roomContainers.Length; i++)
            {
                roomContainers[i].SetActive(i == roomID);
            }
        }
    }

    public void SelectRoom(int roomIndex)
    {
        PlayerPrefs.SetInt("SelectedRoom", roomIndex);
        SceneManager.LoadScene("CleanTheRoom");
    }
}