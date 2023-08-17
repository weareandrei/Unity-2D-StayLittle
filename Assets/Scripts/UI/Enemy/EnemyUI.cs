using System.Collections;
using UI.Base;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Enemy {
    public class EnemyUI : UIElement {

        [SerializeField] private RectTransform SlowChange_Bar;
        [SerializeField] private RectTransform InstantChange_Bar;

        [SerializeField] private float HealthChangeAnimationSpeed = 10f;

        [SerializeField] private float hpBarWidthAim;
        [SerializeField] private float maxBarWidth;

        private Coroutine _adjustBarWidthCoroutine ;
        private Unit.Base.Unit thisEnemyUnit;
        private float unitMaxHP;
        private float unitCurrentHP;

        private void Start() {
            thisEnemyUnit = GetComponentInParent<Unit.Base.Unit>();
            unitMaxHP = thisEnemyUnit.GetMaxHP();
            maxBarWidth = InstantChange_Bar.rect.width;
        }
        
        private void FixedUpdate() {
            float unitNewHP = thisEnemyUnit.GetHP();
            float differenceHP = Mathf.Abs(unitNewHP - unitCurrentHP);
            if (differenceHP > 0) {
                unitCurrentHP = unitNewHP;
                hpBarWidthAim = unitCurrentHP *  maxBarWidth/ unitMaxHP;
                ChangeHealth(differenceHP);
            }
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