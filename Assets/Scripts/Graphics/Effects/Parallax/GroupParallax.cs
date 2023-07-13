using UnityEngine;

namespace Graphics {
    public class GroupParallax : MonoBehaviour {
        public float distanceBetween = 1f;
        public float scaleMultiplier = 0.1f; // Adjust this value for desired scaling

        private void Update() {
            // Get all child GameObjects
            int childCount = transform.childCount;
            Transform[] children = new Transform[childCount];
            for (int i = 0; i < childCount; i++) {
                children[i] = transform.GetChild(i);
            }

            // Start at Z coordinate 0 for the first child
            float currentZ = 0f;

            // Iterate through child GameObjects starting from the second child
            for (int i = 1; i < childCount; i++) {
                Transform child = children[i];

                // Calculate modifications
                float distance = i * distanceBetween;
                float scale = 1f + distance * scaleMultiplier;

                // Apply modifications to the child GameObject
                child.localPosition = new Vector3(0f, 0f, currentZ);
                child.localScale = new Vector3(scale, scale, 1f);

                // Update current Z coordinate for the next child
                currentZ += distance;
            }
        }
    }
}