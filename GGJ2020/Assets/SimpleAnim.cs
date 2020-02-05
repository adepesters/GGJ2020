using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnim : MonoBehaviour
{
    public Sprite[] _frames;
    public float _frame_duration = 0.8f;

    private float _timer = 0.0f;
    private int frame_num = 0;
    private SpriteRenderer _sr;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        frame_num += Random.Range(0, _frames.Length);
        _sr.sprite = _frames[frame_num];
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _frame_duration) {
            _timer -= _frame_duration;

            frame_num++;
            if (frame_num == _frames.Length) {
                frame_num = 0;
            }
            _sr.sprite = _frames[frame_num];
        }
    }
}
