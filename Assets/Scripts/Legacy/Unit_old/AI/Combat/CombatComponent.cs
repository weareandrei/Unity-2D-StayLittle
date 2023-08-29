using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy.Unit_old.AI.Combat {
    public class CombatComponent : MonoBehaviour {
        private Brain.Base.Brain brain;
        [SerializeField] private DamageGivingCollider damageGivingCollider;
        
        private void Start() {
            brain = gameObject.GetComponent<Brain.Base.Brain>();
        }

        public void PerformAttack(GameObject attackTarget = null) {
            brain.isPerformingAttack = true;
            StartCoroutine(Attack(attackTarget));
        }
        
        private IEnumerator Attack(GameObject attackTarget = null) {
            yield return new WaitForSeconds(1);
            damageGivingCollider.gameObject.SetActive(true);

            MakeDamageInArea();
            InstantiateSlashParticles();

            yield return new WaitForSeconds(0.5f);
            brain.isPerformingAttack = false;
            yield return null;
        }

        private void InstantiateSlashParticles() {
            Vector2 colliderPos = damageGivingCollider.transform.position;
            Instantiate(damageGivingCollider.slashVFXPrefab, colliderPos, Quaternion.identity);
        }

        private void MakeDamageInArea() {
            List<GameObject> targets = damageGivingCollider.GetTargetsAvailable();
            foreach (GameObject targetObject in targets) {
                targetObject.GetComponent<Base.Unit>().RecieveDamage(20);
                // if (!recievedDamage) { // damage recieved by this? }
            }
        }
    }
}