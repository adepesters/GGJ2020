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

    [SerializeField] GameObject combat;
    [SerializeField] GameObject atelier;
    [SerializeField] GameObject mainRoom;

    public bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }
    public bool IntroDialogOver { get => introDialogOver; set => introDialogOver = value; }
    public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }
    public bool[] LevelCanBeReached { get => levelCanBeReached; set => levelCanBeReached = value; }
    public bool CanLaunchLevel { get => canLaunchLevel; set => canLaunchLevel = value; }

    void Awake()
    {
        //selectedLevel = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //selectedLevel = 0;
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
        Debug.Log(selectedLevel);
    }
}
