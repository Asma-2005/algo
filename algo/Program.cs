using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;


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
        BigInteger num1 = new BigInteger("3100675333279421257617608000138708458801868104897830766303748759001197941598947667658921326203715302820376126044350727358334784388707871335695033578987322915001084651142521380653788065896776145986861827114585345797313243712035560159039288370426232610063160693819476708720025406607223686363847361116574606364158447113777534349736920690749819066439287617387982955");
        BigInteger num2 = new BigInteger("1171369677597009928756935527390310076747104574041487803153297373026018778590309248490666613573253409796114715852296556570425972890140974834424931319354962747988270078167895368091978679700466964125155929802558454004250319697247623603299250283");
        //Bounse1 bounse1 = new Bounse1();

        //BigInteger sum = num1.Add(num2);
        //Console.WriteLine("Sum: " + sum);
        //BigInteger diff = num2.Sub(num1);
        //Console.WriteLine("Difference: " + diff);
        BigInteger product = num1.Mul(num2);
        Console.WriteLine("Product: " + product);
        //Console.WriteLine("Is num1 even? " + num1.IsEven());
        //Console.WriteLine("Is num2 even? " + num2.IsEven());
        //Console.WriteLine("num1 < num2? " + num1.LessThan(num2));
        //BigInteger smallA = new BigInteger("123");
        //BigInteger smallB = new BigInteger("10");
        //var (q, r) = BigInteger.BinaryDivMod(smallA, smallB);
        //Console.WriteLine("res" + q);
        //Console.WriteLine("reminder " + r);
        //Console.WriteLine("turn to num " + bounse1.turnToBigInt("Hallo world"));
        BigInteger m = new BigInteger("2003");
        BigInteger e = new BigInteger("7");
        BigInteger mod = new BigInteger("3713");
        BigInteger d = new BigInteger("2563");
        BigInteger x = new BigInteger("0");
        x = x.Encryption(m, e, mod);
        Console.WriteLine(x + "   encryption\n");
        BigInteger en = new BigInteger("0");
        en = en.Encryption(x, d, mod);
        Console.WriteLine(en + "  decryption\n");
    }
}