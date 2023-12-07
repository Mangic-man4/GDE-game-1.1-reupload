using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    int coins = 0;

    [SerializeField] Text coinsText;

    [SerializeField] AudioSource collectionSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;

            // Check if coinsText is not null before trying to access its properties
            if (coinsText != null)
            {
                coinsText.text = "Coins: " + coins;
            }

            if (collectionSound != null)
            {
                collectionSound.Play();
            }
        }
    }

}
