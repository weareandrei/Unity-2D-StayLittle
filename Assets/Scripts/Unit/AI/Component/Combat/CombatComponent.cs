using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.AI {
    public class CombatComponent : MonoBehaviour {
        
        public BrainBase Brain { get; set; }

        private DamageGivingCollider damageGivingCollider;
        
        // Current Combat state
        public bool isPerformingAttack = false;
        
        private void Start() {
            Transform damageGivingColliderTransform = transform.Find("DamageGivingCollider"); // Replace "ChildName" with the name of your child GameObject

            if (damageGivingColliderTransform != null) {
                damageGivingCollider = damageGivingColliderTransform.GetComponent<DamageGivingCollider>();

                return;
            } 
            
            Debug.LogError("DamageGivingCollider component not found.");
        }

        
        public void PerformAttack(GameObject attackTarget = null) {
            isPerformingAttack = true;
            StartCoroutine(Attack(attackTarget));
        }
        
        private IEnumerator Attack(GameObject attackTarget = null) {
            yield return new WaitForSeconds(1);
            damageGivingCollider.gameObject.SetActive(true);

            MakeDamageInArea();
            InstantiateSlashParticles();

            yield return new WaitForSeconds(0.5f);
            isPerformingAttack = false;
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