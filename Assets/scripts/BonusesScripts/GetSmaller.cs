using UnityEngine;

public class GetSmaller : MonoBehaviour
{

    public AudioSource audiosource;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat"))
        {
            audiosource.Play();
            Debug.Log("GetBigger triggered by Rat");
            GetSmallerEffect(other.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void GetSmallerEffect(GameObject rat)
    {
        Vector3 newScale = new Vector3(rat.transform.localScale.x / 2f, rat.transform.localScale.y / 2f, rat.transform.localScale.z / 2f);

        rat.transform.localScale = newScale;    
    }
}
