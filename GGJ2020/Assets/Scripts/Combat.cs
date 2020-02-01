using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Actor {
    public int x;
    public int y;
    public int current_hp;
    public int movement_range;
}

public struct Robot {
    public Actor actor;
    public RobotView view;
}

public struct Rock {
    public Actor actor;
    public Transform view;
}

public enum UiMode {
    Select,
    Move,
    Attack,
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

    public List<int[]> _reachable = new List<int[]>();
    public List<int[]> _attackable = new List<int[]>();

    public int _selected_robot_index = 0;

    public UiMode _uiMode = UiMode.Select;

    public Button _move_button;
    public Button _action_button;

    public Transform tiles_container;

    void Start()
    {
        _robotTemplate.gameObject.SetActive(false);
        AddRobot(3, 1, 3);
        AddRobot(1, 3, 2);
        GenerateTerrain();

        _move_button.onClick.AddListener(() => {
            _uiMode = UiMode.Move;
        });

        _action_button.onClick.AddListener(() => {
            _uiMode = UiMode.Attack;
        });
    }

    void AddRobot(int x, int y, int movement_range)
    {
        Robot robot = new Robot();
        robot.actor.x = x;
        robot.actor.y = y;
        robot.actor.movement_range = movement_range;
        robot.view = Instantiate(_robotTemplate, tiles_container);
        robot.view.gameObject.SetActive(true);
        _robots.Add(robot);

        _reachable.Add(new int[tile_count]);
        _attackable.Add(new int[tile_count]);
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
                var tile = Instantiate(_grassTile, tiles_container);
                tile.transform.localPosition = pos;
                tile.gameObject.SetActive(true);

                if (Random.value < 0.2f) {
                    var rockView = Instantiate(_rock, tiles_container);
                    rockView.transform.localPosition = pos;
                    rockView.gameObject.SetActive(true);
                    var rock = new Rock();
                    rock.actor.x = x;
                    rock.actor.y = y;
                    rock.view = rockView;
                    _rocks.Add(rock);
                }
                var selectionFrame = Instantiate(_selectionFrame, tiles_container);
                selectionFrame.transform.localPosition = pos;
                selectionFrame.gameObject.SetActive(true);

                _selectionFrames[y * board_size + x] = selectionFrame.GetComponent<SpriteRenderer>();
            }
        }
    }

    void DiscoverTile(int[] _distances, int tile_index, int distance)
    {
        if (_distances[tile_index] > distance) {
            _distances[tile_index] = distance;

            distance++;

            //
            // Discover neighbours
            //
            if (tile_index % board_size != 0) {
                var left = tile_index - 1;
                if (IsTileFree(left)) {
                    DiscoverTile(_distances, left, distance);
                }
            }

            if (tile_index % board_size != board_size - 1) {
                var right = tile_index + 1;
                if (IsTileFree(right)) {
                    DiscoverTile(_distances, right, distance);
                }
            }

            if (tile_index >= board_size) {
                var bottom = tile_index - board_size;
                if (IsTileFree(bottom)) {
                    DiscoverTile(_distances, bottom, distance);
                }
            }

            if (tile_index < tile_count - board_size) {
                var top = tile_index + board_size;
                if (IsTileFree(top)) {
                    DiscoverTile(_distances, top, distance);
                }
            }
        }

        
    }

    void Update()
    {
        DebugText.Text(Vector3.zero, _uiMode.ToString());

        //
        // Find out which tile the cursor is on
        //
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

        //
        // Update reachable tiles for each robot
        //
        for (int i = 0; i < _robots.Count; i++) {
            var actor = _robots[i].actor;
            var reachable = _reachable[i];
            
            // clear reachable table
            for (int j = 0; j < reachable.Length; j++) {
                reachable[j] = 1000; // init with a crazy high val
            }

            DiscoverTile(reachable, CoordsToIndex(actor.x, actor.y), 0);
        }

        //
        // Update attackable tiles
        //
        for (int robot_index = 0; robot_index < _robots.Count; robot_index++) {
            var attackable_table = _attackable[robot_index];
            var robot = _robots[robot_index];

            for (int tile_index = 0, y = 0; y < board_size; y++) {
                for (int x = 0; x < board_size; x++, tile_index++) {
                    attackable_table[tile_index] = (x == robot.actor.x || y == robot.actor.y) ? 1 : 0; 
                }
            }
        }

        //
        // Update tiles highlights
        //
        for (int tile_index = 0, y = 0; y < board_size; y++) {
            for (int x = 0; x < board_size; x++, tile_index++) {
                var frame = _selectionFrames[tile_index];
                var tile_in_range = InRange(_robots[_selected_robot_index].actor, x, y, 2);
                var reachable_table = _reachable[_selected_robot_index];
                var attackable_table = _attackable[_selected_robot_index];
                var selected_robot = _robots[_selected_robot_index];

                DebugText.Text(GetTilePos(x,y), reachable_table[tile_index].ToString());

                var color = Color.clear;
                var reachable = reachable_table[tile_index] <= selected_robot.actor.movement_range;
                var attackable = attackable_table[tile_index] != 0;
                if (_uiMode == UiMode.Move && reachable) {
                    color = Color.green;
                }
                if (_uiMode == UiMode.Attack && attackable) {
                    color = Color.yellow;
                }
                if (hovered_tile == tile_index) {
                    color = Color.white;
                    if (!hovered_tile_is_free) {
                        color = Color.red;
                    }
                    if (_uiMode == UiMode.Move && !reachable) {
                        color = Color.red;
                    }
                }

                frame.color = color;
                //frame.enabled = color;
            }
        }

        //
        // Input select robot
        //
        if (_uiMode == UiMode.Select) {
            if (hovered_tile >= 0 && Input.GetMouseButtonDown(0)) {
                for (int i = 0; i < _robots.Count; i++) {
                    var actor  =_robots[i].actor;
                    int robot_tile = CoordsToIndex(actor.x, actor.y);
                    if (robot_tile == hovered_tile) {
                        _selected_robot_index = i;
                    }
                }
                _uiMode = UiMode.Select;
            }
        }

        //
        // Input robot movement
        //
        if (_uiMode == UiMode.Move) {
            if (hovered_tile >= 0 && Input.GetMouseButtonDown(0) && hovered_tile_is_free) {
                var robot = _robots[_selected_robot_index];
                robot.actor.x = hovered_x;
                robot.actor.y = hovered_y;
                _robots[_selected_robot_index] = robot;
                _uiMode = UiMode.Select;
            }
        }

        //
        // Input attack
        //
        if (_uiMode == UiMode.Attack) {
            // if (hovered_tile >= 0 && Input.GetMouseButtonDown(0) && hovered_tile_is_free) {
            //     var robot = _robots[_selected_robot_index];
            //     robot.actor.x = hovered_x;
            //     robot.actor.y = hovered_y;
            //     _robots[_selected_robot_index] = robot;
            //     _uiMode = UiMode.Select;
            // }
        }

        //
        // Robot update visuals
        //
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

    bool InRange(Actor actor, int tile_x, int tile_y, int move_range)
    {
        var delta_x = Mathf.Abs(actor.x - tile_x);
        var delta_y = Mathf.Abs(actor.y - tile_y);

        return delta_x + delta_y <= move_range;
    }

    // void DrawScreenLine(Vector3 a, Vector3 b)
    // {
    //     Debug.DrawLine(Camera.main.(a), Camera.main.WorldToScreenPoint(b), Color.red);
    // }
}
