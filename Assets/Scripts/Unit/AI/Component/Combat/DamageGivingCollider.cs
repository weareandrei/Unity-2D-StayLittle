using System.Collections.Generic;
using UnityEngine;

namespace Unit.AI {
    public class DamageGivingCollider : MonoBehaviour {
        private List<GameObject> targetsAvailable;

        [SerializeField] public GameObject slashVFXPrefab;

        private void Start() {
            targetsAvailable = new List<GameObject>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!targetsAvailable.Contains(other.gameObject) && other.gameObject.GetComponent<Base.Unit>()) {
                targetsAvailable.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (targetsAvailable.Contains(other.gameObject)) {
                targetsAvailable.Add(other.gameObject);
            }
        }

        public List<GameObject> GetTargetsAvailable() {
            return this.targetsAvailable;
        }
        
    }
}