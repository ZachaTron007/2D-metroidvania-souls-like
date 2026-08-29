using System.Collections;
using TMPro.Examples;
using UnityEngine;

public class MeleeAttack : ParentMeleeAttack
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected IEnumerator attack;

    public override void Enter() {
        base.Enter();
        attack = AttackCoroutine();
        StartCoroutine(attack);

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
    public IEnumerator AttackCoroutine() {
        float attackSpeed = currentAttack.speed * 1 / attackSpeedModifier;
        float length = (currentAttack.length * attackSpeed);// - currentAttack.clip.frameRate * attackSpeed;
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);// Vector2.zero;
        float startMovingTime = currentAttack.startMovingTime * attackSpeed;
        float startHitBoxTime = currentAttack.startHitBoxTime * attackSpeed;
        yield return new WaitForSeconds(startHitBoxTime * attackSpeed);
        //rb.linearVelocity = Vector2.zero;
        interuptable = .6f;
        SetHitBoxStatus(true);
        currentAttack.attackHitBox.offset = offsetVector();
        float endHitBoxTime = currentAttack.endHitBoxTime * attackSpeed;
        yield return new WaitForSeconds(endHitBoxTime);
        //rb.linearVelocity = Vector2.zero;
        SetHitBoxStatus(false);
        float recoveryTime = (startHitBoxTime + endHitBoxTime >= length) ? 0 : (length - (startHitBoxTime + endHitBoxTime + startMovingTime));

        yield return new WaitForSeconds(recoveryTime);
        //Debug.Log("State Should be Done");
        StateIsDone();
        yield return null;
    }
    public override void Exit() {
        base.Exit();
        interuptable = .1f;
        StopCoroutine(attack);
    }
}
