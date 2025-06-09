using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponValueContainer : MonoBehaviour
{
    [SerializeField] private ModularPunchingBehavior mPB;
    [SerializeField] private HitboxBehavior hBB;

    [Header("Weapon Values")]
    [SerializeField] public List<int> hitDamages = new List<int>();
    [SerializeField] public List<int> hitKnockbackUp = new List<int>();
    [SerializeField] public List<int> hitKnockbackFoward = new List<int>();
    [SerializeField] public List<int> hitKnockbackSide = new List<int>();
    [SerializeField] public List<float> waitTimes = new List<float>();
    [SerializeField] public List<float> comboDropTimes = new List<float>();

    private void Start()
    {
      mPB = GameObject.Find("PunchController").GetComponent<ModularPunchingBehavior>();
      hBB = mPB.hitboxBehavior.gameObject.GetComponent<HitboxBehavior>();
    }

    public void WeaponValueOutput(int hitValueInput)
    {
        //Debug.Log("The value the list is grabbing is " + hitValueInput);

        /// Sets the values of the punch to whatever current hit is in the list, this is added in the inspector

        //Sets the values of the punchs knockback
        hBB.KnockbackUp = hitKnockbackUp[hitValueInput];
        hBB.KnockbackFoward = hitKnockbackFoward[hitValueInput];
        hBB.KnockbackSide = hitKnockbackSide[hitValueInput];

        //Sets the values of the punches damage
        hBB.punchDamage = hitDamages[hitValueInput];

        //sets the time it takes to throw another punch
        mPB.waitTimeStart = waitTimes[hitValueInput];
        //sets the time until the combo is dropped in hit series
        mPB.punchComboTimerStart = comboDropTimes[hitValueInput];
    }
}
