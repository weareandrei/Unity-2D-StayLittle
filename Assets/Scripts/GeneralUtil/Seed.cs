using System;

namespace GeneralUtil {
    public static class Seed {
        // public static int UseSeed(ref string seedState, int choiceCount) {
        //     string choiceCountString = choiceCount.ToString();
        //     string result = "";
        //
        //     foreach (var digit in choiceCountString) {
        //         int currentNumber = int.Parse(digit.ToString());
        //         string firstString = GetFirstCharFromSeed(ref seedState, currentNumber).ToString();
        //         result += firstString;
        //     }
        //
        //     return int.Parse(result);
        // }
        
        public static int UseSeed(ref string seedState, int maxReturn)
        {
            const long a = 1103515245;   // Multiplier
            const int c = 12345;          // Increment
            const long m = int.MaxValue;  // Modulus

            long seed = long.Parse(seedState);
            seed = (a * seed + c) % m;
            seedState = seed.ToString();

            int number = (int)(seed % (maxReturn + 1));

            return number;
        }
        
        public static string UseSeedToGenerateSeed(ref string seedState) {
            int seedLength = 6;
            string newSeed = "";
            
            for (int i = 0; i < seedLength; i++) {
                int firstDigit = UseSeed(ref seedState, 9);
                string newChar = firstDigit.ToString();
            
                newSeed += newChar;
            }

            return newSeed;
        }

        public static void ResetSeed(ref string seedState, string seedOriginalState) {
            seedState = seedOriginalState;
        }

    }
}