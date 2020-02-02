
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

    public void SkinBot(RobotDefinition def)
    {
       
        _simpleAnim._frames = def.IdleAnim;
        _shadow.sprite = def.Shadow;
    }

    private List<SpriteRenderer> _health_elements = new List<SpriteRenderer>();

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
            }
            _health_elements[i].gameObject.SetActive(true);
            _health_elements[i].color = current_health > i ? _friend_color : _empty_color;
            _health_elements[i].transform.localPosition = new Vector3(base_x, 0f, 0f);
            base_x += spacing;
        }
        // disable any remaining slots 
        for (; i < _health_elements.Count; i++) {
            _health_elements[i].gameObject.SetActive(false);
        }
        var dead = current_health <= 0;
        if (_sprite_renderer) {
            _sprite_renderer.color = dead ? new Color(0.5f,0.5f,0.5f,1f) : new Color(1,1,1,1);
            _simpleAnim.enabled = !dead;
        }
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

