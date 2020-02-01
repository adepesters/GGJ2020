using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Robot {
    public int x;
    public int y;
    public int current_hp;
    public RobotView view;
}

public class Combat : MonoBehaviour
{
    public Transform _grassTile;
    public Transform _rock;
    public Transform _selectionFrame;
    public Transform _robotTemplate;

    const int board_size = 8;
    const int tile_count = board_size * board_size;
    public SpriteRenderer[] _selectionFrames = new SpriteRenderer[tile_count];

    const float perspective_ratio = 0.31f;
    const float offset_w = 368f/4f;
    const float offset_h = offset_w * perspective_ratio;
    Vector2 offset_x = new Vector2(offset_w, -offset_h);
    Vector2 offset_y = new Vector2(offset_w, offset_h);

    public List<Robot> _robots = new List<Robot>();

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        _grassTile.gameObject.SetActive(false);
        _rock.gameObject.SetActive(false);
        _selectionFrame.gameObject.SetActive(false);

        Vector2 origin = new Vector2(-offset_w * (board_size - 1), 0f);
        for (int y = 0; y < board_size; y++) {
            for (int x = 0; x < board_size; x++) {
                Vector2 pos = origin + offset_x * x + offset_y * y;
                var tile = Instantiate(_grassTile, transform);
                tile.transform.localPosition = pos;
                tile.gameObject.SetActive(true);

                if (Random.value < 0.2f) {
                    var rock = Instantiate(_rock, transform);
                    rock.transform.localPosition = pos;
                    rock.gameObject.SetActive(true);
                }
                var selectionFrame = Instantiate(_selectionFrame, transform);
                selectionFrame.transform.localPosition = pos;
                selectionFrame.gameObject.SetActive(true);

                _selectionFrames[y * board_size + x] = selectionFrame.GetComponent<SpriteRenderer>();
            }
        }

        for (int i = 0; i < _robots.Count; i++) {
            var robot = _robots[i];
            var view = robot.view;
        }
    }

    void Update()
    {
        Vector3 screen_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screen_pos.z = 0.0f;
        Vector3 tiles_origin_pos = new Vector3(-offset_w * (board_size - 1), 0f, 0.0f);
        Vector3 delta_pos = screen_pos - tiles_origin_pos;
        var div_x = delta_pos / offset_x;
        var div_y = delta_pos / offset_y;
        int hovered_x = Mathf.RoundToInt((div_x.x + div_x.y) * 0.5f);
        int hovered_y = Mathf.RoundToInt((div_y.x + div_y.y) * 0.5f);
        int hovered_tile = -1; // -1 means no tile is hovered
        if (hovered_x >= 0 && hovered_y >= 0 && hovered_x < board_size && hovered_y < board_size) {
            hovered_tile = hovered_y * board_size + hovered_x;
        }

        
        print("hovered " + hovered_tile);

        for (int i = 0, y = 0; y < board_size; y++) {
            for (int x = 0; x < board_size; x++, i++) {
                var frame = _selectionFrames[i];
                frame.enabled = hovered_tile == i;
            }
        }

        DebugText.Text(screen_pos, $"{hovered_x} ; {hovered_y}");
        Debug.DrawLine(tiles_origin_pos, tiles_origin_pos + delta_pos, Color.red);
    }

    // void DrawScreenLine(Vector3 a, Vector3 b)
    // {
    //     Debug.DrawLine(Camera.main.(a), Camera.main.WorldToScreenPoint(b), Color.red);
    // }
}
