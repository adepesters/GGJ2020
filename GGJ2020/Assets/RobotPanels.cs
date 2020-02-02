using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPanels : MonoBehaviour
{
    [SerializeField] Transform _panel_template;

    void OnEnable()
    {
        _panel_template.gameObject.SetActive(false);

        // destroy all children exept the first (which is the template), going backwards (to avoid changing the children indices)
        for(int i = transform.childCount - 1; i > 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }

        var gameDAta = Resources.Load<GameData>("GameData");

        print($"game data, {gameDAta.Definitions.Length} definitions");

        for (int i = 0; i < gameDAta.Definitions.Length; i++) {
            var def = gameDAta.Definitions[i];
            var panel = Instantiate(_panel_template, _panel_template.parent);
            panel.gameObject.SetActive(true);
            var dnd = panel.GetComponentInChildren<DragAndDrop>();
            dnd.robotSprite = def.IdleAnim[0];
            dnd.robotIndex = i;
            dnd.GetComponent<UnityEngine.UI.Image>().sprite = dnd.robotSprite;
        }
    }
}
