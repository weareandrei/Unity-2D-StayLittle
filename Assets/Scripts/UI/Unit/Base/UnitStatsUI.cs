using System.Collections;
using UnityEngine;
using UnitBase = Unit.Base;

namespace UI.Unit {
    public abstract class UnitStatsUI : BaseStatsUI {

        [SerializeField] protected RectTransform SlowChange_Bar;
        [SerializeField] protected RectTransform InstantChange_Bar;

        [SerializeField] protected float HealthChangeAnimationSpeed = 10f;

        [SerializeField] protected float hpBarWidthAim;
        [SerializeField] protected float maxBarWidth;

        protected Coroutine _adjustBarWidthCoroutine;
        protected float unitMaxHP;
        protected float unitCurrentHP;

        protected UnitBase.Unit thisUnit;
        
        private void Awake() {
            thisUnit = GetComponentInParent<UnitBase.Unit>();
        }
        
        private void Start() {
            unitMaxHP = thisUnit.stats.MaxHP;
            maxBarWidth = InstantChange_Bar.rect.width;
        }
        
        private void FixedUpdate() {
            UpdateHP();
            UpdateLevel();
        }

        protected abstract void UpdateLevel();

        protected abstract void UpdateHP();

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