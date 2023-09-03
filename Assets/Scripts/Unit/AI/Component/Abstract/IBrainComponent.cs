namespace Unit.AI {
    public interface IBrainComponent {
        BrainBase Brain { set; get; }
        void SendSignalToBrain(string param);
    }
}