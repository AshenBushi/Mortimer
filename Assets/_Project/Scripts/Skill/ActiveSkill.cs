using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    [SerializeField] private float _cooldown;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
}