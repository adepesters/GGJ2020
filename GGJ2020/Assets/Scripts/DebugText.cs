using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugText : MonoBehaviour
{
    public static List<DebugTextCommand> s_debugTextCommands = new List<DebugTextCommand>();

    public static void Text(Vector3 pos, string text)
    {
        DebugText.s_debugTextCommands.Add(new DebugTextCommand() { Pos = pos, Text = text, BgColor = Color.black } );
    }

    void OnGUI()
    {
        // needed so that the list isn't cleared until stuff is drawn. We don't need layout or events anyway
        if (Event.current.type != EventType.Repaint) {
            return;
        }

        var style = GUI.skin.GetStyle("Label");
        style.fontSize = 18;
        foreach(var item in s_debugTextCommands) {
            var screenPoint = Camera.main.WorldToScreenPoint(item.Pos);
            screenPoint.y = Screen.height - screenPoint.y;
            var size = style.CalcSize(new GUIContent(item.Text));
            var rect = new Rect((Vector2) screenPoint - (size * 0.5f), size);
            var prevCol = GUI.color;
            GUI.color = item.BgColor;
            GUI.DrawTexture(rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, false);
            GUI.color = prevCol;
            GUI.Label(rect, item.Text, style);
        }
    }

    void Update()
    {
        s_debugTextCommands.Clear();
    }
}

public struct DebugTextCommand {
    public string Text;
    public Vector3 Pos;
    public Color BgColor;
}