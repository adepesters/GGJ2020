using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMainRoom : MonoBehaviour
{
    GameSession gameSession;
    CraftingManager craftingManager;

    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        craftingManager = GameObject.FindWithTag("CraftingManager").GetComponent<CraftingManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LaunchMainRoom()
    {
        // Debug.Log("launch");
        if (craftingManager.RobotSlot[0] != -1 &&
        craftingManager.RobotSlot[1] != -1 &&
            craftingManager.RobotSlot[2] != -1)
        {
            gameSession.CanLaunchLevel = true;
            audioSource.Stop();

        }
    }

}
