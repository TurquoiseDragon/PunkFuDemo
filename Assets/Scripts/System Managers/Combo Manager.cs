using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [Header("Combo UI parts")]
    [SerializeField] private TMP_Text combotText;
    [SerializeField] private Slider comboSlider;
    [SerializeField] public float comboTimer;
    [Tooltip("This is the time it takes for the combo to drop")]
    [SerializeField] public float comboTimerOffset;
    [Tooltip("This is the time it takes for the combo to drop")]

    // Start is called before the first frame update
    void Start()
    {
        combotText = GameObject.Find("Combo Text").GetComponent<TMP_Text>();
        comboSlider = GameObject.Find("Combo Timer").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Controls the combo timer and the slider to let the player know their current combo
        if (comboTimer >= 0)
        {
            comboTimer = comboTimer - Time.deltaTime;
            combotText.text = "Combo Time: " + (Mathf.Round(comboTimer * 100.0f) * 0.01f);
            comboSlider.value = comboTimer;
        }
    }

    public void ResetComboTimer()
    {
        comboTimer = comboTimerOffset;
    }
}
