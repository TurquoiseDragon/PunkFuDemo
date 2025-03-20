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
    /// 1. It needs to move the punch hitbox (this could be done through animations)
    /// 2. It needs to refer to a dictonary with all the punch movements and combos, damages, and knockbacks
    /// 3. Keep track of the current punch equiped, the current combo including current and next punch, and current time in the punch animation
    /// 4. Needs to turn the hitbox on and off as needed for each punch
    /// 5. Allow the player to swap from punches freely but reset the combo
    /// </summary>

    [SerializeField] private Animator punchAnimator;
    [SerializeField] private ComboManager comboManager;
    [SerializeField] private TMP_Text punchTimerDebug;

    [Header("Checks, Timers, & Values [DON'T CHANGE THESE VALUES]")]
    [SerializeField] public bool canPunch;
    [Tooltip("Sees if the player can punch or not")]
    [SerializeField] public int punchKnockbackDirection;
    [Tooltip("A keycode to easily identify the direction that a punch should go")]
    [SerializeField] public float punchComboTimer;
    [Tooltip("This is the time it takes for the punch to reset back to the start")]
    [SerializeField] public float punchComboTimerStart;
    [Tooltip("This is the value that sets the time inbetween each punch in a combo")]
    [SerializeField] public int savedPunchState;
    [SerializeField] public bool startCheckingAnimation;
    [SerializeField] public float coolDownTimer;

    [SerializeField] public int numOfClicks;
    [Tooltip("A number to cache the amount of times the player clicks so they can't spam through the attacks")]

    [Header("KeyBinds")]
    [SerializeField] KeyCode shootKey;

    // Start is called before the first frame update
    void Start()
    {
        punchAnimator = GetComponent<Animator>();
        comboManager = GameObject.Find("Combo Manager").GetComponent<ComboManager>();
        canPunch = true;
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
            //ADD A FUNCTION HERE TO GRAB FROM A DICTONARY THAT CONTAINS ALL THE VALUES FOR EACH PUNCH IN COMBOS
            float waitTime = 1;

            canPunch = false;

            numOfClicks = 0;

            AdvanceCombo(savedPunchState, waitTime);
        }

        if (coolDownTimer > 0)
        {
            coolDownTimer = coolDownTimer - Time.deltaTime;
        }
        else
        {
            numOfClicks = 0;

            canPunch = true;

            punchAnimator.SetBool("hit" + savedPunchState.ToString(), false);

            startCheckingAnimation = true;
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
    
    void AdvanceCombo(int currentPunchState, float waitTime)
    {
        Debug.Log("I have started my punch");

        coolDownTimer = waitTime;

        //Advances the punch state up the number three then defaults it back to 0 so the player can move through each punch animation

        if (currentPunchState != 3)
        {
            currentPunchState++;
        }
        else
        {
            currentPunchState = 1;
        }

        //Caches the punch state for later use in the same function
        savedPunchState = currentPunchState;


        //Used to set off the Combo timer to whatever it needs to be for the punch in the combo
       punchComboTimer = punchComboTimerStart;

        //sets the hit[num] bool so the animation can advance
        punchAnimator.SetBool("hit" + currentPunchState.ToString(), true);
    }
}
