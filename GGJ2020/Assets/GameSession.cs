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
    bool canLaunchGameOver;

    [SerializeField] GameObject combat;
    [SerializeField] GameObject atelier;
    [SerializeField] GameObject mainRoom;
    [SerializeField] GameObject death;

    public bool GameHasStarted { get => gameHasStarted; set => gameHasStarted = value; }
    public bool IntroDialogOver { get => introDialogOver; set => introDialogOver = value; }
    public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }
    public bool[] LevelCanBeReached { get => levelCanBeReached; set => levelCanBeReached = value; }
    public bool CanLaunchLevel { get => canLaunchLevel; set => canLaunchLevel = value; }
    public bool CanLaunchMainRoom { get => canLaunchMainRoom; set => canLaunchMainRoom = value; }
    public bool CanLaunchAtelier { get => canLaunchAtelier; set => canLaunchAtelier = value; }
    public bool CanLaunchGameOver { get => canLaunchGameOver; set => canLaunchGameOver = value; }

    void Awake()
    {
        //selectedLevel = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //selectedLevel = 0;
        levelCanBeReached[0] = true;

        combat.SetActive(false);
        atelier.SetActive(false);
        mainRoom.SetActive(true);
        death.SetActive(false);
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
            death.SetActive(false);
        }

        if (canLaunchMainRoom)
        {
            canLaunchMainRoom = false;

            StartCoroutine(MainRoomDelayed());
        }

        if (canLaunchAtelier)
        {
            canLaunchAtelier = false;
            combat.SetActive(false);
            atelier.SetActive(true);
            mainRoom.SetActive(false);
            death.SetActive(false);
        }

        if (canLaunchGameOver)
        {
            StartCoroutine(RelaunchGame());
        }

    }

    IEnumerator MainRoomDelayed()
    {
        var manager = FindObjectOfType<CraftingManager>();
        manager.CrystalsAmount[0] += 1;
        manager.CrystalsAmount[1] += 0;
        manager.CrystalsAmount[2] += 0;
        manager.CrystalsAmount[3] += 1;

        yield return new WaitForSeconds(1.0f);

        combat.SetActive(false);
        atelier.SetActive(false);
        mainRoom.SetActive(true);
        death.SetActive(false);
    }

    IEnumerator RelaunchGame()
    {
        yield return new WaitForSeconds(0.5f);
        canLaunchGameOver = false;
        combat.SetActive(false);
        atelier.SetActive(false);
        mainRoom.SetActive(false);
        death.SetActive(true);
        levelCanBeReached[0] = true;
        yield return new WaitForSeconds(4);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

}
