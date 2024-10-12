using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParentMeleeAttack : State {

    [SerializeField] protected AttackScript currentAttack;

    protected Vector2 offset;
    public Vector2 lookDirection;

    // Start is called before the first frame update
    void Start() {
        interuptable = false;
        offset.x = 0.87f;
        offset.y = 1.27f;
        //attackHitBox = GetComponent<BoxCollider2D>();
    }

    protected Vector2 offsetVector() {
        return new Vector2(unitVariables.direction * offset.x,0);
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
        float hitBoxStartTime = currentAttack.clip.length / 2;
        yield return new WaitForSeconds(hitBoxStartTime);
        currentAttack.attackHitBox.enabled = true;
        currentAttack.attackHitBox.offset = offsetVector();
        float hitBoxStayTime = currentAttack.clip.length / 2;
        yield return new WaitForSeconds(hitBoxStayTime);
        currentAttack.attackHitBox.enabled = false;
        float recoveryTime = currentAttack.clip.length - (hitBoxStayTime + hitBoxStartTime);
        yield return new WaitForSeconds(recoveryTime);
        Exit();
    }
    public override void Exit() {

        stateDone = true;
    }

}
