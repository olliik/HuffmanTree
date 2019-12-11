using System;
using System.Collections.Generic;

namespace HuffmanTreenausta
{
    class Program
    {
        static int verificationNumber = 0; // Verification number for our encoding
        static void Main(string[] args)
        {
            //System.Console.WriteLine("Type in a text message");
            //string str = Console.ReadLine();

            // debug
            string str = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.";
            HuffmanTree tree = new HuffmanTree();
            tree.BuildTree(str);

            // Generate our encode table as a dictionary.
            Dictionary<Node, string> codeTable = tree.GenerateCodeTable(tree.root, "");
            PrintOutEncodeTable(codeTable);

            // The original message.
            System.Console.WriteLine();
            System.Console.WriteLine("Original message: ");
            System.Console.WriteLine(str);

            // Encode the message in 0's and 1's.
            string encodedString = tree.EncodeMessage(codeTable, str, verificationNumber);
            System.Console.WriteLine();
            System.Console.WriteLine("Encoded message: ");
            System.Console.WriteLine(encodedString);


            // Decode the message back to it's original form.
            System.Console.WriteLine();
            System.Console.WriteLine("Decoded message:");
            tree.Decode(tree.root, encodedString);
            System.Console.WriteLine();
            System.Console.WriteLine();
        }

        static void PrintOutEncodeTable(Dictionary<Node, string> EncodeTable)
        {
            System.Console.WriteLine("Encode table:");
            System.Console.WriteLine("char:\t\tbinary: \tfrequency:");
            foreach (KeyValuePair<Node, string> item in EncodeTable)
            {
                System.Console.WriteLine(item.Key.c + "\t\t" + item.Value + "\t\t" + item.Key.freq);
                verificationNumber += item.Key.freq * item.Value.Length;
            }
        }
    }
}
