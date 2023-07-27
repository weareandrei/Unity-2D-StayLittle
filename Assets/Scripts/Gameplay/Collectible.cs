using Content;
using Manager.SubManager;
using UnityEngine;

namespace Interaction {
    public class Collectible : MonoBehaviour {
        [SerializeField] public ContentPayload payload;
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                // other.gameObject.GetComponent<PlayerController>().CollectContent(payload);
                DungeonManager.PlayerEarnedPoints(1, payload.type.ToString());
                gameObject.SetActive(false);
            }
        }
    }
}