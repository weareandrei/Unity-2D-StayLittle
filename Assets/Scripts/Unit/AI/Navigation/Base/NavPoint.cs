using System;
using UnityEngine;

namespace Unit.AI {
    public class NavPoint : MonoBehaviour{
        public Vector2 location;

        private void Awake() {
            location = gameObject.transform.position;
        }
    }
}