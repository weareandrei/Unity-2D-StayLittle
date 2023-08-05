using UnityEngine;

namespace Unit.AI {
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