using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using algo;


public class BigInteger
{
    private List<byte> digits;
    public BigInteger()
    {
        digits = new List<byte> { 0 };
    }
    public BigInteger(string number)
    {
        digits = new List<byte>();
        for (int i = number.Length - 1; i >= 0; i--)
            digits.Add((byte)(number[i] - '0'));
    }

    // Helper methods throughout the implementation
    private void RemoveLeadingZeros()
    {
        while (digits.Count > 1 && digits[^1] == 0)
            digits.RemoveAt(digits.Count - 1);
    }
    public bool LessThan(BigInteger other)
    {
        if (this.digits.Count != other.digits.Count)
            return this.digits.Count < other.digits.Count;
        for (int i = this.digits.Count - 1; i >= 0; i--)
        {
            if (this.digits[i] != other.digits[i])
                return this.digits[i] < other.digits[i];
        }
        return false;
    }
    public bool GreaterThan(BigInteger other)
    {
        if (this.digits.Count != other.digits.Count)
            return this.digits.Count > other.digits.Count;
        for (int i = this.digits.Count - 1; i >= 0; i--)
        {
            if (this.digits[i] != other.digits[i])
                return this.digits[i] > other.digits[i];
        }
        return false;
    }
    public BigInteger powermod(BigInteger basee, BigInteger exp, BigInteger mod)
    {
        BigInteger result = new BigInteger("1");
        basee = BinaryDivMod(basee, mod).Remainder;

        if (exp.Equals(new BigInteger("0")))
        {
            return result;
        }

        while (exp.GreaterThan(new BigInteger("0")))
        {
            BigInteger resultmod = BinaryDivMod(exp, new BigInteger("2")).Remainder;
            string s = resultmod.ToString();
            string value1 = "1";               /// this string for get value from mod %2 and compare with 1 (odd);
            if (s.Equals(value1))
            {
                //Console.WriteLine(exp.ToString());
                result = result.Mul(basee);
                result = BinaryDivMod(result, mod).Remainder;
                //Console.WriteLine(result.ToString());
            }
            basee = basee.Mul(basee);
            basee = BinaryDivMod(basee, mod).Remainder;
            exp = BinaryDivMod(exp, new BigInteger("2")).Quotient;
            //Console.WriteLine(result.ToString());
        }

        return result;
    }
    public BigInteger Encryption(BigInteger m, BigInteger e, BigInteger n)
    {
        BigInteger encr = new BigInteger();
        encr = powermod(m, e, n);
        return encr;
    }
    public BigInteger Decryption(BigInteger em, BigInteger d, BigInteger n)
    {

        BigInteger dencr = powermod(em, d, n);

        return dencr;
    }
    public bool Equals(BigInteger other)
    {
        if (this.digits.Count != other.digits.Count)
            return false;
        for (int i = 0; i < this.digits.Count; i++)
            if (this.digits[i] != other.digits[i])
                return false;
        return true;
    }
    private BigInteger HalveValue()
    {
        // Handle zero case
        if (this.Equals(new BigInteger("0")))
            return new BigInteger("0");

        BigInteger result = new BigInteger();
        result.digits = new List<byte>(new byte[this.digits.Count]);
        int carry = 0;

        for (int i = this.digits.Count - 1; i >= 0; i--)
        {
            int current = this.digits[i] + carry * 10;
            result.digits[i] = (byte)(current / 2);
            carry = current % 2;
        }

        result.RemoveLeadingZeros();
        return result;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = digits.Count - 1; i >= 0; i--)
            sb.Append(digits[i]);
        return sb.ToString();
    }
    public bool IsEven()
    {
        return digits[0] % 2 == 0;
    }

    public BigInteger Add(BigInteger other)
    {
        BigInteger result = new BigInteger();
        result.digits.Clear();
        int carry = 0;
        int n = Math.Max(this.digits.Count, other.digits.Count);
        for (int i = 0; i < n || carry > 0; i++)
        {
            int sum = carry;
            if (i < this.digits.Count) sum += this.digits[i];
            if (i < other.digits.Count) sum += other.digits[i];
            result.digits.Add((byte)(sum % 10));
            carry = sum / 10;
        }
        return result;
    }
    public BigInteger Sub(BigInteger other)
    {
        BigInteger result = new BigInteger();
        result.digits.Clear();
        int borrow = 0;
        for (int i = 0; i < this.digits.Count; i++)
        {
            int diff = this.digits[i] - borrow;
            if (i < other.digits.Count) diff -= other.digits[i];
            if (diff < 0)
            {
                diff += 10;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }
            result.digits.Add((byte)(diff));
        }
        result.RemoveLeadingZeros();
        return result;
    }
    // public BigInteger Mul(BigInteger other)
    // {
    //     BigInteger result = new BigInteger();
    //     result.digits = new List<int>(new int[this.digits.Count + other.digits.Count]);
    //     for (int i = 0; i < this.digits.Count; i++)
    //     {
    //         for (int j = 0; j < other.digits.Count; j++)
    //         {
    //             result.digits[i + j] += this.digits[i] * other.digits[j];
    //             if (result.digits[i + j] >= 10)
    //             {
    //                 result.digits[i + j + 1] += result.digits[i + j] / 10;
    //                 result.digits[i + j] %= 10;
    //             }
    //         }
    //     }
    //     result.RemoveLeadingZeros();
    //     return result;
    // }

    // Binary long division and modulus
    public static (BigInteger Quotient, BigInteger Remainder) BinaryDivMod(BigInteger dividend, BigInteger divisor)
    {
        if (divisor.Equals(new BigInteger("0")))
            throw new DivideByZeroException("Cannot divide by zero.");

        if (dividend.LessThan(divisor))
            return (new BigInteger("0"), dividend);

        BigInteger quotient = new BigInteger("0"); //123 , 10
        BigInteger remainder = dividend;

        // the largest power of 2 multiplied by divisor <= dividend (long division rule)
        BigInteger powerOfTwo = new BigInteger("1");
        BigInteger tempDivisor = divisor;

        while (!remainder.LessThan(tempDivisor))
        {
            tempDivisor = tempDivisor.Add(tempDivisor);
            powerOfTwo = powerOfTwo.Add(powerOfTwo);
        }

        // half the tempDivisor and powerOfTwo to start the binary long division
        tempDivisor = tempDivisor.HalveValue();
        powerOfTwo = powerOfTwo.HalveValue();

        // binary loong division
        while (!powerOfTwo.Equals(new BigInteger("0")))
        {
            if (!remainder.LessThan(tempDivisor))
            {
                remainder = remainder.Sub(tempDivisor);
                quotient = quotient.Add(powerOfTwo);
            }


            tempDivisor = tempDivisor.HalveValue();
            powerOfTwo = powerOfTwo.HalveValue();
        }

        return (quotient, remainder);
    }
    private static BigInteger ShiftLeft(BigInteger num, int n)
    {
        BigInteger result = new BigInteger();
        result.digits = new List<byte>(new byte[n]);
        result.digits.AddRange(num.digits);
        return result;
    }
    public BigInteger Mul(BigInteger other)
    {
        BigInteger result = new BigInteger();


        result.digits = new List<byte>(new byte[this.digits.Count + other.digits.Count]);

        if ((this.digits.Count == 1 && this.digits[0] == 0) || (other.digits.Count == 1 && other.digits[0] == 0))
        {
            return new BigInteger("0");
        }

        int n = Math.Max(this.digits.Count, other.digits.Count);
        int m = n / 2;

        BigInteger xright = new BigInteger();
        BigInteger xleft = new BigInteger();
        BigInteger yright = new BigInteger();
        BigInteger yleft = new BigInteger();
        xright.digits.Clear();
        xleft.digits.Clear();
        yright.digits.Clear();
        yleft.digits.Clear();

        if (this.digits.Count + other.digits.Count < 8)
        {
            for (int i = 0; i < this.digits.Count; i++)
            {
                int carry = 0;
                for (int j = 0; j < other.digits.Count; j++)
                {
                    int temp = result.digits[i + j] + this.digits[i] * other.digits[j] + carry;
                    result.digits[i + j] = (byte)(temp % 10);
                    carry = temp / 10;
                }
                int k = i + other.digits.Count;
                while (carry > 0)
                {
                    int temp = result.digits[k] + carry;
                    result.digits[k] = (byte)(temp % 10);
                    carry = temp / 10;
                    k++;
                }
            }
            result.RemoveLeadingZeros();
            return result;
        }
        else
        {
            for (int i = 0; i < m && i < this.digits.Count; i++)
            {
                xright.digits.Add(this.digits[i]);
            }
            for (int i = m; i < this.digits.Count; i++)
            {
                xleft.digits.Add(this.digits[i]);
            }

            for (int i = 0; i < m && i < other.digits.Count; i++)
            {
                yright.digits.Add(other.digits[i]);
            }
            for (int i = m; i < other.digits.Count; i++)
            {
                yleft.digits.Add(other.digits[i]);
            }
        }

        BigInteger numR = xright.Mul(yright);
        BigInteger numL = xleft.Mul(yleft);
        BigInteger numM = (xleft.Add(xright)).Mul(yleft.Add(yright)).Sub(numR).Sub(numL);
        BigInteger result1 = ShiftLeft(numL, 2 * m);
        BigInteger result2 = ShiftLeft(numM, m);
        result = result1.Add(result2).Add(numR);

        result.RemoveLeadingZeros();
        return result;
    }

}
class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("            Main Menu");
            Console.WriteLine("========================================");
            Console.WriteLine("1 - Encryption & Decryption");
            Console.WriteLine("2 - Arithmetic Operations");
            Console.WriteLine("3 - Exit");
            Console.WriteLine("========================================");
            int choice;
            Console.Write("Enter your choice (1-3): ");
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("Invalid choice! Please enter a number between 1 and 3.");
                Console.Write("Enter your choice (1-3): ");
            }
            switch (choice)
            {
                case 1:
                    Console.WriteLine("\nEncryption & Decryption selected...");
                    FileHandling.Encyption_Decryption_file();
                    break;

                case 2:
                    Console.WriteLine("\nArithmetic Operations selected...");
                    FileHandling.Arithmetic_file();
                    break;

                case 3:
                    Console.WriteLine("\nExiting program. Goodbye!");
                    return;

                default:
                    break;
            }
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

    }
}