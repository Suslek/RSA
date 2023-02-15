using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            int input;
            int p;
            int q;
            int e;
            int n;
            int phi;
            int d;

            string InputMessage;
            string RSAEncodedMessage;
            string RSADecodedMessage;

            Random random = new Random();

            Console.WriteLine("введите максимум диапазона в котором генерируются простые числа");
            input = Convert.ToInt32(Console.ReadLine());
            p = GenerateNumber(input, random);
            q = GenerateNumber(input, random);

            n = p * q;
            phi = (p - 1) * (q - 1);
            e = GenerateE(phi);
            d = FindD(phi, e);

            Console.WriteLine("p = " + p);
            Console.WriteLine("q = " + q);
            Console.WriteLine("n = " + n);
            Console.WriteLine("phi = " + phi);
            Console.WriteLine("e = " + e);
            Console.WriteLine("d = " + d);

            Console.WriteLine("Введите выражение");

            InputMessage = Console.ReadLine();
            
            Console.WriteLine("Введеное выражение:" + InputMessage);
            
            RSAEncodedMessage = RSAEncoding(InputMessage, e, n);
            
            Console.WriteLine("Введеное выражение зашифрованное в RSA:" + RSAEncodedMessage);

            RSADecodedMessage = RSADecoding(RSAEncodedMessage, d, n);
            
            Console.WriteLine("Введеное выражение расшифрованное из RSA:" + RSADecodedMessage);

            Console.ReadLine();
        }

        static int GenerateNumber(int range, Random random)
        {
            int result = random.Next(2, range);

            while(IsPrimeCheck(result) == false)
            {
                result++;
            }
            return result;
        }

        static bool IsPrimeCheck(int input)
        {
            for (int i = 2; i < input; i++)
            {
                if(input%i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        static int GenerateE(int phi)
        {
            int result = 2;
            while (Math.Abs(phi%result) != 1)
            {
                result++;
            }
            return result;
        }

        static int FindD(int phi, int e)
        {
            int result = 1;
            while(e*result%phi != 1)
            {
                result++;
            }
            return result;
        }

        static string RSAEncoding(string inputMessage, int e, int n)
        {
            var inputEncodedBytes = Encoding.UTF8.GetBytes(inputMessage);
            int[] tmp = new int[inputEncodedBytes.Length];
            for (int i = 0; i < inputEncodedBytes.Length; i++)
            {
                //tmp[i] = Convert.ToUInt64(Math.Pow(inputEncodedBytes[i],e)%n);

                tmp[i] = inputEncodedBytes[i] % n;
                for (int j = 1; j < e; j++)
                {
                    tmp[i] = tmp[i] * inputEncodedBytes[i] % n;
                }
            }
            var result = Convert.ToBase64String(inputEncodedBytes);
            return result;
        }

        static string RSADecoding(string RSAEncodedMessage, int d, int n)
        {
            var inputEncodedBytes = Convert.FromBase64String(RSAEncodedMessage);
            int[] tmp = new int[inputEncodedBytes.Length];
            for (int i = 0; i < inputEncodedBytes.Length; i++)
            {
                //tmp[i] = Convert.ToUInt64(Math.Pow(inputEncodedBytes[i], d) % n);

                tmp[i] = inputEncodedBytes[i] % n;
                for (int j = 1; j < d; j++)
                {
                    tmp[i] = tmp[i] * inputEncodedBytes[i] % n;
                }
            }
            var result = Encoding.UTF8.GetString(inputEncodedBytes);
            return result;
        }
    }
}
