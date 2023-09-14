using System.Collections;
using System.Collections.Generic;
using Unit.Base;
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

        
        public void PerformAttack(string param) {
            // switch (param) {
            //     case "LightAttack":
            //         break;
            //     case "HeavyAttack":
            //         break;
            // }
            
            isPerformingAttack = true;
            StartCoroutine(Attack(param));
        }
        
        private IEnumerator Attack(string param) {
            
            switch (param) {
                case "LightAttack":
                    yield return new WaitForSeconds(0.3f);
                    damageGivingCollider.gameObject.SetActive(true);

                    MakeDamageInArea(20);
                    InstantiateSlashParticles();
                    
                    yield return new WaitForSeconds(0.3f);
                    break;
                case "HeavyAttack":
                    yield return new WaitForSeconds(0.6f);
                    damageGivingCollider.gameObject.SetActive(true);

                    MakeDamageInArea(50);
                    InstantiateSlashParticles();
                    
                    yield return new WaitForSeconds(0.4f);
                    break;
            }
            
            isPerformingAttack = false;
            yield return null;
        }

        private void InstantiateSlashParticles() {
            Vector2 colliderPos = damageGivingCollider.transform.position;
            Instantiate(damageGivingCollider.slashVFXPrefab, colliderPos, Quaternion.identity);
        }

        private void MakeDamageInArea(float damage) {
            List<GameObject> targets = damageGivingCollider.GetTargetsAvailable();
            foreach (GameObject targetObject in targets) {
                targetObject.GetComponent<BaseUnit>().RecieveDamage(damage, Brain.gameObject);
                // if (!recievedDamage) { // damage recieved by this? }
            }
        }
    }
}