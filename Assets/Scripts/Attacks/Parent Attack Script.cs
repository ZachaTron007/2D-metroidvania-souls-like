using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParentMeleeAttack : State {

    public AttackInfo currentAttack;
    public float attackTime;
    [SerializeField] protected float attackSpeedModifier = 1;
    [SerializeField] private float attackLungeSpeed;
    //[SerializeField] private float attackSpeed = 1;
    public Vector2 lookDirection;
    private int tempDirection;
    protected IEnumerator attack;

    protected void Start() {
        clip = unitVariables.animations.parryAnimation;
        
        interuptable = .8f;
        tempDirection = unitVariables.GetDirection();
        attack = Attack();
    }
    public override void Enter() {
        clip = currentAttack.clip;
        base.Enter();
        attack = Attack();

    }
    protected Vector2 offsetVector() {
        return new Vector2(unitVariables.GetDirection() * Mathf.Abs(currentAttack.attackHitBox.offset.x), Mathf.Abs(currentAttack.attackHitBox.offset.y));
    }
    /*
     * summary:
     *  1. starts playing the attack animation
     *  2. sets a time for the hitbox to activate
     *  2.5. sets the hitbox to activate and offset
     *  3. sets a time for the hitbox to stay
     *  3.5. disables the hitbox
     *  4. waits for the attack animation to finish, if it hasn't finished already
     *  5. starts the recovery state
     */
    protected IEnumerator Attack() {
        float attackSpeed = currentAttack.speed * 1/attackSpeedModifier;
        float length = (currentAttack.length * attackSpeed);// - currentAttack.clip.frameRate * attackSpeed;
        interuptable = .1f;
        animator.speed = currentAttack.speed;
        
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);// Vector2.zero;
        float startMovingTime = currentAttack.startMovingTime * attackSpeed;
        float startHitBoxTime = currentAttack.startHitBoxTime * attackSpeed;
        yield return new WaitForSeconds(startHitBoxTime * attackSpeed);
        //rb.linearVelocity = Vector2.zero;
        interuptable = .6f;
        currentAttack.attackHitBox.enabled = true;
        currentAttack.attackHitBox.offset = offsetVector();
        float endHitBoxTime = currentAttack.endHitBoxTime * attackSpeed;
        yield return new WaitForSeconds(endHitBoxTime);
        //rb.linearVelocity = Vector2.zero;
        currentAttack.attackHitBox.enabled = false;
        float recoveryTime = (startHitBoxTime + endHitBoxTime >= length) ? 0 : ( length - (startHitBoxTime + endHitBoxTime + startMovingTime));/*
        Debug.Log("--------------------------------");
        Debug.Log(currentAttack.name);
        Debug.Log("Time till state done: "+recoveryTime);
        */
        yield return new WaitForSeconds(recoveryTime);
        //Debug.Log("State Should be Done");
        StateIsDone();
        yield return null;
    }
    protected override void StateIsDone() {
        base.StateIsDone();
        //Debug.Log("Done: "+IsStateDone());
        currentAttack.attackHitBox.enabled = false;
        animator.speed = 1;
        StopCoroutine(attack);
    }

}
