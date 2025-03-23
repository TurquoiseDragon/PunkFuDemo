using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

public class ModularPunchingBehavior : MonoBehaviour
{
    /// <summary>
    /// THIS SCRIPT NEEDS TO DO THESE THINGS
    /// 1. It needs to refer to a dictonary with all the punch movements and combos, damages, and knockbacks
    /// 2. Allow the player to swap from punches freely but reset the combo
    /// </summary>

    [SerializeField] private Animator punchAnimator;
    [SerializeField] private ComboManager comboManager;
    [SerializeField] private TMP_Text punchTimerDebug;
    [SerializeField] private WeaponValueContainer weaponValueContainer;

    #region Checks & Values
    [Header("Checks, Timers, & Values [DON'T CHANGE THESE VALUES]")]
    [SerializeField] public bool canPunch;
    [Tooltip("Sees if the player can punch or not")]
    [SerializeField] public float punchComboTimer;
    [Tooltip("This is the time it takes for the punch to reset back to the start")]
    [SerializeField] public int savedPunchState;
    [Tooltip("This is the current cached state of the punch, used for advancing the combo & collecting values")]
    [SerializeField] public float coolDownTimer;
    [Tooltip("This is the value that keeps track of the cooldown inbetween punches")]
    [SerializeField] public int numOfClicks;
    [Tooltip("A number to cache the amount of times the player clicks so they can't spam through the attacks")]
    #endregion

    #region Weapon Values
    [Header("Punch Values [These get changed by the valuecontainer")]
    [SerializeField] public int punchKnockbackDirection;
    [Tooltip("A key to easily identify the direction that a punch should go")]
    [SerializeField] public float punchDamage;
    [Tooltip("A value to assign to the punch each time it's thrown, it's just the damage of the punch")]
    [SerializeField] public float knockbackValue;
    [Tooltip("A value to find how far each punch should knock back enemies (might just be used on death)")]
    [SerializeField] public float punchComboTimerStart;
    [Tooltip("This is the value that sets the time that the player can continue a punch combo without dropping")]
    [SerializeField] public float waitTimeStart;
    [Tooltip("The value that the player HAS to wait before throwing another punch, used to make sure error don't appear")]

    #endregion

    [SerializeField] private int currentWeaponEquiped;
    [Tooltip("The token to know what weapon the player currently has equiped")]

    [Header("KeyBinds")]
    [SerializeField] KeyCode shootKey;

    /// <summary>
    /// WHAT ELSE NEEDS WORK NOW?
    /// 1.ensure that the values are getting set in time
    /// 2.figure out a way to attach the direction to a numeric key
    /// 3.apply all of thes values to enemies
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        comboManager = GameObject.Find("Combo Manager").GetComponent<ComboManager>();
        canPunch = true;
        savedPunchState = 1;
        coolDownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Chaches the number of clicks so that the player can't spam and break the animation loop
        if (Input.GetKeyDown(shootKey))
        {
            numOfClicks++;
        }

        //checks to see if the number of clicks is over the current cache and if the player is able to punch
        if (numOfClicks >= 1 && canPunch && coolDownTimer <= 0)
        {

            canPunch = false;

            numOfClicks = 0;

            AdvanceCombo(savedPunchState);
        }


        //This is the function that counts down the cooldown timer and then resets the players combo if it hits zero
        if (coolDownTimer > 0)
        {
            coolDownTimer = coolDownTimer - Time.deltaTime;
        }
        else
        {
            numOfClicks = 0;

            canPunch = true;

            if (savedPunchState != 0)
            {
                punchAnimator.SetBool("hit" + savedPunchState.ToString(), false);
            }
        }

        //This drops the current combo back to the start whenever the timer for the punch ends
        if (punchComboTimer <= 0)
        {
            punchAnimator.SetBool("ComboDropped", true);
            savedPunchState = 0;
        }
        else
        {
            punchAnimator.SetBool("ComboDropped", false);
            punchComboTimer = punchComboTimer - Time.deltaTime;
        }

        //DEBUG FEATURE (simply transfers the time it takes for 
        punchTimerDebug.text = ("Time till drop: " + (Mathf.Round(punchComboTimer * 100.0f) * 0.01f));
    }
    
    void AdvanceCombo(int currentPunchState)
    {
        ///DEBUGS
        //Debug.Log("I have started my punch");
        //Debug.Log("the value before being changed is " + currentPunchState);


        //Advances the punch state up the number three then defaults it back to 0 so the player can move through each punch animation

        if (currentPunchState >= 3 || currentPunchState == 0)
        {
            currentPunchState = 1;
        }
        else
        {
            ++currentPunchState;
        }

        //Caches the punch state for later use in the same function
        savedPunchState = currentPunchState;

        //Debug.Log("the value after being changed is " + savedPunchState);

        //Grabs the values from the value contaner for the currently selected weapon
        GrabFromWeaponDictonary(savedPunchState);

        //Used to set off the Combo timer to whatever it needs to be for the punch in the combo
        punchComboTimer = punchComboTimerStart;


        //COOL DOWN TIMER ALWAYS HAS TO BE LOWER THAN THE COMBO PUNCH TIMER
        //IF IT ISN'T THE PUNCH WON'T BE ABLE TO PROGRESS TO THE NEXT STAGE
        coolDownTimer = waitTimeStart;

        //sets the hit[num] bool so the animation can advance
        punchAnimator.SetBool("hit" + currentPunchState.ToString(), true);
    }

    void GrabFromWeaponDictonary(int currentHitInCombo)
    {
        weaponValueContainer.WeaponValueOutput(currentHitInCombo);
    }
}
