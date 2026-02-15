using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class FlotingHealthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UppdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }
}
