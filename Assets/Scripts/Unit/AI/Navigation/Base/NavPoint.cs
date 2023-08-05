using System;
using UnityEngine;

namespace Unit.AI {
    public class NavPoint : MonoBehaviour{
        private Vector3 location;

        private void Awake() {
            location = gameObject.transform.position;
        }
    }
}