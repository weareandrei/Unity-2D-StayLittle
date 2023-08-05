namespace Unit.Controller {
    public class UnitController : BaseController {
        protected override void GetMovementInput() {
            // UnitController will take commands here
            throw new System.NotImplementedException();
        }

        protected override void GetMovementSpeed() {
            // Depending on other commands, or some factors Unit will speed up or slow down...
            throw new System.NotImplementedException();
        }

        protected override void CheckInputs() {
            return;
        }
    }
}