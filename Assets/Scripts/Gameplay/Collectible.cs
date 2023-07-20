using Content;
using UnityEngine;

namespace Interaction {
    public class Collectible : MonoBehaviour {
        [SerializeField] private ContentPayload payload;
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                // other.gameObject.GetComponent<PlayerController>().CollectContent(payload);
                gameObject.SetActive(false);
            }
        }
    }
}