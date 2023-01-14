using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    private Character _character;
    private float _maxHealth;
    private float TOLERANCE = 0.01f;
    private Canvas _canvas;
    
    [SerializeField] private RectTransform fill;
    
    private void Awake()
    {
        _character = GetComponentInParent<Character>();
        _canvas = GetComponentInChildren<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _maxHealth = _character.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(_character.Hp - _maxHealth) < TOLERANCE)
        {
            _canvas.gameObject.SetActive(false);
        }
        else
        {
            _canvas.gameObject.SetActive(true);
            fill.anchorMin = new Vector2(1-(_character.Hp)/_maxHealth, 0);
        }
    }
}
