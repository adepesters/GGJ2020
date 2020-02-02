using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    GameSession gameSession;
    int levelID;
    [SerializeField] GameObject colorOverlay;
    Color originalColor;
    [SerializeField] int[] nextAvailableLevels;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
        levelID = int.Parse(Regex.Replace(this.gameObject.name, "[^0-9]", ""));
        originalColor = colorOverlay.GetComponent<Image>().color;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.LevelCanBeReached[levelID])
        {
            colorOverlay.GetComponent<Image>().enabled = true;
        }
        else
        {
            colorOverlay.GetComponent<Image>().enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (gameSession.LevelCanBeReached[levelID])
        {
            gameSession.SelectedLevel = levelID;
        }
    }

    private void OnMouseOver()
    {
        if (gameSession.LevelCanBeReached[levelID])
        {
            colorOverlay.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
        }
    }

    private void OnMouseExit()
    {
        if (gameSession.LevelCanBeReached[levelID])
        {
            colorOverlay.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
        }
    }
}
