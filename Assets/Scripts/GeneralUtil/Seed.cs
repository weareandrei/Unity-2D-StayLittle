namespace GeneralUtil {
    public static class Seed {
        public static int UseSeed(ref string seedState, int choiceCount) {
            int seedNumber = (int)seedState[0];
            seedState = seedState.Substring(1, seedState.Length - 1) + seedNumber;

            while (choiceCount <= seedNumber) {
                seedNumber = seedNumber - choiceCount;
                if (seedNumber < 0) {
                    seedNumber = 0;
                }
            }
            
            return seedNumber;
        }

        public static void ResetSeed(ref string seedState, string seedOriginalState) {
            seedState = seedOriginalState;
        }

    }
}