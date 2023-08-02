using System;
using UnityEngine;

namespace Actor.AI.Pathfinding {
    public class NavPoint : MonoBehaviour{
        private Vector3 location;

        private void Awake() {
            location = gameObject.transform.position;
        }
    }
}