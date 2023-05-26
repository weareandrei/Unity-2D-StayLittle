using System.Linq;

namespace Util {
    public static class Random {
        private static System.Random random = new System.Random();
        
        public static string GenerateSeed() {
            int length = random.Next(5, 21); // generate random length between 5 and 20
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}