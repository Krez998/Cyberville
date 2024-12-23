using System;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private NU_MeeleFighter _meeleFighter;

    private void Awake()
    {
        _meeleFighter = GetComponent<NU_MeeleFighter>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _meeleFighter.TryToAttack();
        }
    }
}
