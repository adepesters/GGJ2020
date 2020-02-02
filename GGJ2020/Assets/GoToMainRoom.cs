using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMainRoom : MonoBehaviour
{
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LaunchMainRoom()
    {
        // Debug.Log("launch");
        gameSession.CanLaunchLevel = true;
    }

}
