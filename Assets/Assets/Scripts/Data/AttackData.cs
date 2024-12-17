using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/Create New Attack Data")]
public class AttackData : ScriptableObject
{
    public string AnimName => _animName;

    /// <summary>
    /// Указывает, какой коллизионный объект используется
    /// </summary>
    public AttackHitbox HitboxToUse => _hitboxToUse;

    /// <summary>
    /// Количество процентов от длины анимации, которое должно пройти, чтобы начать атаку
    /// </summary>
    public float ImpactStartTime => _impactStartTime;

    /// <summary>
    /// Количество процентов от длины анимации, которое должно пройти, чтобы завершить атаку
    /// </summary>
    public float ImpactEndTime => _impactEndTime;

    [SerializeField] private string _animName;
    [SerializeField] private AttackHitbox _hitboxToUse;
    [SerializeField] private float _impactStartTime;
    [SerializeField] private float _impactEndTime;
}

public enum AttackHitbox { LeftHand, RightHand, LeftFoot, RightFoot };