using System;
using UnityEngine;

namespace Unit.AI {
    public class NavPoint : MonoBehaviour {
        public Vector2 location;
        protected Color gizmoColor = default; // Store the color here

        private void Awake() {
            location = gameObject.transform.position;
        }
    }
}