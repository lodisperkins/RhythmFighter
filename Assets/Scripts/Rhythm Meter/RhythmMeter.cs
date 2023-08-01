using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RhythmMeter : MonoBehaviour
{
    [SerializeField]
    private GameObject _rhythmBarObject;
    [SerializeField]
    private Transform _leftSpawnTrans;
    [SerializeField]
    private Transform _rightSpawnTrans;
    [SerializeField]
    private Transform _meterMiddleTrans;

    private float _currentTime = 0;

    private static bool _onBeat;
    public static bool OnBeat
    {
        set { _onBeat = value; }
        get { return _onBeat; }
    }

    void Awake()
    {
        InvokeRepeating("SpawnRhythmBar", 0, 0.5f);       
    }

    private void SpawnRhythmBar()
    {
        GameObject leftBar = Instantiate(_rhythmBarObject, _leftSpawnTrans);
        GameObject rightBar = Instantiate(_rhythmBarObject, _rightSpawnTrans);
    }

    //Checks whether the player attacked 5 frames before or 3 frames after the downbeat. Current set to be constantly checked, but could be checked only when an attack is input
    public void CheckThis()
    {
        _currentTime = Time.time;
        if (_currentTime % 1 >= 25f / 60f && _currentTime % 1 <= 33f / 60f || _currentTime % 1 <= 3f / 60f || _currentTime % 1 >= 55f / 60f)
            _onBeat = true;
        else
            _onBeat = false;
    }

    void Update()
    {
        CheckThis();
    }

}
