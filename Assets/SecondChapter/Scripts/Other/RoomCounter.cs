using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCounter : DefaultClass
{
    private void Start()
    {
        int roomCount = PlayerPrefs.GetInt("room_count", 0);
        PlayerPrefs.SetInt("room_count", roomCount + 1);
    }
}
