using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float maxHitPoints = 100f;
    float hitPoints;

    public Slider healthSlider;

    void Start()
    {
        hitPoints = maxHitPoints;
        SetHealthSlider();
    }

    void Hit(float rawDamage)
    {
        hitPoints -= rawDamage;
        SetHealthSlider();
        Debug.Log("OUCH: " + hitPoints.ToString());

        if (hitPoints <= 0)
        {
            OnDeath();
        }
    }

    void SetHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = NormalisedHitPoint();
        }
    }

    float NormalisedHitPoint()
    {
        return hitPoints / maxHitPoints;
    }

    void OnDeath()
    {
        Debug.Log("TODO: GAME OVER - YOU DIED");
        GameManager.Instance.GameOver();
    }

    void Heal(float heal)
    {
        hitPoints += heal;
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
        SetHealthSlider();
        Debug.Log("Healed: " + hitPoints.ToString());
    }

}
