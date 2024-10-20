using UnityEngine;
using System.Collections;

public class InvincibilityBonus : MonoBehaviour
{
    public AudioSource audiosource; // מקור הקול לבונוס
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // שמירת ה-Sprite Renderer של הבונוס
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat")) // בדוק אם העכבר נגע בבונוס
        {
            audiosource.Play(); // השמעת צליל בעת לקיחת הבונוס
            Debug.Log("Invincibility bonus collected by Rat");
            // התחל את הקורוטינה שנותנת לעכבר בלתי פגיעות
            StartCoroutine(GiveInvincibility(other.gameObject, 3f));
            // כיבוי ה-Sprite Renderer של הבונוס לאחר שנאסף
            spriteRenderer.enabled = false;
        }
    }

    private IEnumerator GiveInvincibility(GameObject rat, float duration)
    {
        HealthController healthController = rat.GetComponent<HealthController>(); // קבל את הקומפוננטה של בריאות
        if (healthController != null)
        {
            // התחל את מצב הבלתי פגיעות
            yield return healthController.EnterInvulnerabilityState(duration);
        }
    }
}