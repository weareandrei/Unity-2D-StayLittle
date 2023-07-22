// using System;
// using UnityEngine;
//
// namespace Interaction {
//
//     using UnityEngine;
//
//     public class SimpleStretch : MonoBehaviour
//     {
//         public GameObject anchorObject;
//         private SpriteRenderer spriteRenderer;
//         
//         [SerializeField] private Vector3 initialScale;
//         [SerializeField] private Vector3 initialPosition;
//         [SerializeField] public float initialDistance;
//
//         private void Start() {
//             spriteRenderer = GetComponent<SpriteRenderer>();
//             initialScale = transform.localScale;
//             initialPosition = transform.position;
//             initialDistance = Vector3.Distance(transform.position, anchorObject.transform.position);
//         }
//
//         private void Update() {
//             float distance = Vector3.Distance(transform.position, anchorObject.transform.position);
//             float distanceChange = distance - initialDistance;
//             
//             Vector3 newCenterPosition = new Vector3(initialPosition.y, initialPosition.y - distanceChange / 2, initialPosition.z);
//             transform.position = newCenterPosition;
//
//             Vector3 newScale = new Vector3(initialScale.x, initialScale.y + distanceChange, initialScale.z);
//             transform.localScale = newScale;
//
//             // float stretchFactor = Mathf.Clamp(distance / maxDistance, 0f, 1f) * maxStretchFactor;
//             //
//             // // Apply the stretching factor to the Y-axis of the local scale
//             // Vector3 newScale = initialScale;
//             // newScale.y += stretchFactor;
//             // transform.localScale = newScale;
//             //
//             // Update the position of the "StretchingObject" to stay attached to the "AnchorObject"
//             // transform.position = anchorObject.transform.position;
//         }
//
//     }
//
// }