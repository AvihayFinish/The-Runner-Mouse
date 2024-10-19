using UnityEngine;
using System.Collections;

public class GetSmaller : MonoBehaviour
{
    public AudioSource audiosource;
    private Vector3 originalScale;
    public float effectDuration = 5f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
      
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat") )
        {
            audiosource.Play();
            Debug.Log("GetSmaller triggered by Rat");

            if (originalScale == Vector3.zero)
            {
                originalScale = other.transform.localScale;
                Debug.Log("Original scale saved: " + originalScale);
            }

            GetSmallerEffect(other.gameObject);
   
            spriteRenderer.enabled = false;
        }
    }

    private void GetSmallerEffect(GameObject rat)
    {
       
        Vector3 newScale = originalScale / 2f;
        rat.transform.localScale = newScale;
        Debug.Log("Rat scaled down to: " + newScale);

        StartCoroutine(ResetSizeAfterDelay(rat, effectDuration));
    }

    private IEnumerator ResetSizeAfterDelay(GameObject rat, float delay)
    {
        Debug.Log("Coroutine started, waiting for " + delay + " seconds");
  
        yield return new WaitForSeconds(delay);

        Debug.Log("Time passed, resetting rat size to original");
   
        rat.transform.localScale = originalScale;

        Debug.Log("Rat size reset to: " + originalScale);

         gameObject.SetActive(false);
    }
}
