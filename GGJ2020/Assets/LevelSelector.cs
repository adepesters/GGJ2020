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

    }

    private void OnMouseDown()
    {
        gameSession.SelectedLevel = levelID;
    }

    private void OnMouseOver()
    {
        colorOverlay.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 200);
    }

    private void OnMouseExit()
    {
        colorOverlay.GetComponent<Image>().color = new Color(originalColor.r, originalColor.g, originalColor.b, 100);
    }
}
