using UnityEngine;

public class itemCollector : MonoBehaviour
{
    private int gold = 0;

    private int silver = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gold"))
        {
            Destroy(collision.gameObject);
            gold++;
            Debug.Log("Gold: " +gold);
        }
        if (collision.gameObject.CompareTag("Silver"))
        {
            Destroy(collision.gameObject);
            silver++;
            Debug.Log("Silver: " +silver);
        }
    }
}
