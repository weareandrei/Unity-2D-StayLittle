using System;
using System.Security.Cryptography;

namespace Util {
    public static class Seed
    {
        private static readonly string StringPI =
            "1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679821480865132823066470938446095505822317253594081284811174502841027019385211055596446229489549303819644288109756659334461284756482337867831652712019091456485669234603486104543266482133936072602491412737245870066063155881748815209209628292540917153643678925903600113305305488204665213841469519415116094330572703657595919530921861173819326117931051185480744623799627495673518857527248912279381830119491298336733624406566430860213949463952247371907021798609437027705392171762931767523846748184676694051320005681271452635608277857713427577896091736371787214684409012249534301465495853710507922796892589235420199561121290219608640344181598136297747713099605187072113499999983729780499510597317328160963185950244594553469083026425223082533446850352619311881710100031378387528865875332083814206171776691473035982534904287554687311595628638823537875937519577818577805321712268066130019278766111959092164201989123";
        // StringPi has a length of 1003
        
        public static int UseSeed(ref string seedState, int maxReturn) {
            string seed = seedState;
            string seedSuffix = seed[^7..];
            
            int first3CharSuffix = GetFirst3CharSeed(seedSuffix);
            int last4CharSuffix = int.Parse(seedSuffix[^4..]);
    
            int new4CharSeed = int.Parse(StringPI.Substring(first3CharSuffix,4));

            seedState = FormNewSeed(
                seedState,
                Format3CharSeed(first3CharSuffix), 
                Format4CharSeed(new4CharSeed)
            );
            
            return GetNumber(maxReturn, last4CharSuffix);
        }
        
        public static string UseSeedToGenerateSeed(ref string seedState) {
            int seedLength = seedState.Length;
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
        
        public static string Reverse( string s )
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        
        private static int GetFirst3CharSeed(string seedSuffix) {
            int first3Char = int.Parse(seedSuffix.Substring(0, 3));
            
            first3Char++;
            first3Char %= 1000;
            
            return first3Char;
        }

        private static string Format4CharSeed(int new4CharSeed) {
            string result4Char = new4CharSeed.ToString();
            while (result4Char.Length < 4) {
                result4Char = '0' + result4Char;
            }
            return result4Char;
        }

        private static string Format3CharSeed(int first3CharSeed) {
            string result3Char = first3CharSeed.ToString();
            while (result3Char.Length < 3) {
                result3Char = '0' + result3Char;
            }
            return result3Char;
        }

        private static string FormNewSeed(string seedState, string result3Char, string result4Char) {
            return seedState.Substring(0, seedState.Length - 7) + result3Char + result4Char;
        }

        private static int GetNumber(int maxReturn, int new4CharSeed) {
            return (int)(new4CharSeed % (maxReturn + 1));
        }

    }
}