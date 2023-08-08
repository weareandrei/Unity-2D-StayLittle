using UnityEngine;

namespace Unit.AI {
    public class ActionPoint : NavPoint {
        private void OnTriggerEnter2D(Collider2D other) {
            try {
                Pathfinder pathfinder = other.gameObject.GetComponent<Pathfinder>();
                if (pathfinder != null)
                {
                    pathfinder.ActionPointDetected();
                }
            }
            catch { /*ignored*/ }
        }
    }
}