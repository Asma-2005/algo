using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Numerics;
using System.Collections;
 public struct result
{
   public int elapsedTime;
   public string value;
   public string type;
    public result(int elapsedTime, string value, string type)
    {
        this.elapsedTime = elapsedTime;
        this.value = value;
        this.type = type;
    }
}


    namespace algo{

    internal class FileHandling
    {

        public static void Encyption_Decryption_file() {
            string InputFilePath;
            string InputFileName;
            while (true)
            {
                Console.WriteLine("Enter Path of the Input Folder: ");
                InputFilePath = Console.ReadLine();

                if (!Directory.Exists(InputFilePath))
                {
                    Console.WriteLine("The folder path does not exist. Please enter a valid folder path.");
                    continue; 
                }
                Console.WriteLine("Enter Input File Name: ");
                InputFileName = Console.ReadLine();

                InputFilePath = Path.Combine(InputFilePath, InputFileName + ".txt");

                if (!File.Exists(InputFilePath))
                {
                    Console.WriteLine($"File '{InputFilePath}' not found. Please check the file name and try again.");
                    continue; 
                }

              
                break;
            }
            Console.WriteLine("File found! Please wait... \n");
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(InputFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    lines.Add(line);
                }
            }
            int testcount = int.Parse(lines[0]);



            Dictionary<int,result> output = new Dictionary<int,result>(); 
            int Key=0;
            for (int i = 1; i < testcount*4; i += 4)
            {
                BigInteger copy_result = new BigInteger();
                BigInteger N = new BigInteger(lines[i]);
                BigInteger EorD = new BigInteger(lines[i+1]);
                BigInteger MorEM = new BigInteger(lines[i+2]);
                int check=int.Parse(lines[i+3]);
                int start_time = System.Environment.TickCount;
                if (check == 0)  // encryption
                {
                    copy_result = copy_result.Encryption(MorEM, EorD, N);
                    int end_time = System.Environment.TickCount;
                    output[Key]= new result((end_time - start_time), copy_result.ToString(),"Encryption result: ");
                }
                else if (check == 1) { // decryption
                    copy_result = copy_result.Decryption(MorEM, EorD, N);
                    int end_time = System.Environment.TickCount;
                    output[Key] = new result((end_time - start_time), copy_result.ToString(), "Decryption result: ");
                }

                Key++;
            }
            string OutputFilePath;
            string OutputFileName;
            while (true)
            {
                Console.WriteLine("Enter Path of the folder:");
                OutputFilePath = Console.ReadLine();

                if (!Directory.Exists(OutputFilePath))
                {
                    Console.WriteLine("The folder path does not exist. Please enter a valid folder path.");
                    continue; 
                }
                Console.WriteLine("Enter File Name:");
                OutputFileName = Console.ReadLine();

                OutputFilePath = Path.Combine(OutputFilePath, OutputFileName + ".txt");

                              
                break;
            }
            Console.WriteLine("___________________________DONE!___________________________ \n");
            using (StreamWriter writer = new StreamWriter(OutputFilePath))
            {
                for (int i = 0; i < testcount; i++) {
                    writer.WriteLine(i+1+" - "+ output[i].type + output[i].value + "\n"+ "Time elapsed: "+ output[i].elapsedTime + " ms\n-------------------------------------------------");
                
                }
            }
        }
        public static void Arithmetic_file() {
            string InputFilePath;
            string InputFileName;
            while (true)
            {
                Console.WriteLine("Enter Path of the Input Folder:");
                InputFilePath = Console.ReadLine();

                if (!Directory.Exists(InputFilePath))
                {
                    Console.WriteLine("The folder path does not exist. Please enter a valid folder path.");
                    continue; 
                }
                Console.WriteLine("Enter intput File Name:");
                InputFileName = Console.ReadLine();

                InputFilePath = Path.Combine(InputFilePath, InputFileName + ".txt");

                if (!File.Exists(InputFilePath))
                {
                    Console.WriteLine($"File '{InputFilePath}' not found. Please check the file name and try again.");
                    continue; 
                }

                break;
            }
            Console.WriteLine("File found! Please wait... \n");
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(InputFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    lines.Add(line);
                }
            }
           int testcount = int.Parse(lines[0]);
           Dictionary<int, result> output = new Dictionary<int, result>();
            int Key = 0;
            Console.WriteLine(       "1- Addtion \n"+
                                     "2- Subtraction \n"+
                                     "3- Multiplication \n"+
                                     "4- Division \n"+
                                     "5- Modelus\n"+
                                     "Enter Opration Number: ");
            int choice = int.Parse(Console.ReadLine());
            for (int i = 1; i < testcount * 2; i += 2)
            {
               
                BigInteger copy_result = new BigInteger();
                BigInteger Number1 = new BigInteger(lines[i]);
                BigInteger Number2 = new BigInteger(lines[i + 1]);
                int start_time = System.Environment.TickCount;
                int end_time;
                switch (choice)
                {
                    case 1:
                        copy_result = Number1.Add(Number2);
                        end_time = System.Environment.TickCount;
                        output[Key] = new result((end_time - start_time), copy_result.ToString(), "Addtion result: ");
                        break;
                    case 2:
                        copy_result = Number1.Sub(Number2);
                        end_time = System.Environment.TickCount;
                        output[Key] = new result((end_time - start_time), copy_result.ToString(), "Subtraction result: ");
                        break;
                    case 3:
                        copy_result = Number1.Mul(Number2);
                        end_time = System.Environment.TickCount;
                        output[Key] = new result((end_time - start_time), copy_result.ToString(), "Multiplication result: ");
                        break;
                    case 4:
                        copy_result = BigInteger.BinaryDivMod(Number1, Number2).Quotient;
                        end_time = System.Environment.TickCount;
                        output[Key] = new result((end_time - start_time), copy_result.ToString(), "Division result: ");
                        break;
                    case 5:
                        copy_result = BigInteger.BinaryDivMod(Number1, Number2).Remainder;
                        end_time = System.Environment.TickCount;
                        output[Key] = new result((end_time - start_time), copy_result.ToString(), "Modelus result: ");
                        break;
                    default:
                        break;
                }
                Key++;
            }
            string OutputFilePath;
            string OutputFileName;
            while (true)
            {
                Console.WriteLine("Enter Path of the OutputFolder:");
                OutputFilePath = Console.ReadLine();

                if (!Directory.Exists(OutputFilePath))
                {
                    Console.WriteLine("The folder path does not exist. Please enter a valid folder path.");
                    continue;
                }
                Console.WriteLine("Enter Output File Name:");
                OutputFileName = Console.ReadLine();

                OutputFilePath = Path.Combine(OutputFilePath, OutputFileName + ".txt");
                break;
            }
            Console.WriteLine("=================================================DONE!================================================= \n");
            using (StreamWriter writer = new StreamWriter(OutputFilePath))
            {
                for (int i = 0; i < testcount; i++)
                {
                    writer.WriteLine(i + 1 + " - " + output[i].type + output[i].value + "\n" + "Time elapsed: " + output[i].elapsedTime + " ms\n=================================================");

                }
            }

        }


    }
}
