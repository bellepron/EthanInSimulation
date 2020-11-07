using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    public float health = 100f;
    private GameManager gameManager;
    private Slider healthBarSlider;
    public TextMeshPro givenDamageText;
    private float maxHealth = 100f;


    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        healthBarSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        //givenDamageText = GameObject.Find("Given Damage Text").GetComponent<TextMeshPro>();
    }

    public void ApplyDamage(float damage)
    {
        if (health > damage)
        {
            health -= damage;
            if (gameObject.CompareTag("Player"))
            {
                gameManager.UpdateHealth(damage);
            }
            if (gameObject.CompareTag("Enemy"))
            {
                float dmg = damage - damage % 1;
                givenDamageText.text = dmg.ToString();
                Instantiate(givenDamageText, gameObject.transform.position + new Vector3(0, 1.8f, 0), Quaternion.identity);
            }
        }
        else
        {
            damage = health;
            health = 0;
            if (gameObject.CompareTag("Player"))
            {
                gameManager.UpdateHealth(damage);
            }
        }
        if (health == 0)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                float dmg = damage - damage % 1;
                givenDamageText.text = dmg.ToString();
                Instantiate(givenDamageText, gameObject.transform.position + new Vector3(0, 1.8f, 0), Quaternion.identity);

                gameManager.UpdateScore(1);
                StartCoroutine(DestroyTime());
            }
        }
    }

    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = health;
        }

    }

    // IEnumerator ZombieDestroyTime()
    // {
    //     yield return new WaitForSeconds(2);
    // }
    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
