using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public struct Plan
{
    public int move_to_tile;
    public int attack_group;
}

public struct Actor
{
    public int x;
    public int y;
    public int current_hp;
    public int max_hp;
    public int movement_range;
    public RobotView view;
    public List<int> plop;
    public bool friend;
    public Plan plan;
}

public struct Robot
{
    public Actor actor;
}

public struct Rock
{
    public Actor actor;
    public Transform view;
}

public enum UiMode
{
    Select,
    Move,
    Attack,
}

public class Combat : MonoBehaviour
{
    public Transform _grassTile;
    public Transform _waterTile;
    public Transform _rock;
    public Transform _column;
    public Transform _goal;
    public Transform _selectionFrame;
    public RobotView _robotTemplate;
    public RobotView _enemyTemplate;

    int _board_size;
    int _tile_count;
    private SpriteRenderer[] _selectionFrames;

    const float perspective_ratio = 0.31f;
    const float offset_w = 368f / 4f;
    const float offset_h = offset_w * perspective_ratio;
    Vector2 offset_x = new Vector2(offset_w, -offset_h);
    Vector2 offset_y = new Vector2(offset_w, offset_h);

    public List<Actor> _robots = new List<Actor>();
    public List<Rock> _rocks = new List<Rock>();

    public List<int[]> _reachable = new List<int[]>();
    public List<int[]> _attackable = new List<int[]>();

    public int _selected_robot_index = 0;

    public UiMode _uiMode = UiMode.Select;

    public Button _move_button;
    public Button _action_button;
    public Button _end_turn_button;


    public Transform tiles_container;

    public bool _battle_won = false;

    private Actor _goal_actor;

    const char GRASS = '.';
    const char ROCK = 'O';
    const char SPAWN = '+';
    const char GOAL = '^';
    const char WATER = '_';
    const char COLUMN = 'I';
    const char ENEMY = 'X';

    // const string level_data =
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRR......RRR" + 
    // "RRRRRSSRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR" + 
    // "RRRRRRRRRRRR";


    string[] _levels = new string[]
    {
        "OOOOOOOO" +
        "O...^..O" +
        "O....X.O" +
        "___..___" +
        "OI....IO" +
        "O..+...O" +
        "O.+..+.O" +
        "OOOOOOOO",

        "OOOOOOOO" +
        "O..X^.XO" +
        "O....I.O" +
        "___..___" +
        "OI....IO" +
        "O..+.XXO" +
        "O.+X.+.O" +
        "OOOOOOOO"

    };

    string _level_data;

    GameSession gameSession;

    void OnEnable()
    {
        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();

        _robotTemplate.gameObject.SetActive(false);
        _enemyTemplate.gameObject.SetActive(false);

        GenerateTerrain(gameSession.SelectedLevel);

        _move_button.onClick.AddListener(() =>
        {
            if (_selected_robot_index >= 0 && _robots[_selected_robot_index].current_hp > 0 && !_battle_won)
            {
                _uiMode = UiMode.Move;
            }
        });

        _action_button.onClick.AddListener(() =>
        {
            if (_selected_robot_index >= 0 && _robots[_selected_robot_index].current_hp > 0 && !_battle_won)
            {
                _uiMode = UiMode.Attack;
            }
        });

        _end_turn_button.onClick.AddListener(() =>
        {
            EnemiesPlan();
        });
    }

    public void NextTurn()
    {
        //EnemiesExecute();
        EnemiesPlan();
    }

    void EnemiesPlan()
    {
        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            if (robot.friend)
            {
                continue;
            }

            // we're dealing with an enemy
            var reachable_table = _reachable[i];
            // int best_move_score = -10000;
            // int best_move_tile = -1;
            // int best_move_group = 0;

            for (int tile_index = 0; tile_index < _tile_count; tile_index++)
            {
                var reachable = reachable_table[tile_index] <= robot.movement_range;
                if (reachable)
                {
                    // we can potentially get there, let's rate this move
                    var x = tile_index % _board_size;
                    var y = tile_index / _board_size;

                    var temp_attackable = new int[_tile_count];

                    // clear attackable table
                    // for (int j = 0; j < temp_attackable.Length; j++) {
                    //     temp_attackable[j] = 0;
                    // }

                    Tilecast(temp_attackable, robot.x, robot.y, 1, 0, group_id: 1, -1);
                    Tilecast(temp_attackable, robot.x, robot.y, -1, 0, group_id: 2, -1);
                    Tilecast(temp_attackable, robot.x, robot.y, 0, 1, group_id: 3, -1);
                    Tilecast(temp_attackable, robot.x, robot.y, 0, -1, group_id: 4, -1);

                    int[] score_for_attack_group = new int[5];


                    for (int j = 0; j < temp_attackable.Length; j++)
                    {
                        if (temp_attackable[j] != 0)
                        {
                            var group = temp_attackable[j];
                            // Could potentially reach here, who did we hit?
                            for (int k = 0; k < _robots.Count; k++)
                            {
                                var targetRobot = _robots[k];
                                if (CoordsToIndex(targetRobot.x, targetRobot.y) == j)
                                {
                                    if (targetRobot.friend)
                                        score_for_attack_group[group]++;
                                    else
                                        score_for_attack_group[group]--;
                                }
                            }
                        }
                    }

                    var best_group = -1;
                    var best_score = -1000;
                    for (int group = 1; group < 5; group++)
                    {
                        var s = score_for_attack_group[group];
                        if (s > best_score)
                        {
                            best_score = s;
                            best_group = group;
                        }
                        // Avoid giving a priority to a particular angle (kindof...)
                        if (s == best_score)
                        {
                            if (Random.value < 0.25f)
                            {
                                best_group = group;
                            }
                        }
                    }

                    if (best_score > 0)
                    {
                        robot.plan.move_to_tile = tile_index;
                        robot.plan.attack_group = best_group;
                        _robots[i] = robot;
                        break; // move on to the next enemy's plan
                    }
                }
            }

        }
    }

    void EnemiesExecute()
    {
        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            if (robot.friend)
            {
                continue;
            }
            else
            {

            }
        }
    }

    void AddRobot(int x, int y, int movement_range, bool friend = true)
    {
        Actor actor = new Actor();
        actor.x = x;
        actor.y = y;
        actor.movement_range = movement_range;
        actor.max_hp = 3;
        actor.current_hp = 3;
        actor.friend = friend;
        var template = friend ? _robotTemplate : _enemyTemplate;
        actor.view = Instantiate(template, tiles_container);
        actor.view.gameObject.SetActive(true);
        _robots.Add(actor);

        _reachable.Add(new int[_tile_count]);
        _attackable.Add(new int[_tile_count]);
    }

    Vector2 GetTilePos(int x, int y)
    {
        Vector2 origin = new Vector2(-offset_w * (_board_size - 1), 0f);
        Vector2 pos = origin + offset_x * x + offset_y * y;
        return pos;
    }

    void GenerateTerrain(int levelId)
    {
        _level_data = _levels[levelId];
        _board_size = (int)Mathf.Sqrt((float)_level_data.Length);
        _tile_count = _level_data.Length;

        if (_board_size * _board_size != _tile_count)
        {
            Debug.LogError("Damned, a non square level :(");
            this.enabled = false;
            return;
        }

        _selectionFrames = new SpriteRenderer[_tile_count];

        _grassTile.gameObject.SetActive(false);
        _waterTile.gameObject.SetActive(false);
        _rock.gameObject.SetActive(false);
        _column.gameObject.SetActive(false);
        _goal.gameObject.SetActive(false);
        _selectionFrame.gameObject.SetActive(false);

        Vector2 origin = new Vector2(-offset_w * (_board_size - 1), 0f);
        for (int tile_index = 0, y = 0; y < _board_size; y++)
        {
            for (int x = 0; x < _board_size; x++, tile_index++)
            {
                // each tile
                Vector2 pos = GetTilePos(x, y);
                char tile_char = GRASS; // default tile
                if (tile_index < _level_data.Length)
                {
                    tile_char = _level_data[tile_index];
                }
                if (tile_char != WATER)
                {
                    var tile = Instantiate(_grassTile, tiles_container);
                    tile.transform.localPosition = pos;
                    tile.gameObject.SetActive(true);
                }
                else
                {
                    var tile = Instantiate(_waterTile, tiles_container);
                    tile.transform.localPosition = pos;
                    tile.gameObject.SetActive(true);
                }

                if (tile_char == SPAWN)
                {
                    AddRobot(x, y, 2, friend: true);
                }

                if (tile_char == ENEMY)
                {
                    AddRobot(x, y, 2, friend: false);
                }

                if (tile_char == ROCK)
                {
                    var rockView = Instantiate(_rock, tiles_container);
                    rockView.transform.localPosition = pos;
                    rockView.gameObject.SetActive(true);
                    var rock = new Rock();
                    rock.actor.x = x;
                    rock.actor.y = y;
                    rock.view = rockView;
                    _rocks.Add(rock);
                }
                if (tile_char == COLUMN)
                {
                    var columnView = Instantiate(_column, tiles_container);
                    columnView.transform.localPosition = pos;
                    columnView.gameObject.SetActive(true);
                }
                if (tile_char == GOAL)
                {
                    var goal = Instantiate(_goal, tiles_container);
                    goal.transform.localPosition = pos;
                    goal.gameObject.SetActive(true);
                    _goal_actor = new Actor();
                    _goal_actor.x = x;
                    _goal_actor.y = y;
                    _goal_actor.max_hp = 3;
                    _goal_actor.current_hp = _goal_actor.max_hp;
                    _goal_actor.view = goal.GetComponentInChildren<RobotView>();
                }
                var selectionFrame = Instantiate(_selectionFrame, tiles_container);
                selectionFrame.transform.localPosition = pos;
                selectionFrame.gameObject.SetActive(true);

                _selectionFrames[y * _board_size + x] = selectionFrame.GetComponent<SpriteRenderer>();
            }
        }
    }

    void DiscoverTile(int[] _distances, int tile_index, int distance)
    {
        if (_distances[tile_index] > distance)
        {
            _distances[tile_index] = distance;

            distance++;

            //
            // Discover neighbours
            //
            if (tile_index % _board_size != 0)
            {
                var left = tile_index - 1;
                if (IsTileFree(left))
                {
                    DiscoverTile(_distances, left, distance);
                }
            }

            if (tile_index % _board_size != _board_size - 1)
            {
                var right = tile_index + 1;
                if (IsTileFree(right))
                {
                    DiscoverTile(_distances, right, distance);
                }
            }

            if (tile_index >= _board_size)
            {
                var bottom = tile_index - _board_size;
                if (IsTileFree(bottom))
                {
                    DiscoverTile(_distances, bottom, distance);
                }
            }

            if (tile_index < _tile_count - _board_size)
            {
                var top = tile_index + _board_size;
                if (IsTileFree(top))
                {
                    DiscoverTile(_distances, top, distance);
                }
            }
        }
    }

    bool Tilecast(int[] attackable, int start_x, int start_y, int d_x, int d_y, int group_id, int hovered_index)
    {
        bool met_hovered_tile = false;
        while (true)
        {
            start_x += d_x;
            start_y += d_y;
            int index = CoordsToIndex(start_x, start_y);
            if (index == hovered_index && index != -1)
            {
                met_hovered_tile = true;
            }
            if (index == -1)
            {
                break;
            }
            attackable[index] = group_id;
            if (!IsTileFree(index, allow_water: true))
            {
                break;
            }
        }
        return met_hovered_tile;
    }

    void Update()
    {
        //
        // Find out which tile the cursor is on
        //
        Vector3 screen_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screen_pos.z = 0.0f;
        Vector3 tiles_origin_pos = new Vector3(-offset_w * (_board_size - 1), 0f, 0.0f);
        Vector3 delta_pos = screen_pos - tiles_origin_pos;
        var div_x = delta_pos / offset_x;
        var div_y = delta_pos / offset_y;
        int hovered_x = Mathf.RoundToInt((div_x.x + div_x.y) * 0.5f);
        int hovered_y = Mathf.RoundToInt((div_y.x + div_y.y) * 0.5f);
        int hovered_tile = -1; // -1 means no tile is hovered
        if (hovered_x >= 0 && hovered_y >= 0 && hovered_x < _board_size && hovered_y < _board_size)
        {
            hovered_tile = hovered_y * _board_size + hovered_x;
        }

        bool hovered_tile_is_free = IsTileFree(hovered_tile);

        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            if (robot.friend)
            {
                continue;
            }

            // we're dealing with an enemy
            var pos = (Vector3)GetTilePos(robot.x, robot.y);
            pos.z = 10f;
            DebugText.Text(robot.view.transform.position, $"moving to: {robot.plan.move_to_tile}, group: {robot.plan.attack_group}");
            Debug.Log($"moving to: {robot.plan.move_to_tile}, group: {robot.plan.attack_group}");
        }

        //
        // Update reachable tiles for each robot
        //
        for (int i = 0; i < _robots.Count; i++)
        {
            var actor = _robots[i];
            var reachable = _reachable[i];

            // clear reachable table
            for (int j = 0; j < reachable.Length; j++)
            {
                reachable[j] = 1000; // init with a crazy high val
            }

            DiscoverTile(reachable, CoordsToIndex(actor.x, actor.y), 0);
        }

        //
        // Update attackable tiles
        //
        int attack_group_id = -1;
        for (int robot_index = 0; robot_index < _robots.Count; robot_index++)
        {
            //UpdateAttackableTable(attackableTable, robot_index);
            var attackable_table = _attackable[robot_index];
            var robot = _robots[robot_index];

            // clear attackable table
            for (int j = 0; j < attackable_table.Length; j++)
            {
                attackable_table[j] = 0;
            }

            var selected = _selected_robot_index == robot_index;
            if (selected)
            {
                if (Tilecast(attackable_table, robot.x, robot.y, 1, 0, group_id: 1, hovered_tile)) attack_group_id = 1;
                if (Tilecast(attackable_table, robot.x, robot.y, -1, 0, group_id: 2, hovered_tile)) attack_group_id = 2;
                if (Tilecast(attackable_table, robot.x, robot.y, 0, 1, group_id: 3, hovered_tile)) attack_group_id = 3;
                if (Tilecast(attackable_table, robot.x, robot.y, 0, -1, group_id: 4, hovered_tile)) attack_group_id = 4;
            }
            //DebugText.Text(Vector3.zero, attack_group_id.ToString());
        }

        //
        // Update tiles highlights
        //
        for (int tile_index = 0, y = 0; y < _board_size; y++)
        {
            for (int x = 0; x < _board_size; x++, tile_index++)
            {
                var frame = _selectionFrames[tile_index];
                var tile_in_range = InRange(_robots[_selected_robot_index], x, y, 2);
                var reachable_table = _reachable[_selected_robot_index];
                var attackable_table = _attackable[_selected_robot_index];
                var selected_robot = _robots[_selected_robot_index];

                //DebugText.Text(GetTilePos(x,y), attackable_table[tile_index].ToString());

                var color = Color.clear;
                var reachable = reachable_table[tile_index] <= selected_robot.movement_range;
                var attackable = attackable_table[tile_index] != 0;
                if (_uiMode == UiMode.Move && reachable)
                {
                    color = Color.green;
                }
                if (_uiMode == UiMode.Attack && attackable)
                {
                    color = Color.yellow;
                    var selected_attack_group = (attack_group_id == attackable_table[tile_index]);
                    if (selected_attack_group)
                    {
                        color = Color.red;
                    }
                }
                if (hovered_tile == tile_index && _uiMode != UiMode.Attack)
                {
                    color = new Color(1f, 1f, 1f, 0.6f);
                    if (_uiMode == UiMode.Move && !hovered_tile_is_free)
                    {
                        color = Color.red;
                    }
                    if (_uiMode == UiMode.Move && !reachable)
                    {
                        color = Color.red;
                    }
                }
                if (x == selected_robot.x && y == selected_robot.y)
                {
                    color = Color.white;
                }

                frame.color = color;
                //frame.enabled = color;
            }
        }

        //
        // Input select robot
        //
        if (_uiMode == UiMode.Select)
        {
            if (hovered_tile >= 0 && Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < _robots.Count; i++)
                {
                    var actor = _robots[i];
                    int robot_tile = CoordsToIndex(actor.x, actor.y);
                    if (robot_tile == hovered_tile)
                    {
                        _selected_robot_index = i;
                    }
                }
                _uiMode = UiMode.Select;
            }
        }

        //
        // Input robot movement
        //
        if (_uiMode == UiMode.Move)
        {
            if (hovered_tile >= 0 && Input.GetMouseButtonDown(0) && hovered_tile_is_free)
            {
                var robot = _robots[_selected_robot_index];
                robot.x = hovered_x;
                robot.y = hovered_y;
                _robots[_selected_robot_index] = robot;
                _uiMode = UiMode.Select;
                var partivjnc = robot.view.GetComponentInChildren<ParticleSystem>();
                if (partivjnc)
                {
                    partivjnc.Emit(4);
                }
            }
        }

        //
        // Input attack
        //
        if (_uiMode == UiMode.Attack)
        {
            if (hovered_tile >= 0 && Input.GetMouseButtonDown(0) && attack_group_id != 0)
            {
                var robot = _robots[_selected_robot_index];

                for (int tile_index = 0, y = 0; y < _board_size; y++)
                {
                    for (int x = 0; x < _board_size; x++, tile_index++)
                    {
                        var attackable_table = _attackable[_selected_robot_index];
                        if (attackable_table[tile_index] == attack_group_id)
                        {
                            dammage_tile(tile_index, 1);
                        }
                    }
                }

                _uiMode = UiMode.Select;
            }
        }

        //
        // Robot update visuals
        //
        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            var view = robot.view;

            var prevPos = view.transform.position;
            view.transform.position = GetTilePos(robot.x, robot.y);
            view.RefreshHealth(robot.max_hp, robot.current_hp);
            // if (move_particles) {
            //     print("MOVIN THE PARTICLES");
            //     var newPos = view.transform.position;
            //     for (int w = 0; w < 10; w++) {
            //         var ppppos = Vector3.Lerp(prevPos, newPos, (float)w / 10.0f);
            //         var bloefm = new ParticleSystem.EmitParams();
            //         bloefm.position = ppppos;

            //         robot.view.GetComponentInChildren<ParticleSystem>().Emit(bloefm, 5);
            //     }

            //     //robot.view.GetComponentInChildren<ParticleSystem>().Emit(4);
            // }
        }

        // Update other health counters
        _goal_actor.view.RefreshHealth(_goal_actor.max_hp, _goal_actor.current_hp);
    }

    void dammage_tile(int tile_index, int dammage)
    {
        for (int i = 0; i < _rocks.Count; i++)
        {
            var rock = _rocks[i];
            if (CoordsToIndex(rock.actor.x, rock.actor.y) == tile_index)
            {
                rock.actor.current_hp--;
                if (rock.actor.current_hp <= 0)
                {
                    Debug.Log("A rock was KILLED!!!");
                }
                _rocks[i] = rock;
            }
        }
        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            if (CoordsToIndex(robot.x, robot.y) == tile_index)
            {
                robot.current_hp--;
                if (robot.current_hp <= 0)
                {
                    Debug.Log("A robot was KILLED!!!");
                }
                robot.view.DammageEffect(dammage);
                _robots[i] = robot;
            }
        }

        if (CoordsToIndex(_goal_actor.x, _goal_actor.y) == tile_index)
        {
            _goal_actor.current_hp--;
            if (_goal_actor.current_hp <= 0)
            {
                Debug.Log("Battle won!");
                _battle_won = true;
            }
            _goal_actor.view.DammageEffect(dammage);
        }


    }

    bool IsTileFree(int index, bool allow_water = false)
    {
        if (index == -1)
        {
            return false;
        }
        if (index >= _level_data.Length)
        {
            return false;
        }
        char tile_type = _level_data[index];

        if (tile_type == WATER && !allow_water)
        {
            return false;
        }

        if (tile_type == COLUMN || tile_type == GOAL)
        {
            return false;
        }

        for (int i = 0; i < _rocks.Count; i++)
        {
            var actor = _rocks[i].actor;
            if (CoordsToIndex(actor.x, actor.y) == index)
            {
                if (actor.current_hp > 0)
                {
                    return false;
                }
            }
        }
        for (int i = 0; i < _robots.Count; i++)
        {
            var actor = _robots[i];
            if (CoordsToIndex(actor.x, actor.y) == index)
            {
                return false;
            }
        }
        return true;
    }

    int CoordsToIndex(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _board_size && y < _board_size)
        {
            return y * _board_size + x;
        }
        else
        {
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
