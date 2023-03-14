using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] Monster _monsterBase;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void DieEnd()
    {
        _monsterBase.DieEnd();
    }
}
