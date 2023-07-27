using System;
using UnityEngine;

namespace Gameplay {

    public class SimpleStretch : MonoBehaviour
    {
        public GameObject platform;
        public GameObject mechanism;

        private SpriteRenderer ropeSpriteRenderer;
        private float initialDistance;
        private float initialScaleY;

        void Start() {
            ropeSpriteRenderer = GetComponent<SpriteRenderer>();
            initialDistance = Vector3.Distance(platform.transform.position, mechanism.transform.position);
            initialScaleY = transform.localScale.y;
        }

        void Update() {
            if (platform != null && mechanism != null) {
                Vector3 platformPosition = platform.transform.position;
                Vector3 mechanismPosition = mechanism.transform.position;

                // Calculate the current distance between the platform and mechanism
                float currentDistance = Vector3.Distance(platformPosition, mechanismPosition);

                // Calculate the ratio of the current distance to the initial distance
                float distanceRatio = currentDistance / initialDistance;

                // Set the rope's scale based on the distance ratio
                Vector3 newScale = new Vector3(transform.localScale.x, initialScaleY * distanceRatio, transform.localScale.z);
                transform.localScale = newScale;

                // Position the rope's pivot at the mechanism's position
                Vector3 newPosition = (platformPosition + mechanismPosition) / 2f;
                transform.position = newPosition;
            }
        }
    }


}