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

    private bool _inAction = false; // отслеживает, выполняет ли игрок какое-то действие
    public bool _doCombo = false;
    public int _comboCount = 0;

    /// <summary>
    /// Выполняет атаку если игрок не находится в процессе действия
    /// </summary>
    public void TryToAttack()
    {
        // если игрок не выполняет никаких действий, мы можем провести атаку
        if (!_inAction)
        {
            StartCoroutine(Attack());
        }
        // выполняем комбо, как только текущая атака достигнет стадии удара или восстановления
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
    /// Когда мы атакуем, мы меняем состояние атаки в зависимости от времени анимации
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack()
    {
        _inAction = true;
        _attackState = AttackState.Windup;


        // CrossFade позволяет переходить к нужной нам анимации, вместо того чтобы сразу ее воспроизвести
        // 0.2f означает на переход к анимации удара, уйдет 20% времени текущей анимации
        _animator.CrossFade(_attacks[_comboCount].AnimName, 0.2f); // CrossFadeInFixedTime принимает значение перехода анимации в секундах
                                                                   // CrossFadeInFixedTime - фнукция плавного перехода по времени, принимает значение перехода анимации в секундах
                                                                   // но лучше использовать фнукцию плавного перехода по времени, потому что если текущая анимация длится долго
                                                                   // то переход займет больше времени

        // когда мы вызываем функцию перехода, Unity не начинает переход к новой анимации сразу, она запустится только после ткущего кадра
        // значит нам нужно подождать 1 кадр, прежде чем вызывать функцию получения состояния Animator
        // иначе мы получим некорректное состояние, потому что переход еще не начался
        yield return null; // таким образом наша сопрограмма будет ждать 1 кадр

        // в Animator нет способа узнать когда анимация закончится
        var animState = _animator.GetNextAnimatorStateInfo(1); // когда вызовется эта строка, наш Animator будет находится в переходном состоянии 
        // а в переходном состоянии будет наша анимация передвижения, а следующей анимацией будет анимация атаки
        // 1 - Override Layer, там находится анимация атаки


        // Когда мы атакуем, мы меняем состояние атаки в зависимости от времени анимации
        float timer = 0f;
        while (timer <= animState.length) // пока значение таймера будет не больше длительности анимации, мы будем ждать 1 кадр
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

            yield return null; // ждем 1 кадр
        }

        _attackState = AttackState.Idle;
        _comboCount = 0;
        _inAction = false;
    }

    /// <summary>
    /// Если эта функция вызывается, значит какой-то триггер вошел в коллайдер персонажа
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // если другой коллайдер который столкнулся с коллайдером персонажа, является Hitbox
        // то мы должны воспроизвести анимацию удара
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
