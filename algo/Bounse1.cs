using System;
using System.Numerics;
using System.Text;

public class Bounse1
{
    public static StringBuilder turnToBigInt(string sentence)
    {
        sentence = sentence.ToLower();
        StringBuilder numberString = new StringBuilder();
        BigInteger bigInteger = new BigInteger();
        foreach (char c in sentence)
        {
            if (char.IsLetter(c))
            {
                int num = c - 'a'; 
                numberString.Append(num.ToString("D2")); //to be able to decode again(each char has a number from 0 to 25)
            }
        }

        return numberString;
    }
}