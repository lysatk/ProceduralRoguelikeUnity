using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI: MonoBehaviour
{
    [SerializeField]
    private Slider[] sliders; 
   

    public void SetMaxCooldowns(int[] maxCooldowns)
    {
        if (maxCooldowns.Length != sliders.Length)
        {
            Debug.LogError("The number of maxCooldowns should match the number of sliders.");
            return;
        }

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].maxValue = maxCooldowns[i];
            sliders[i].value = maxCooldowns[i];
        }
    }

    public void UpdateCooldown(int cooldownIndex, int cooldownValue)
    {
        if (cooldownIndex < 0 || cooldownIndex >= sliders.Length)
        {
            Debug.LogError("Invalid cooldownIndex: " + cooldownIndex);
            return;
        }

        sliders[cooldownIndex].value = cooldownValue;
    }
}
