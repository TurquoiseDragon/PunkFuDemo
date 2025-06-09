using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBehavior : MonoBehaviour
{
    #region Punch Values
    [SerializeField] public int KnockbackUp;
    [Tooltip("A value to assign to the upwards velocity of knockback")]
    [SerializeField] public int KnockbackFoward;
    [Tooltip("A value to assign to the Forward velocity of knockback")]
    [SerializeField] public int KnockbackSide;
    [Tooltip("A value to assign to the side velocity of knockback")]
    [SerializeField] public float punchDamage;
    [Tooltip("A value to find how far each punch should knock back enemies (might just be used on death)")]
    #endregion

    private void OnCollisionEnter(Collision other)
    {
        ///DEBUGS FOR ALL VALUES
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Current Damage Value of Punch is " + punchDamage);
            Debug.Log("Current Knockback Value of is " + KnockbackUp + " upwards and " + KnockbackFoward + " Forwards and " + KnockbackSide + " to the side");
        }
      
    }
}
