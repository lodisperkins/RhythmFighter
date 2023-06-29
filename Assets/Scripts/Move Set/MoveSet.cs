using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSet : MonoBehaviour
{
    private bool _isAttacking = false;
    public bool IsAttacking
    {
        get { return _isAttacking; }
        private set { _isAttacking = value; }
    }

    void Awake()
    {
        
    }

    void Update()
    {
        
    }
}
