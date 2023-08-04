using UnityEngine;

namespace Actor.AI.Pathfinding {
    public class ActionPoint : NavPoint {
        private void OnTriggerEnter2D(Collider2D other) {
            try {
                // other.GetComponent<Pathfinding>().PerformAction();
            }
            catch {
                /*ignored*/
            }
        }
    }
}