using UnityEngine;

public class AddHealthBonus : MonoBehaviour
{
    public AudioSource audiosource;
    [SerializeField] private float healthToAdd = 500f; // Amount of health to add

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat"))
        {
            audiosource.Play();
            Debug.Log("Health bonus triggered by Rat");
            AddHealthEffect(other.gameObject);
            gameObject.SetActive(false); // Deactivate bonus after it's collected
        }
    }

    private void AddHealthEffect(GameObject rat)
    {
        // Assuming the rat has a HealthController script to manage health
        HealthController healthController = rat.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.AddHealth(healthToAdd); // Call a method to add health
        }
    }
}
