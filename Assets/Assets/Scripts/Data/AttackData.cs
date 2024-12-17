using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/Create New Attack Data")]
public class AttackData : ScriptableObject
{
    public string AnimName => _animName;

    /// <summary>
    /// ���������, ����� ������������ ������ ������������
    /// </summary>
    public AttackHitbox HitboxToUse => _hitboxToUse;

    /// <summary>
    /// ���������� ��������� �� ����� ��������, ������� ������ ������, ����� ������ �����
    /// </summary>
    public float ImpactStartTime => _impactStartTime;

    /// <summary>
    /// ���������� ��������� �� ����� ��������, ������� ������ ������, ����� ��������� �����
    /// </summary>
    public float ImpactEndTime => _impactEndTime;

    [SerializeField] private string _animName;
    [SerializeField] private AttackHitbox _hitboxToUse;
    [SerializeField] private float _impactStartTime;
    [SerializeField] private float _impactEndTime;
}

public enum AttackHitbox { LeftHand, RightHand, LeftFoot, RightFoot };