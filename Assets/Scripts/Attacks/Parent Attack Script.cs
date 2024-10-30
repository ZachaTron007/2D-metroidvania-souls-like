using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParentMeleeAttack : State {

    [SerializeField] public AttackInfo currentAttack;
    public Vector2 lookDirection;
    private int tempDirection;
    protected IEnumerator attack;

    // Start is called before the first frame update
    void Start() {
        interuptable = false;
        tempDirection = unitVariables.GetDirection();
        attack = Attack();
    }
    public override void Enter() {
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
        animator.Play(currentAttack.clip.name);
        yield return new WaitForSeconds(currentAttack.startHitBoxTime);
        currentAttack.attackHitBox.enabled = true;
        currentAttack.attackHitBox.offset = offsetVector();
        yield return new WaitForSeconds(currentAttack.endHitBoxTime);
        currentAttack.attackHitBox.enabled = false;
        float recoveryTime = (currentAttack.startHitBoxTime + currentAttack.endHitBoxTime >= currentAttack.clip.length) ? 0 : ( currentAttack.clip.length - (currentAttack.startHitBoxTime + currentAttack.endHitBoxTime));

        yield return new WaitForSeconds(recoveryTime);
        Exit();
    }
    public override void Exit() {
        currentAttack.attackHitBox.enabled = false;
        StopCoroutine(attack);
        stateDone = true;
    }

}
