using System.Collections;
using Unit.Base;
using UnityEngine;

namespace UI.Unit {
    public class EnemyStatsUI : NPCStatsUI {
        
        private void Start() {
            unitMaxHP = thisUnit.stats.MaxHP;
            maxBarWidth = InstantChange_Bar.rect.width;
        }

        protected override void UpdateHP() {
            float unitNewHP = thisUnit.stats.CurrentHP;
            float differenceHP = Mathf.Abs(unitNewHP - unitCurrentHP);
            if (differenceHP > 0) {
                unitCurrentHP = unitNewHP;
                hpBarWidthAim = unitCurrentHP *  maxBarWidth/ unitMaxHP;
                ChangeHealth(differenceHP);
            }
        }

        protected override void UpdateLevel() {
            
        }
        
        private void ChangeHealth(float amount) {
            // Value = Mathf.Clamp(Value + amount, 0, 100f);

            if (_adjustBarWidthCoroutine != null) {
                StopCoroutine(_adjustBarWidthCoroutine);
            }

            _adjustBarWidthCoroutine = StartCoroutine(AdjustBarWidth(amount));
        } 

        private IEnumerator AdjustBarWidth(float amount) {
            RectTransform instantChangeBar = amount >= 0 ? InstantChange_Bar : SlowChange_Bar;
            RectTransform slowChangeBar = amount >= 0 ? SlowChange_Bar : InstantChange_Bar;
            
            SetRectWidth(instantChangeBar, hpBarWidthAim);
            
            while (Mathf.Abs(instantChangeBar.rect.width - slowChangeBar.rect.width) > 1f) {
                SetRectWidth(slowChangeBar, Mathf.Lerp(
                        slowChangeBar.rect.width, b: hpBarWidthAim, 
                        Time.deltaTime * HealthChangeAnimationSpeed));
                yield return null;
            }
            
            SetRectWidth(slowChangeBar, hpBarWidthAim);
        }

        public void SetRectWidth(RectTransform t, float width) {
            t.sizeDelta = new Vector2(width, y: t.rect.height);
        }

    }
}