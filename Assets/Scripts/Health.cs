using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Master")]
    public bool auto = false;// have control or not
    public float corpseLifeTime = 2f;

    // config
    [Header("Configuration")]
    public bool output = true;
    public float InitialHealth = 100f;

    // audio
    [Header("Audio")]
    public AudioClip DeadSound;

    // references
    [Header("References")]
    public bool autoReference = true;

    public AudioSource Audio;
    public Animator BodyAnimator;
    public Image OutputImage;

    // deubug
    [Header("Debug")]
    public float CurrentHealth;
    public bool isAlive = true;

    private void Awake()
    {
        // setting references
        if (autoReference == true)
        {
            BodyAnimator = GetComponent<Animator>();
            Audio = GetComponent<AudioSource>();
        }

        // check control
        if (auto == true)
            output = false;
    }

    private void Start()
    {
        // setting defaults
        CurrentHealth = InitialHealth;
    }

    private void Update()
    {
        CheckHealth(); // Checks for player conciousness

        if (auto == false)
            DisplayHealth();

        if (isAlive == false)
            GoToFinalDestination();
    }

    // display health
    private void DisplayHealth()
    {
        if (output == true)
            OutputImage.fillAmount = CurrentHealth / InitialHealth;
    }

    // checks player health
    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
            isAlive = false;

        if (isAlive == false)
        {
            BodyAnimator.SetTrigger("dead"); // animation
            PlayDeathSound(); // sound
        }
    }

    // handles damages player.. or enemy
    public void AddDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
            isAlive = false;
    }

    // plays death sound
    private void PlayDeathSound()
    {
        Audio.clip = DeadSound;
        Audio.Play();
    }

    // stops player
    private void GoToFinalDestination()
    {
        // manumally disabling all components
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Health>().enabled = false;

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;

        if (auto)
        {

            Invoke("SelfDestroy", corpseLifeTime);
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
