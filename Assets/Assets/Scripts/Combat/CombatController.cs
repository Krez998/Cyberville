using System;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private MeeleFighter _meeleFighter;

    private void Awake()
    {
        _meeleFighter = GetComponent<MeeleFighter>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _meeleFighter.TryToAttack();
        }
    }
}
