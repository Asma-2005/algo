using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;


public class BigInteger
{   
    private List<int> digits;
    public BigInteger()
    {
        digits = new List<int> { 0 };
    }
    public BigInteger(string number)
    {
        digits = new List<int>();
        for (int i = number.Length - 1; i >= 0; i--)
            digits.Add(number[i] - '0');
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
        result.digits = new List<int>(new int[this.digits.Count]);
        int carry = 0;

        for (int i = this.digits.Count - 1; i >= 0; i--)
        {
            int current = this.digits[i] + carry * 10;
            result.digits[i] = current / 2;
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
            result.digits.Add(sum % 10);
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
            result.digits.Add(diff);
        }
        result.RemoveLeadingZeros();
        return result;
    }
    public BigInteger Mul(BigInteger other)
    {
        BigInteger result = new BigInteger();
        result.digits = new List<int>(new int[this.digits.Count + other.digits.Count]);
        for (int i = 0; i < this.digits.Count; i++)
        {
            for (int j = 0; j < other.digits.Count; j++)
            {
                result.digits[i + j] += this.digits[i] * other.digits[j];
                if (result.digits[i + j] >= 10)
                {
                    result.digits[i + j + 1] += result.digits[i + j] / 10;
                    result.digits[i + j] %= 10;
                }
            }
        }
        result.RemoveLeadingZeros();
        return result;
    }

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
    result.digits = new List<int>(new int[n]); 
    result.digits.AddRange(num.digits);
    return result;
}
    public BigInteger Mul(BigInteger other)
    {
        BigInteger result = new BigInteger();
        
        result.digits =new List<int>(new int[this.digits.Count+other.digits.Count]);
        

        if ((this.digits.Count == 1 && this.digits[0] == 0) || (other.digits.Count == 1 && other.digits[0] == 0))
        {
            result = new BigInteger("0");
            return result;
        }
        int n = Math.Max(this.digits.Count, other.digits.Count);
        int m = n / 2;

        BigInteger xright = new BigInteger();
        BigInteger xleft = new BigInteger();
       
        BigInteger yright = new BigInteger();
        BigInteger yleft = new BigInteger();
   

        if (this.digits.Count + other.digits.Count < 8)
        {
            for (int i = 0; i < this.digits.Count; i++)
            {
                for (int j = 0; j < other.digits.Count; j++)
                {
                    result.digits[i + j] += this.digits[i] * other.digits[j];
                    if (result.digits[i + j] >= 10)
                    {
                        result.digits[i + j + 1] += result.digits[i + j] / 10;
                        result.digits[i + j] %= 10;
                    }
                }
            }
            result.RemoveLeadingZeros();
            return result;
        }
        else
        {
            for (int i = 0; i < m; i++)
            {
                xright.digits.Add(this.digits[i]);
            }
            for (int i = m; i < this.digits.Count; i++)
            {
                xleft.digits.Add(this.digits[i]);
            }

            for (int i = 0; i < m; i++)
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
           BigInteger numM = (xleft.Add(xright)).Mul(yleft.Add(yright)).Sub(numR).Sub(numL); ;
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
        BigInteger num1 = new BigInteger("250000000000000000000000000000447994033122339944857");
        BigInteger num2 = new BigInteger("100000000000000000000000000000000000087667839374684");
        Bounse1 bounse1 = new Bounse1();
        
        BigInteger sum = num1.Add(num2);
        Console.WriteLine("Sum: " + sum);
        BigInteger diff = num2.Sub(num1);
        Console.WriteLine("Difference: " + diff);
        BigInteger product = num1.Mul(num2);
        Console.WriteLine("Product: " + product);
        Console.WriteLine("Is num1 even? " + num1.IsEven());
        Console.WriteLine("Is num2 even? " + num2.IsEven());
        Console.WriteLine("num1 < num2? " + num1.LessThan(num2));
        BigInteger smallA = new BigInteger("123");
        BigInteger smallB = new BigInteger("10");
        var (q, r) = BigInteger.BinaryDivMod(smallA, smallB);
        Console.WriteLine("res" + q);
        Console.WriteLine("reminder " + r);
        Console.WriteLine("turn to num " + bounse1.turnToBigInt("Hallo world"));
    }
}

