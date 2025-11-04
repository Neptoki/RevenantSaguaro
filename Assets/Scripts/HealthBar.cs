using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private float lerpSpeed = 0.05f;

    void Update()
    {
        //animation
        if (easeHealthSlider.value != healthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, healthSlider.value, lerpSpeed);
        }
    }

    public void SetSliderMax(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
        easeHealthSlider.value = maxHealth;
    }

    public void SetSlider(float health)
    {
        healthSlider.value = health;
    }
}
