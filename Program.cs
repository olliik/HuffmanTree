/* LAHDE: http://web.fi.uba.ar/~jvillca/hd/public/books/Data_Structures_and_Algorithms_in_Java_2nd_Edition.pdf s.419

    1. Make a Node object (as seen in tree.java) for each character used in the
    message. For our Susie example that would be nine nodes. Each node has two
    data items: the character and that character’s frequency in the message. Table
    8.4 provides this information for the Susie message.

    2. Make a tree object for each of these nodes. The node becomes the root of the
    tree.

    3. Insert these trees in a priority queue (as described in Chapter 4). They are
    ordered by frequency, with the smallest frequency having the highest priority.
    That is, when you remove a tree, it’s always the one with the least-used
    character.

    Now do the following:
    1. Remove two trees from the priority queue, and make them into children of a
    new node. The new node has a frequency that is the sum of the children’s
    frequencies; its character field can be left blank.

    2. Insert this new three-node tree back into the priority queue.

    3. Keep repeating steps 1 and 2. The trees will get larger and larger, and there will
    be fewer and fewer of them. When there is only one tree left in the queue, it is
    the Huffman tree and you’re done


    The characters with the highest counts should be coded with a small number of bits.

    */


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
