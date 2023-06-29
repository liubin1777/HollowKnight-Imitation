using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] Transform attackPoint;

    private Animator animator;
    private PlayerMover playerMover;
    private float attackCooldown;
    private bool possibleAttack;
    private bool isAttack;
    private bool isSkill;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        Debug.Log(isAttack);
        //AttackCooldown();
    }

    private void OnSkill(InputValue value)
    {
        if (playerMover.IsDash())     // ��� �� ���� �ȵǵ���
            return;

        isSkill = value.isPressed;

        if (isSkill)
        {
            animator.SetTrigger("Skill");

            if (playerMover.InputDir().y > 0)
                Howling();
            else if(playerMover.InputDir().y < 0 && !playerMover.IsGround())
                Dive();
            else
                ShotSoul();
        }
    }

    private void Howling()
    {
        Debug.Log("use Howling");
    }

    private void ShotSoul()
    {
        ShotSoul shotSoul = GameManager.Resource.Instantiate<ShotSoul>("Prefab/Player/Skill/ShotSoul", attackPoint.position, GameObject.Find("PoolManager").transform);
    }

    private void Dive()
    {
        Debug.Log("use Dive");

    }

    private void AttackCooldown()
    {
        if(isAttack)
        {
            return;
        }

        attackCooldown += Time.deltaTime;
    }

    private void OnAttack(InputValue value)
    {
        if (playerMover.IsDash())     // ��� �� ���� �ȵ�
            return;

        isAttack = value.isPressed;

        if (isAttack)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void AttackUp()
    {

    }

    private void JumpAttackDown()
    {

    }
    
    public bool IsAttack()
    {
        return isAttack;
    }

    public bool IsSkill()
    {
        return isSkill;
    }
}
