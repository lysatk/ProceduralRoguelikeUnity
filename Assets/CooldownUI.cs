using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI: MonoBehaviour
{
    [SerializeField]
    private Slider[] sliders;

    private Coroutine[] cooldownCoroutines= new Coroutine[5];

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

    public void UpdateCooldown(int cooldownIndex, float cooldownValue)
    {
        if (cooldownIndex < 0 || cooldownIndex >= sliders.Length)
        {
            Debug.LogError("Invalid cooldownIndex: " + cooldownIndex);
            return;
        }

        // Stop any existing coroutine for the same slider before starting a new one
        if (cooldownCoroutines[cooldownIndex] != null)
        {
            StopCoroutine(cooldownCoroutines[cooldownIndex]);
        }

        cooldownCoroutines[cooldownIndex] = StartCoroutine(UpdateCooldownCoroutine(cooldownIndex,cooldownValue));
    }

    private IEnumerator UpdateCooldownCoroutine(int cooldownIndex,float cooldownDuration)
    {
        Slider slider = sliders[cooldownIndex];
        slider.value = 0.0f;

        float timer = 0.0f;

        while (timer < cooldownDuration)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Lerp(0.0f, 1.0f, timer / cooldownDuration);
            yield return null;
        }

        slider.value = 1.0f;

        // Reset the coroutine reference
        cooldownCoroutines[cooldownIndex] = null;
    }
}
