using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponValueContainer : MonoBehaviour
{
    [SerializeField] private ModularPunchingBehavior mPB;

    [Header("Weapon Values")]
    [SerializeField] public List<int> hitDamages = new List<int>();
    [SerializeField] public List<int> hitKnockbackDirection = new List<int>();
    [SerializeField] public List<int> hitKnockbackValue = new List<int>();
    [SerializeField] public List<float> waitTimes = new List<float>();
    [SerializeField] public List<float> comboDropTimes = new List<float>();

    private void Start()
    {
      mPB = GameObject.Find("PunchController").GetComponent<ModularPunchingBehavior>();   
    }

    public void WeaponValueOutput(int hitValueInput)
    {
        //Debug.Log("The value the list is grabbing is " + hitValueInput);

        /// Sets the values of the punch to whatever current hit is in the list, this is added in the inspector

        //sets the direction of the punches knockback
        mPB.punchKnockbackDirection = hitKnockbackDirection[hitValueInput];
        //Sets the values of the punches damage
        mPB.punchDamage = hitDamages[hitValueInput];
        //sets the values of the knockback
        mPB.knockbackValue = hitKnockbackValue[hitValueInput];
        //sets the time it takes to throw another punch
        mPB.waitTimeStart = waitTimes[hitValueInput];
        //sets the time until the combo is dropped in hit series
        mPB.punchComboTimerStart = comboDropTimes[hitValueInput];
    }
}
