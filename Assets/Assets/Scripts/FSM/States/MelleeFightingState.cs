using System.Collections;
using UnityEngine;

public enum AttackState { Idle, Windup, Impact, Cooldown }

public class MelleeFightingState : PlayerOnFootState
{
    private MonoBehaviour _monoBehaviour;
    private AttackState _attackState;
    private SphereCollider _leftHandCollider, _rightHandCollider, _leftFootCollider, _rightFootCollider;
    private bool _inAction = false;
    private bool _doCombo = false;
    private int _comboCount = 0;
    private Coroutine _attackCoroutine;

    public MelleeFightingState(StateMachine stateMachine, PlayerConfig config, MonoBehaviour monoBehaviour) : base(stateMachine, config)
    {
        _monoBehaviour = monoBehaviour;
        _leftHandCollider = Config.Animator.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<SphereCollider>();
        _rightHandCollider = Config.Animator.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<SphereCollider>();
        _leftFootCollider = Config.Animator.GetBoneTransform(HumanBodyBones.LeftFoot).GetComponent<SphereCollider>();
        _rightFootCollider = Config.Animator.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<SphereCollider>();
        DisableAllHitboxes();      
    }

    public override void Enter()
    {
        TryToAttack();
    }

    public override void HandleInput()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            TryToAttack();
        }
    }

    public override void LogicUpdate()
    {
        Config.Animator.SetFloat("moveAmount", MoveAmount, 0.3f, Time.deltaTime);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {

        _monoBehaviour.StopCoroutine(_attackCoroutine);
    }

    private void TryToAttack()
    {
        if (!_inAction)
        {
            _attackCoroutine = _monoBehaviour.StartCoroutine(Attack());
        }
        // выполняем комбо, как только текущая атака достигнет стадии удара или восстановления
        else if (_attackState == AttackState.Impact || _attackState == AttackState.Cooldown)
        {
            _doCombo = true;
        }
    }

    private IEnumerator Attack()
    {
        _inAction = true;
        _attackState = AttackState.Windup;
        Config.Animator.CrossFade(Config.Attacks[_comboCount].AnimName, 0.2f);
        yield return null;
        var animState = Config.Animator.GetNextAnimatorStateInfo(1);

        float timer = 0f;
        while (timer <= animState.length) // пока значение таймера будет не больше длительности анимации, мы будем ждать 1 кадр
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;
            if (_attackState == AttackState.Windup)
            {
                if (normalizedTime >= Config.Attacks[_comboCount].ImpactStartTime)
                {
                    _attackState = AttackState.Impact;
                    EnableHitbox(Config.Attacks[_comboCount]);
                }
            }
            else if (_attackState == AttackState.Impact)
            {
                if (normalizedTime >= Config.Attacks[_comboCount].ImpactEndTime)
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
                    _comboCount = (_comboCount + 1) % Config.Attacks.Count;
                    _attackCoroutine = _monoBehaviour.StartCoroutine(Attack());
                    yield break;
                }
            }

            yield return null; // ждем 1 кадр
        }

        _attackState = AttackState.Idle;
        _comboCount = 0;
        _inAction = false;

        StateMachine.ChangeState<IdleState>();
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
        _leftHandCollider.enabled = false;
        _rightHandCollider.enabled = false;
        _leftFootCollider.enabled = false;
        _rightFootCollider.enabled = false;
    }
}
