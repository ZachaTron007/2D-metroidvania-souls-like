using UnityEngine;

public class PlayerAttackInfo : AttackInfo
{
    [Header("Effect Info")]
    [SerializeField] private float destroyDelay = .3f;
    [SerializeField] private float effectSpeed = 1;
    [SerializeField] private float xOffset, yOffSet;
    [SerializeField] private GameObject eeffect;
    public override void VisualEffect() {
        base.VisualEffect();
        PlayerState player = HelperFunctions.getParentTransfromComponent<PlayerState>(transform);
        Vector3 startPos = player.transform.position;
        GameObject effect = Instantiate(eeffect, new Vector3(startPos.x+(xOffset*player.GetDirection()),startPos.y+yOffSet,startPos.z), new Quaternion(0,0,0,0));
        Destroy(effect, destroyDelay);
        effect.GetComponent<Animator>().speed = effectSpeed;


    }
}
