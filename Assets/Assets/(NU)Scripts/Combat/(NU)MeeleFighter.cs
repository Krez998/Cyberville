using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum AttackState { Idle, Windup, Impact, Cooldown }

public class NU_MeeleFighter : MonoBehaviour
{
    public bool InAction => _inAction;

    [SerializeField] private List<AttackData> _attacks;

    [SerializeField] GameObject _sword;
    private BoxCollider _swordCollider;
    [SerializeField] private SphereCollider _leftHandCollider, _rightHandCollider, _leftFootCollider, _rightFootCollider;

    private Animator _animator;

    [SerializeField] private AttackState _attackState;

    private bool _inAction = false; // �����������, ��������� �� ����� �����-�� ��������
    public bool _doCombo = false;
    public int _comboCount = 0;

    /// <summary>
    /// ��������� ����� ���� ����� �� ��������� � �������� ��������
    /// </summary>
    public void TryToAttack()
    {
        // ���� ����� �� ��������� ������� ��������, �� ����� �������� �����
        if (!_inAction)
        {
            StartCoroutine(Attack());
        }
        // ��������� �����, ��� ������ ������� ����� ��������� ������ ����� ��� ��������������
        else if (_attackState == AttackState.Impact || _attackState == AttackState.Cooldown)
        {
            _doCombo = true;
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if(_sword != null)
        {
            _swordCollider = _sword.GetComponent<BoxCollider>();
            _leftHandCollider = _animator.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<SphereCollider>();
            _rightHandCollider = _animator.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<SphereCollider>();
            _leftFootCollider = _animator.GetBoneTransform(HumanBodyBones.LeftFoot).GetComponent<SphereCollider>();
            _rightFootCollider = _animator.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<SphereCollider>();

            DisableAllHitboxes();
        }
    }

    /// <summary>
    /// ����� �� �������, �� ������ ��������� ����� � ����������� �� ������� ��������
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack()
    {
        _inAction = true;
        _attackState = AttackState.Windup;


        // CrossFade ��������� ���������� � ������ ��� ��������, ������ ���� ����� ����� �� �������������
        // 0.2f �������� �� ������� � �������� �����, ����� 20% ������� ������� ��������
        _animator.CrossFade(_attacks[_comboCount].AnimName, 0.2f); // CrossFadeInFixedTime ��������� �������� �������� �������� � ��������
                                                                   // CrossFadeInFixedTime - ������� �������� �������� �� �������, ��������� �������� �������� �������� � ��������
                                                                   // �� ����� ������������ ������� �������� �������� �� �������, ������ ��� ���� ������� �������� ������ �����
                                                                   // �� ������� ������ ������ �������

        // ����� �� �������� ������� ��������, Unity �� �������� ������� � ����� �������� �����, ��� ���������� ������ ����� ������� �����
        // ������ ��� ����� ��������� 1 ����, ������ ��� �������� ������� ��������� ��������� Animator
        // ����� �� ������� ������������ ���������, ������ ��� ������� ��� �� �������
        yield return null; // ����� ������� ���� ����������� ����� ����� 1 ����

        // � Animator ��� ������� ������ ����� �������� ����������
        var animState = _animator.GetNextAnimatorStateInfo(1); // ����� ��������� ��� ������, ��� Animator ����� ��������� � ���������� ��������� 
        // � � ���������� ��������� ����� ���� �������� ������������, � ��������� ��������� ����� �������� �����
        // 1 - Override Layer, ��� ��������� �������� �����


        // ����� �� �������, �� ������ ��������� ����� � ����������� �� ������� ��������
        float timer = 0f;
        while (timer <= animState.length) // ���� �������� ������� ����� �� ������ ������������ ��������, �� ����� ����� 1 ����
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;
            if (_attackState == AttackState.Windup)
            {
                if (normalizedTime >= _attacks[_comboCount].ImpactStartTime)
                {
                    _attackState = AttackState.Impact;
                    EnableHitbox(_attacks[_comboCount]);
                }
            }
            else if (_attackState == AttackState.Impact)
            {
                if (normalizedTime >= _attacks[_comboCount].ImpactEndTime)
                {
                    _attackState = AttackState.Cooldown;
                    DisableAllHitboxes();
                }
            }
            else if (_attackState == AttackState.Cooldown)
            {
                if (_doCombo)
                {
                    _doCombo = false;
                    _comboCount = (_comboCount + 1) % _attacks.Count;

                    StartCoroutine(Attack());
                    yield break;
                }
            }

            yield return null; // ���� 1 ����
        }

        _attackState = AttackState.Idle;
        _comboCount = 0;
        _inAction = false;
    }

    /// <summary>
    /// ���� ��� ������� ����������, ������ �����-�� ������� ����� � ��������� ���������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // ���� ������ ��������� ������� ���������� � ����������� ���������, �������� Hitbox
        // �� �� ������ ������������� �������� �����
        if(other.CompareTag("Hitbox") && !InAction)
        {
            Debug.Log("Dont hurt me!");
            StartCoroutine(PlayHitReaction());
        }
    }


    private IEnumerator PlayHitReaction()
    {
        _inAction = true;

        yield return null; 

        _animator.CrossFade("Hit To Body", 0.2f);

        var animState = _animator.GetNextAnimatorStateInfo(1);  

        yield return new WaitForSeconds(animState.length * 0.8f);

        _inAction = false;
    }

    private void EnableHitbox(AttackData attackData)
    {
        switch (attackData.HitboxToUse)
        {
            case AttackHitbox.LeftHand:
                _leftHandCollider.enabled = true;
                break;
            case AttackHitbox.RightHand:
                _rightHandCollider.enabled = true;
                break;
            case AttackHitbox.LeftFoot:
                _leftFootCollider.enabled = true;
                break;
            case AttackHitbox.RightFoot:
                _rightFootCollider.enabled = true;
                break;
            default:
                break;
        }
    }

    private void DisableAllHitboxes()
    {
        _swordCollider.enabled = false;
        _leftHandCollider.enabled = false;
        _rightHandCollider.enabled = false;
        _leftFootCollider.enabled = false;
        _rightFootCollider.enabled = false;
    }
}
