using UI.Base;
using UnitBase = Unit.Base;

namespace UI.Unit {
    public class BaseStatsUI : UIElement {
        protected UnitBase.Unit thisUnit;

        protected float displayedHP;
        protected float displayedXP;
        protected int displayedLevel;

        private void Start() {
            thisUnit = GetComponent<UnitBase.Unit>();
        }
    }
}