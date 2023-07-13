using UnityEngine;

namespace Graphics {
    public class DynamicParallax : MonoBehaviour {
        public Transform player; // Reference to the player or camera transform
        public float horizontalParallaxStrength = 0.5f; // Adjust this value for the desired horizontal parallax effect
        public float verticalParallaxStrength = 0.2f; // Adjust this value for the desired vertical parallax effect

        private Vector3 previousPlayerPosition;

        private void Start() {
            
            if (player == null) {
                player = GameObject.FindGameObjectsWithTag("Player")[0]?.transform;
                if (player == null) {
                    player = Camera.main.transform; // Assign the main camera as the default player
                }
            }

            previousPlayerPosition = player.position;
        }

        private void Update() {
            // Calculate the player's movement delta
            Vector3 playerMovement = player.position - previousPlayerPosition;

            // Iterate through each child layer
            for (int i = 0; i < transform.childCount; i++) {
                Transform layer = transform.GetChild(i);

                // Calculate the layer's parallax offset based on the player's movement
                float horizontalParallaxOffset = playerMovement.x * horizontalParallaxStrength * (i + 1);
                float verticalParallaxOffset = playerMovement.y * verticalParallaxStrength * (i + 1);

                // Apply the parallax offset to the layer's position
                Vector3 layerPosition = layer.position;
                layerPosition.x += horizontalParallaxOffset;
                layerPosition.y += verticalParallaxOffset;
                layer.position = layerPosition;
            }

            // Update the previous player position for the next frame
            previousPlayerPosition = player.position;
        }
    }
}