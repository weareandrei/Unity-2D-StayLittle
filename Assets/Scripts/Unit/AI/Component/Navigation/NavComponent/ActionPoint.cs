using Unit.AI.Navigation;
using UnityEditor;
using UnityEngine;

namespace Unit.AI {
    public class ActionPoint : NavPoint {
        private void OnTriggerEnter2D(Collider2D other) {
            try {
                Pathfinder pathfinder = other.GetComponentInChildren<Pathfinder>();
                if (pathfinder != null) {
                    pathfinder.ActionPointDetected();
                }
            }
            catch { /*ignored*/ }
        }
        
        private void OnDrawGizmos () {
            if (gizmoColor == default) {
                gizmoColor = Random.ColorHSV(0.5f, 0.5f, 0.2f, 0.2f, 1f, 1f, 0.5f, 0.5f); // 20% transparent
            }
            
            Handles.color = gizmoColor;
            Gizmos.DrawWireCube	(
                transform.position, new Vector3(0.3f,0.3f));
        }
    }
}