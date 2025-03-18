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
    [SerializeField] public bool isPunching;
    [Tooltip("Checks to see if the player is punch so I can pause the combo timer")]
    [SerializeField] public float punchTimer;
    [Tooltip("The time it takes for a punch to reset the can punch timer")]
    [SerializeField] public int punchKnockbackDirection;
    [Tooltip("A keycode to easily identify the direction that a punch should go")]
    [SerializeField] public float punchComboTimer;
    [Tooltip("This is the time it takes for the punch to reset back to the start")]
    [SerializeField] public float punchComboTimerStart;
    [Tooltip("This is the value that sets the time inbetween each punch in a combo")]


    [Header("KeyBinds")]
    [SerializeField] KeyCode shootKey;

    // Start is called before the first frame update
    void Start()
    {
        punchAnimator = GetComponent<Animator>();
        comboManager = GameObject.Find("Combo Manager").GetComponent<ComboManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shootKey) && canPunch && !isPunching)
        {
            //ADD A FUNCTION HERE TO GRAB FROM A DICTONARY THAT CONTAINS ALL THE VALUES FOR EACH PUNCH IN COMBOS
            canPunch = false;

            StartCoroutine(AdvanceCombo(punchAnimator.GetInteger("CurrentComboStage")));
        }

        //This drops the current combo back to the start whenever the timer for the punch ends
        if (punchComboTimer <= 0)
        {
            punchAnimator.SetBool("ComboDropped", true);
            punchAnimator.SetBool("ComboStart", false);
        }
        else
        {
            punchAnimator.SetBool("ComboDropped", false);
            punchComboTimer = punchComboTimer - Time.deltaTime;
        }

        //DEBUG FEATURE (simply transfers the time it takes for 
        punchTimerDebug.text = ("Time till drop: " + (Mathf.Round(punchComboTimer * 100.0f) * 0.01f));

        ////System to help identify what weapon the player currently has equipped
        //switch (Event.current.isKey)
        //{
        //    case 1:
        //        punchAnimator.SetInteger("ComboStartTag",1);
        //        break;
        //}

    }

    IEnumerator AdvanceCombo(int currentPunchState)
    {
        Debug.Log("I have started my punch");

        canPunch = false;

        punchAnimator.SetBool("ComboWait", false);

        if (punchAnimator.GetNextAnimatorStateInfo(0).normalizedTime < 0.7f)
        {
            punchAnimator.SetBool("hit1", true);
            punchAnimator.SetBool("hit2", false);
        }
            punchAnimator.SetBool("ComboStart", false);
            //Advances the punch state up the number three then defaults it back to 0 so the player can move through each punch animation
            if (currentPunchState != 3)
            {
                currentPunchState++;
            }
            else
            {
                currentPunchState = 1;
            }


        //Used to set off the Combo timer to whatever it needs to be for the punch in the combo
        if (!isPunching)
        {
            punchComboTimer = punchComboTimerStart;
        }

        //A function to wait for the animation to finish before the player can punch again and the state resets to the next one
        yield return new WaitForSeconds((punchAnimator.GetCurrentAnimatorStateInfo(0).length));

        punchAnimator.SetBool("ComboWait", true);

        Debug.Log("I have reach the end of my punch");

        canPunch = true;
    }
}
