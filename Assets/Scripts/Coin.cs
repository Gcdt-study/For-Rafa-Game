using UnityEngine;

public class Coin : MonoBehaviour
{
    public float speedBoost = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.speed += speedBoost;
            }

            CoinManager.instance.AddCoin();
            Destroy(gameObject);
        }
        Debug.Log("CoinManager.instance = " + CoinManager.instance);

    }


}
