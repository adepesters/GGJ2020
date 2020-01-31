using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Transform _grassTile;
    public Transform _rock;
    public float _ratio;

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        const int board_size = 8;

        const float offset_w = 368f/4f;
        float offset_h = offset_w * _ratio;
        Vector2 offset_x = new Vector2(offset_w, -offset_h);
        Vector2 offset_y = new Vector2(offset_w, offset_h);

        Vector2 origin = new Vector2(-offset_w * board_size, 0f);
        for (int y = 0; y < board_size; y++) {
            for (int x = 0; x < board_size; x++) {
                Vector2 pos = origin + offset_x * x + offset_y * y;
                var tile = Instantiate(_grassTile, transform);
                tile.transform.localPosition = pos;
                if (Random.value < 0.2f) {
                    var rock = Instantiate(_rock, transform);
                    rock.transform.localPosition = pos;
                }

            }
        }
    }
}
