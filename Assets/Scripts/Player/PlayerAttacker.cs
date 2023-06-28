using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    private Animator animator;
    private PlayerMover playerMover;
    private float attackCooldown;
    private bool isAttack;
    private bool isSkill;
    private bool possibleAttack;

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
        if (playerMover.IsDash())     // ��� �� ���� �ȵ�
            return;

        isSkill = value.isPressed;

        if (isSkill)       // �̶� ���� �ݶ��̴� �����ϸ� �ɰŰ���
            animator.SetTrigger("Skill");
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

        if (isAttack)       // �̶� ���� �ݶ��̴� �����ϸ� �ɰŰ���
            animator.SetTrigger("Attack");
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
