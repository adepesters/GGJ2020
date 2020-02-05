
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class RobotView : MonoBehaviour
{
    public SpriteRenderer health_slot_template;
    public Color _empty_color;
    public Color _friend_color;
    public TextMeshPro _dammage_label_template;
    public SimpleAnim _simpleAnim;
    public SpriteRenderer _shadow;
    private SpriteRenderer _sprite_renderer;
    private List<SpriteRenderer> _health_elements = new List<SpriteRenderer>();
    private List<SpriteRenderer> _health_elements_backgrounds = new List<SpriteRenderer>();

    public void SkinBot(RobotDefinition def)
    {
       
        _simpleAnim._frames = def.IdleAnim;
        _shadow.sprite = def.Shadow;
    }

    public void Start()
    {
        health_slot_template.gameObject.SetActive(false);
        _dammage_label_template.gameObject.SetActive(false);
        if (_simpleAnim) {
            _sprite_renderer = _simpleAnim.GetComponent<SpriteRenderer>();
        }
    }

    public void RefreshHealth(int max_health, int current_health)
    {
        int i = 0;
        const float spacing = 28f;
        var total_width = spacing * (max_health - 1f);
        var base_x = -total_width * 0.5f;
        for(; i < max_health; i++) {
            if (_health_elements.Count < max_health) {
                _health_elements.Add(Instantiate(health_slot_template, health_slot_template.transform.parent));
                var bg = _health_elements[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
                _health_elements_backgrounds.Add(bg);
            }
            _health_elements[i].gameObject.SetActive(true);
            var prev_alpha = _health_elements[i].color.a;
            var color = current_health > i ? _friend_color : _empty_color;;
            color.a = prev_alpha;
            _health_elements[i].color = color;
            _health_elements[i].transform.localPosition = new Vector3(base_x, 0f, 0f);
            base_x += spacing;
        }
        // disable any remaining slots 
        for (; i < _health_elements.Count; i++) {
            _health_elements[i].gameObject.SetActive(false);
        }
        var dead = current_health <= 0;
        if (_sprite_renderer) {
            _sprite_renderer.color = dead ? new Color(0.5f,0.5f,0.5f,_sprite_renderer.color.a) : new Color(1,1,1,_sprite_renderer.color.a);
            _simpleAnim.enabled = !dead;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        while(_sprite_renderer.color.a > 0.01f) {
            var color = _sprite_renderer.color;
            color.a = Mathf.MoveTowards(_sprite_renderer.color.a, 0f, Time.smoothDeltaTime);
            _sprite_renderer.color = color;
            
            foreach(var e in _health_elements) {
                var c = e.color;
                c.a = color.a;
                e.color = c;
            }

            foreach(var e in _health_elements_backgrounds) {
                var c = e.color;
                c.a = color.a;
                e.color = c;
            }

            Debug.Log(_sprite_renderer.color);
            yield return null;
        }
        //Destroy(gameObject);
    }

    public void DammageEffect(int dammage)
    {
        if (dammage > 0) {
            
            var dammage_label = Instantiate(_dammage_label_template, _dammage_label_template.transform.parent);
            dammage_label.gameObject.SetActive(true);
            dammage_label.text = $"-{dammage}";
            StartCoroutine(AnimLabelRoutine(dammage_label));
            Destroy(dammage_label.gameObject, 1.4f);
        }
    }

    IEnumerator AnimLabelRoutine(TextMeshPro tmp)
    {
        var speed = 20f;
        while(tmp != null) {
            tmp.transform.Translate(Vector3.up * speed * Time.smoothDeltaTime, Space.World);
            speed *= 0.96f;
            yield return null;
        }
    }
}

