using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    bool gameHasStarted;
    bool introDialogOver;

    int selectedLevel = -1;
    bool[] levelCanBeReached = new bool[10];

    bool canLaunchLevel;
    bool canLaunchMainRoom;
    bool canLaunchAtelier;

    [SerializeField] GameObject combat;
    [SerializeField] GameObject atelier;
    [SerializeField] GameObject mainRoom;

    public bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }
    public bool IntroDialogOver { get => introDialogOver; set => introDialogOver = value; }
    public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }
    public bool[] LevelCanBeReached { get => levelCanBeReached; set => levelCanBeReached = value; }
    public bool CanLaunchLevel { get => canLaunchLevel; set => canLaunchLevel = value; }
    public bool CanLaunchMainRoom { get => canLaunchMainRoom; set => canLaunchMainRoom = value; }
    public bool CanLaunchAtelier { get => canLaunchAtelier; set => canLaunchAtelier = value; }

    void Awake()
    {
        //selectedLevel = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //selectedLevel = 0;
        levelCanBeReached[0] = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (introDialogOver)
        {
            introDialogOver = false;
            levelCanBeReached[0] = true;
        }

        if (CanLaunchLevel)
        {
            CanLaunchLevel = false;
            combat.SetActive(true);
            atelier.SetActive(false);
            mainRoom.SetActive(false);
        }

        if (canLaunchMainRoom)
        {
            canLaunchMainRoom = false;
            combat.SetActive(false);
            atelier.SetActive(false);
            mainRoom.SetActive(true);
        }

        if (canLaunchAtelier)
        {
            canLaunchAtelier = false;
            combat.SetActive(false);
            atelier.SetActive(true);
            mainRoom.SetActive(false);
        }


        Debug.Log(selectedLevel);
    }
}
