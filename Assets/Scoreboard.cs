using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    public Transform roomHolder;
    public GameObject scoreDoorPrefab;

    private void Start()
    {
        Scoring();
    }

    public void Scoring()
    {
        Room[] rooms = GameManager.Instance.GetRooms();

        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsOccupied)
            {
                GameObject scoreRoom = Instantiate(scoreDoorPrefab, roomHolder);
                var scoreDoor = scoreRoom.GetComponent<ScoreDoor>();
                scoreDoor.SetRoomInfo(rooms[i]);
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
