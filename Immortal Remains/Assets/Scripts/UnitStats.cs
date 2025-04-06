using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField] private float HP;
    [SerializeField] private float ATK;
    [SerializeField] private float DEF;
    [SerializeField] private float movSPD;
    [SerializeField] private float atkSPD;
    [SerializeField] private float Range;
    void Start()
    {
        
    }

    private float calcAttack ()
    {
        return ATK;
    }

    private float calcDef ()
    {
        return ATK;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
