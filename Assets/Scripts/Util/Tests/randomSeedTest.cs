using System;
using NUnit.Framework;
using UnityEngine;
using Util;

public class RandomSeedTest
{
    public static String lcp(String s, String t) {
        int n = Math.Min(s.Length,t.Length);  
        for(int i = 0; i < n; i++){  
            if(s[i] != t[i]){  
                return s.Substring(0,i);  
            }  
        }  
        return s.Substring(0,n);  
    }  
    
    [Test]
    public void RandomSeedTestSimplePasses() {
        string seed = "010079456275";
        string seedState = seed;
        for (int k = 0; k < 10; k++) // for UseSeed() test
        {
            seedState = (Convert.ToInt32(seed[^8..]) + k * 100000).ToString();
            Debug.Log("Seed: " + seedState);
            string str = "";
            for (int i = 0; i < 100; i++) {
                int c = Seed.UseSeed(ref seedState, 8);
                str += c.ToString();
            }
            Debug.Log("Str: " + str);
            // int numericNewSeed = Convert.ToInt32(seed) + 1;
            // seed = numericNewSeed.ToString();
            String lrs = "";  
            int n = str.Length;  
            for(int i = 0; i < n; i++){  
                for(int j = i+1; j < n-i; j++){  
                    //Checks for the largest common factors in every substring  
                    String x = lcp(str.Substring(i),str.Substring(j));  
                    //If the current prefix is greater than previous one   
                    //then it takes the current one as longest repeating sequence  
                    if(x.Length > lrs.Length) lrs = x;    
                }  
            }
        
            Debug.Log("Longest repeating sequence: " + lrs);
        }
        for (int k = 0; k < 100; k++) // for UseSeedToGenerateSeed() test
        {
            Debug.Log(Seed.UseSeedToGenerateSeed(ref seedState));
        }
    }

}