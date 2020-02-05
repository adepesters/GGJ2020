using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public struct Plan
{
    public bool is_valid;
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
    public int robot_index;
    public RobotView view;
    public List<int> plop;
    public bool friend;
    public Plan plan;
    public bool dead;
    public bool has_moved;
    public bool has_shot;
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
    public AudioClip[] attackSounds;

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
    public int _currentSpawnRobotSlot = 0;

    public void ResetGame() {
        _battle_won = false;
        _currentSpawnRobotSlot = 0;

        _robots.Clear();
        _rocks.Clear();

        for(int i = tiles_container.childCount - 1; i >= 0; i--) {
            Destroy(tiles_container.GetChild(i).gameObject);
        }
    }

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

        // Jo
        "I......^" +
        ".X...__." +
        "..O..__." +
        "...O+..." +
        "+..XO..." +
        ".__..O.." +
        ".__...X." +
        "...+...I",

        "_......." +
        "_.X....+" +
        "_.X__..." +
        "_.X__..+" +
        "_^X__..." +
        "_.X__..+" +
        "_.X....." +
        "_.......",

        "O.+...+." +
        ".....I.+" +
        ".X..XI.." +
        ".I...I.." +
        ".I....X." +
        ".I......" +
        ".XX..I.." +
        "^.X..OOI",

        ".O..I.+X" +
        "+.._...." +
        ".X....X+" +
        ".__X.I.." +
        "..X....." +
        "X.....O." +
        "X^.I..X." +
        ".X....__",

        "+......+" +
        ".O....O." +
        "..____.." +
        ".._^._.." +
        ".._.._.." +
        ".X_IX_X." +
        "........" +
        "OO..+.OO",

        "IOIOOOOO+" +
        "O..X....." +
        "O.____..." +
        "I._XXX..." +
        "+._X^X..." +
        "I._XXX..." +
        "O.____..." +
        "O..X....." +
        "IOOOOOOO+",

        "_X..+.O..+" +
        "___I...O.." +
        "_X_.X....." +
        "___I.O...." +
        "_X_....+.." +
        "___IX....." +
        "_X_.....O." +
        "___I..X..." +
        "_X_.X..X.." +
        "__^X.OOOOO",

        // Swan
        ".+.+.+.O" +
        "X......X" +
        ".X.__.X." +
        "..X..X.." +
        "O..XX..O" +
        "..X__X.." +
        ".X.__.X." +
        "X.O.^O.X",

        "OO..X......" +
        ".O..X...++." +
        ".OO.X...+.." +
        "..O.X......" +
        "..OO......." +
        "...O......." +
        "XX...___..." +
        "XXX..___..." +
        "^XXX.___..." +
        "XXX..___..." +
        "XX........."
        

    };

    string _level_data;

    GameSession gameSession;

    void OnEnable()
    {
        ResetGame();

        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();

        _robotTemplate.gameObject.SetActive(false);
        _enemyTemplate.gameObject.SetActive(false);

        GenerateTerrain(gameSession.SelectedLevel);

        _move_button.onClick.AddListener(() =>
        {
            var robot = _robots[_selected_robot_index];
            if (_selected_robot_index >= 0 && robot.current_hp > 0 && robot.friend && !_battle_won)
            {
                _uiMode = UiMode.Move;
            }
        });

        _action_button.onClick.AddListener(() =>
        {
            var robot = _robots[_selected_robot_index];
            if (_selected_robot_index >= 0 && robot.current_hp > 0 && robot.friend && !_battle_won)
            {
                _uiMode = UiMode.Attack;
            }
        });

        _end_turn_button.onClick.AddListener(() =>
        {
            NextTurn();
        });
    }

    public void NextTurn()
    {
        //EnemiesExecute();
        StartCoroutine(NextTurnRoutine());
    }

    IEnumerator NextTurnRoutine()
    {
        yield return EnemiesPlan();
        //yield return EnemiesExecutePlannedMove();

        // reset turn vars
        for (int i = 0; i < _robots.Count; i++) {
            var robot = _robots[i];
            robot.has_moved = false;
            robot.has_shot = false;
            _robots[i] = robot;
        }
    }

    IEnumerator EnemiesExecutePlannedMove()
    {
        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            if (robot.friend || !robot.plan.is_valid)
            {
                continue;
            }

            var dest_x = robot.plan.move_to_tile % _board_size;
            var dest_y = robot.plan.move_to_tile / _board_size;
            robot = Move(robot, dest_x, dest_y);
            robot.plan.is_valid = false;
            _robots[i] = robot;

            //yield return new WaitForSeconds(0.6f);
        }
        yield return null;
    }

    Actor Move(Actor actor, int target_x, int target_y)
    {
        actor.x = target_x;
        actor.y = target_y;
        var partivjnc = actor.view.GetComponentInChildren<ParticleSystem>();
        if (partivjnc)
        {
            partivjnc.Emit(4);
        }
        actor.has_moved = true;

        return actor;
    }


    IEnumerator EnemiesPlan()
    {
        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            if (robot.friend || robot.dead)
            {
                continue;
            }

            // we're dealing with an enemy
            var reachable_table = _reachable[i];
            // int best_move_score = -10000;
            // int best_move_tile = -1;
            // int best_move_group = 0;

            var candidates = new List<int>();

            for (int tile_index = 0; tile_index < _tile_count; tile_index++)
            {
                var reachable = reachable_table[tile_index] <= robot.movement_range;
                if (reachable)
                {
                    candidates.Add(tile_index);

                    // we can potentially get there, let's rate this move
                    // var x = tile_index % _board_size;
                    // var y = tile_index / _board_size;
                }
            }

            int dest_index = -1;
            int current_group_id = -1;
            for (int iter = 0; iter < 8; iter++) {
                dest_index = candidates[Random.Range(0, candidates.Count)];

                var x = dest_index % _board_size;
                var y = dest_index / _board_size;

                var temp_attackable = new int[_tile_count];

                bool met_friend = false;

                current_group_id = 1;
                Tilecast(temp_attackable, x, y, 1, 0, current_group_id, -1, ref met_friend); 
                if (met_friend) goto after_tilecast;

                current_group_id = 2;
                Tilecast(temp_attackable, x, y, -1, 0, current_group_id, -1, ref met_friend);
                if (met_friend) goto after_tilecast;

                current_group_id = 3;
                Tilecast(temp_attackable, x, y, 0, 1, current_group_id, -1, ref met_friend);
                if (met_friend) goto after_tilecast;
                
                current_group_id = 4;
                Tilecast(temp_attackable, x, y, 0, -1, current_group_id, -1, ref met_friend);
                if (met_friend) goto after_tilecast;

                current_group_id = 0; // no attack

                after_tilecast:
                if (met_friend) {
                    break;
                }
            }

            // robot.plan.is_valid = true;
            // robot.plan.move_to_tile = dest_index;
            //robot.plan.attack_group = best_group;
            var dest_x = dest_index % _board_size;
            var dest_y = dest_index / _board_size;

            robot = Move(robot, dest_x, dest_y);

            _robots[i] = robot;

            if (current_group_id != 0) {
                yield return new WaitForSeconds(0.2f);
                apply_attack(i, current_group_id);
            }

            yield return new WaitForSeconds(0.5f);
        }
        

        //     for (int tile_index = 0; tile_index < _tile_count; tile_index++)
        //     {
        //         var reachable = reachable_table[tile_index] <= robot.movement_range;
        //         if (reachable)
        //         {
        //             // we can potentially get there, let's rate this move
        //             var x = tile_index % _board_size;
        //             var y = tile_index / _board_size;

        //             var temp_attackable = new int[_tile_count];

        //             // clear attackable table
        //             // for (int j = 0; j < temp_attackable.Length; j++) {
        //             //     temp_attackable[j] = 0;
        //             // }



        //             int[] score_for_attack_group = new int[5];


        //             for (int j = 0; j < temp_attackable.Length; j++)
        //             {
        //                 if (temp_attackable[j] != 0)
        //                 {
        //                     var group = temp_attackable[j];
        //                     // Could potentially reach here, who did we hit?
        //                     for (int k = 0; k < _robots.Count; k++)
        //                     {
        //                         var targetRobot = _robots[k];
        //                         if (CoordsToIndex(targetRobot.x, targetRobot.y) == j)
        //                         {
        //                             if (targetRobot.friend)
        //                                 score_for_attack_group[group]++;
        //                             else
        //                                 score_for_attack_group[group]--;
        //                         }
        //                     }
        //                 }
        //             }

        //             var best_group = -1;
        //             var best_score = -1000;
        //             for (int group = 1; group < 5; group++)
        //             {
        //                 var s = score_for_attack_group[group];
        //                 if (s > best_score)
        //                 {
        //                     best_score = s;
        //                     best_group = group;
        //                 }
        //                 // Avoid giving a priority to a particular angle (kindof...)
        //                 if (s == best_score)
        //                 {
        //                     if (Random.value < 0.25f)
        //                     {
        //                         best_group = group;
        //                     }
        //                 }
        //             }

        //             if (best_score > 0)
        //             {
        //                 robot.plan.is_valid = true;
        //                 robot.plan.move_to_tile = tile_index;
        //                 robot.plan.attack_group = best_group;
        //                 _robots[i] = robot;
        //                 break; // move on to the next enemy's plan
        //             }
        //         }
        //     }

        // }
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

    void AddRobot(int x, int y, int movement_range, bool friend = true, int robot_index = -1)
    {
        Actor actor = new Actor();
        actor.x = x;
        actor.y = y;
        actor.movement_range = movement_range;
        actor.max_hp = 3;
        actor.current_hp = 3;
        actor.friend = friend;
        actor.robot_index = robot_index;
        var template = friend ? _robotTemplate : _enemyTemplate;
        actor.view = Instantiate(template, tiles_container);
        actor.view.gameObject.SetActive(true);
        if (robot_index != -1) {
            var gameData =  Resources.Load<GameData>("GameData");
            var def = gameData.Definitions[robot_index];
            actor.view.SkinBot(def);
            actor.max_hp = def.StatsMax[0];
            actor.current_hp = def.StatsMax[0];
        } else {
            var gameData =  Resources.Load<GameData>("GameData");
            var def = gameData.Enemies[0];
            actor.view.SkinBot(def);
            actor.max_hp = def.StatsMax[0];
            actor.current_hp = def.StatsMax[0];
        }
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
                    // friend robot
                    var robot_index = FindObjectOfType<CraftingManager>().RobotSlot[_currentSpawnRobotSlot];
                    if (robot_index != -1) {
                        AddRobot(x, y, 2, friend: true, robot_index);
                        _currentSpawnRobotSlot++;
                    } 
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

    bool Tilecast(int[] attackable, int x, int y, int d_x, int d_y, int group_id, int hovered_index, ref bool met_friend)
    {
        bool met_hovered_tile = false;
        while (true)
        {
            x += d_x;
            y += d_y;
            int index = CoordsToIndex(x, y);
            if (index == hovered_index && index != -1)
            {
                met_hovered_tile = true;
            }
            if (index == -1)
            {
                break;
            }

            foreach (var r in _robots)
            {
                if (r.x == x && r.y == y) {
                    if (!r.dead && r.friend) {
                        met_friend = true;
                    }
                }
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
        // Update win state
        // 
        bool at_least_one_friend_alive = false;

        foreach(var r in _robots) {
            if (r.friend && !r.dead) {
                at_least_one_friend_alive = true;
            }
        }
        if (!at_least_one_friend_alive) {
            gameSession.CanLaunchGameOver = true;
        }

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


        //
        // Debug enemies plan
        //
        /* 
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
            if (robot.plan.is_valid)
            {
                var target_x = robot.plan.move_to_tile % _board_size;
                var target_y = robot.plan.move_to_tile / _board_size;
                DebugText.Text(robot.view.transform.position, $"{robot.x};{robot.y} move to: ({target_x};{target_y}), group: {robot.plan.attack_group}");
            }
            else
            {
                DebugText.Text(robot.view.transform.position, $"No plan");
            }
        }
        */

        //
        // Update reachable tiles for each robot
        //
        for (int i = 0; i < _robots.Count; i++)
        {
            var actor = _robots[i];
            if(actor.dead)
                continue;

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
            if(robot.dead)
                continue;

            // clear attackable table
            for (int j = 0; j < attackable_table.Length; j++)
            {
                attackable_table[j] = 0;
            }

            int temp_attack_group_id = 0;
            bool dummy = false;
            if (Tilecast(attackable_table, robot.x, robot.y, 1, 0, group_id: 1, hovered_tile, ref dummy)) temp_attack_group_id = 1;
            if (Tilecast(attackable_table, robot.x, robot.y, -1, 0, group_id: 2, hovered_tile, ref dummy)) temp_attack_group_id = 2;
            if (Tilecast(attackable_table, robot.x, robot.y, 0, 1, group_id: 3, hovered_tile, ref dummy)) temp_attack_group_id = 3;
            if (Tilecast(attackable_table, robot.x, robot.y, 0, -1, group_id: 4, hovered_tile, ref dummy)) temp_attack_group_id = 4;

            var selected = _selected_robot_index == robot_index;
            if (selected) {
                attack_group_id = temp_attack_group_id;
            }
            
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

                var reachable_table = _reachable[_selected_robot_index];
                var reachable = reachable_table[hovered_tile] <= robot.movement_range;

                if (reachable) {
                    robot = Move(robot, hovered_x, hovered_y);
                }

                _robots[_selected_robot_index] = robot;
                _uiMode = UiMode.Select;
            }
        }

        //
        // Input attack
        //
        if (_uiMode == UiMode.Attack)
        {
            if (hovered_tile >= 0 && Input.GetMouseButtonDown(0) && attack_group_id != 0)
            {
                apply_attack(_selected_robot_index, attack_group_id);

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

        //
        // Update other health counters
        //
        _goal_actor.view.RefreshHealth(_goal_actor.max_hp, _goal_actor.current_hp);

        //
        // Update user interface
        //
        {
            var robot = _robots[_selected_robot_index];
            _move_button.interactable = !robot.has_moved;
            _action_button.interactable = !robot.has_shot;
        }
    }

    void apply_attack(int _selected_robot_index, int attack_group_id)
    {
        GetComponent<AudioSource>().PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]);

        var robot = _robots[_selected_robot_index];
        robot.has_shot = true;
        _robots[_selected_robot_index] = robot; // NOTE: there would be a risk of overriding stuff done in dammage_tile here.                }

        Debug.Log("apply attack with id " + attack_group_id);

        if (attack_group_id == 0) {
            return;
        }
        //Debug.Break();

        var attackable_table = _attackable[_selected_robot_index];
        for (int tile_index = 0, y = 0; y < _board_size; y++)
        {
            for (int x = 0; x < _board_size; x++, tile_index++)
            {       
                //DebugText.Text(GetTilePos(x,y), attackable_table[tile_index].ToString());
                if (attackable_table[tile_index] == attack_group_id)
                {
                    dammage_tile(tile_index, 1);
                }
            }
        }
    }

    void dammage_tile(int tile_index, int dammage)
    {
        for (int i = 0; i < _rocks.Count; i++)
        {
            var rock = _rocks[i];
            if(rock.actor.dead)
                continue;
            if (CoordsToIndex(rock.actor.x, rock.actor.y) == tile_index)
            {
                rock.actor.current_hp--;
                if (rock.actor.current_hp <= 0)
                {
                    Debug.Log("A rock was KILLED!!!");
                    rock.actor.dead = true;
                }
                _rocks[i] = rock;
            }
        }
        for (int i = 0; i < _robots.Count; i++)
        {
            var robot = _robots[i];
            if(robot.dead)
                continue;

            if (CoordsToIndex(robot.x, robot.y) == tile_index)
            {
                robot.current_hp--;
                if (robot.current_hp <= 0)
                {
                    Debug.Log("A robot was KILLED!!!");
                    robot.dead = true;

                    if (!robot.friend) {
                        robot.view.FadeOut();
                        //_robots.RemoveAt(i);
                    }
                }
                robot.view.DammageEffect(dammage);
                _robots[i] = robot;

                robot.view.RefreshHealth(robot.max_hp, robot.current_hp);
            }
        }

        if (CoordsToIndex(_goal_actor.x, _goal_actor.y) == tile_index)
        {
            _goal_actor.current_hp--;
            if (_goal_actor.current_hp <= 0)
            {
                Debug.Log("Battle won!");
                _battle_won = true;
                gameSession.CanLaunchMainRoom = true;
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

        if (tile_type == COLUMN || tile_type == GOAL || tile_type == ROCK)
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
            if (actor.dead == false && CoordsToIndex(actor.x, actor.y) == index)
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
