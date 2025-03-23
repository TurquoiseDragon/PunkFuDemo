using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{

    [Header("Collision Test")]
    [SerializeField] public LayerMask TestingLayer;
    [SerializeField] public GameObject damageNumPrefab;
    [SerializeField] private ComboManager comboManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //a script to purly test what is currently being collided with 
        if (other.gameObject.tag == "PlayerHitbox")
        {
            Debug.Log("I got hit 1");
            comboManager.ResetComboTimer();

            ///DEBUGS FOR ALL VALUES
            Debug.Log("Current Damage Value of " + other.GetComponentInParent<ModularPunchingBehavior>().savedPunchState + " is " + other.GetComponentInParent<ModularPunchingBehavior>().punchDamage);
            Debug.Log("Current Knockback Value of " + other.GetComponentInParent<ModularPunchingBehavior>().savedPunchState + " is " + other.GetComponentInParent<ModularPunchingBehavior>().knockbackValue);
            Debug.Log("Current Knockback Direction of " + other.GetComponentInParent<ModularPunchingBehavior>().savedPunchState + " is " + other.GetComponentInParent<ModularPunchingBehavior>().punchKnockbackDirection);
        }
        else
        {
            Debug.Log("I got hit 2");
        }
    }
}
