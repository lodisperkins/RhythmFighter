using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private float _maxRoundTime;
    [SerializeField]
    private float _remainingRoundTime;

    private float _roundStartTime;
    private float _elapsedRoundTime;

    [SerializeField]
    private HealthBehaviour _player1HealthBehavior;
    [SerializeField]
    private HealthBehaviour _player2HealthBehavior;

    private float _player1HealthPercentage = 1;
    private float _player2HealthPercentage = 1;

    [SerializeField]
    private Slider _player1HealthSlider;
    [SerializeField]
    private Image _player1Fill;

    [SerializeField]
    private Slider _player2HealthSlider;
    [SerializeField]
    private Image _player2Fill;

    [SerializeField]
    private Gradient _sliderGradient;
    [SerializeField]
    private TextMeshProUGUI _roundTimer;

    void Awake()
    {
        _roundStartTime = Time.time;
        _remainingRoundTime = _maxRoundTime;

        _player1HealthBehavior.AddOnTakeDamageAction(UpdatePlayerHealthUI);
        _player2HealthBehavior.AddOnTakeDamageAction(UpdatePlayerHealthUI);
    }

    public void UpdatePlayerHealthUI(GameObject attacker)
    {
        if(attacker == _player2HealthBehavior.gameObject)
        {
            _player1HealthPercentage = _player1HealthBehavior.Health / _player1HealthBehavior.MaxHealth;
            _player1HealthSlider.value = _player1HealthPercentage;
            _player1Fill.color = _sliderGradient.Evaluate(_player1HealthSlider.value);
        }
        else
        {
            _player2HealthPercentage = _player2HealthBehavior.Health / _player2HealthBehavior.MaxHealth;
            _player2HealthSlider.value = _player2HealthPercentage;
            _player2Fill.color = _sliderGradient.Evaluate(_player2HealthSlider.value);
        }
    }

    public void RoundStart()
    {
        _roundStartTime = Time.time;
        _remainingRoundTime = _maxRoundTime;
    }

    void Update()
    {
        if (Time.time - _elapsedRoundTime >= 1) { _elapsedRoundTime++; _remainingRoundTime--; _roundTimer.text = _remainingRoundTime.ToString(); }
    }
}
