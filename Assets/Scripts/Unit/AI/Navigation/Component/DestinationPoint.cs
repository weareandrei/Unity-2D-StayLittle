using UnityEngine;

namespace Unit.AI {
    
    public class DestinationPoint : NavPoint {

        private void OnTriggerEnter2D(Collider2D other) {
            try {
                // other.GetComponent<Pathfinding>().PerformDestinationCalc();
            } catch { /*ignored*/ }
        }
        
    }
    
}