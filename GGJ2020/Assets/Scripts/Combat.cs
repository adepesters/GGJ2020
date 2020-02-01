using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Actor {
    public int x;
    public int y;
    public int current_hp;
}

public struct Robot {
    public Actor actor;
    public RobotView view;
}

public struct Rock {
    public Actor actor;
    public Transform view;
}

public class Combat : MonoBehaviour
{
    public Transform _grassTile;
    public Transform _rock;
    public Transform _selectionFrame;
    public RobotView _robotTemplate;

    const int board_size = 8;
    const int tile_count = board_size * board_size;
    public SpriteRenderer[] _selectionFrames = new SpriteRenderer[tile_count];

    const float perspective_ratio = 0.31f;
    const float offset_w = 368f/4f;
    const float offset_h = offset_w * perspective_ratio;
    Vector2 offset_x = new Vector2(offset_w, -offset_h);
    Vector2 offset_y = new Vector2(offset_w, offset_h);

    public List<Robot> _robots = new List<Robot>();
    public List<Rock> _rocks = new List<Rock>();

    public int _selected_robot = 0;

    void Start()
    {
        _robotTemplate.gameObject.SetActive(false);
        AddRobot(3, 1);
        GenerateTerrain();
    }

    void AddRobot(int x, int y)
    {
        Robot robot = new Robot();
        robot.actor.x = x;
        robot.actor.y = y;
        robot.view = Instantiate(_robotTemplate, transform);
        robot.view.gameObject.SetActive(true);
        _robots.Add(robot);
    }

    Vector2 GetTilePos(int x, int y)
    {
        Vector2 origin = new Vector2(-offset_w * (board_size - 1), 0f);
        Vector2 pos = origin + offset_x * x + offset_y * y;
        return pos;
    }

    void GenerateTerrain()
    {
        _grassTile.gameObject.SetActive(false);
        _rock.gameObject.SetActive(false);
        _selectionFrame.gameObject.SetActive(false);

        Vector2 origin = new Vector2(-offset_w * (board_size - 1), 0f);
        for (int y = 0; y < board_size; y++) {
            for (int x = 0; x < board_size; x++) {
                Vector2 pos = GetTilePos(x, y);
                var tile = Instantiate(_grassTile, transform);
                tile.transform.localPosition = pos;
                tile.gameObject.SetActive(true);

                if (Random.value < 0.2f) {
                    var rockView = Instantiate(_rock, transform);
                    rockView.transform.localPosition = pos;
                    rockView.gameObject.SetActive(true);
                    var rock = new Rock();
                    rock.actor.x = x;
                    rock.actor.y = y;
                    rock.view = rockView;
                    _rocks.Add(rock);
                }
                var selectionFrame = Instantiate(_selectionFrame, transform);
                selectionFrame.transform.localPosition = pos;
                selectionFrame.gameObject.SetActive(true);

                _selectionFrames[y * board_size + x] = selectionFrame.GetComponent<SpriteRenderer>();
            }
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

        bool hovered_tile_is_free = IsTileFree(hovered_tile);

        for (int i = 0, y = 0; y < board_size; y++) {
            for (int x = 0; x < board_size; x++, i++) {
                var frame = _selectionFrames[i];
                frame.enabled = hovered_tile == i;
                frame.color = hovered_tile_is_free ? Color.white : Color.red;
            }
        }

        DebugText.Text(screen_pos, $"{hovered_x} ; {hovered_y}");
        Debug.DrawLine(tiles_origin_pos, tiles_origin_pos + delta_pos, Color.red);


        if (hovered_tile >= 0 && Input.GetMouseButtonDown(0) && hovered_tile_is_free) {
            var robot = _robots[_selected_robot];
            robot.actor.x = hovered_x;
            robot.actor.y = hovered_y;
            _robots[_selected_robot] = robot;
        }

        for (int i = 0; i < _robots.Count; i++) {
            var robot = _robots[i];
            var view = robot.view;

            view.transform.position = GetTilePos(robot.actor.x, robot.actor.y);
        }
    }

    bool IsTileFree(int index)
    {
        for(int i = 0; i < _rocks.Count; i++) {
            var actor = _rocks[i].actor;
            if (CoordsToIndex(actor.x, actor.y) == index) {
                return false;
            }
        }
        for(int i = 0; i < _robots.Count; i++) {
            var actor = _robots[i].actor;
            if (CoordsToIndex(actor.x, actor.y) == index) {
                return false;
            }
        }
        return true;
    }

    int CoordsToIndex(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < board_size && y < board_size) {
            return y * board_size + x;
        } else {
            return -1; // outside game board
        }
    }

    // void DrawScreenLine(Vector3 a, Vector3 b)
    // {
    //     Debug.DrawLine(Camera.main.(a), Camera.main.WorldToScreenPoint(b), Color.red);
    // }
}
