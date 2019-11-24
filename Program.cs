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
        static int rivinPituus = 0; // Tarkistusnumero encodaamiseen
        static void Main(string[] args)
        {
            /* Accept a text message - possibly more than one line */
            //System.Console.WriteLine("Type in a text message");
            //string str = Console.ReadLine();

            // debug
            string str = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.";
            HuffmanTree tree = new HuffmanTree();
            tree.BuildTree(str);

            /* 
                Rakennetaan Encode Table dictionary muotoon 
                Käytän codeTablea parametrinä myöhemmin EncodeMessage metodissa.
            */
            Dictionary<Node, string> codeTable = tree.GenerateCodeTable(tree.root, "");
            TulostaCodeTable(codeTable);

            // Käyttäjän syöttämä rivi alkuperäisessä muodossaan.
            System.Console.WriteLine();
            System.Console.WriteLine("Alkuperäinen rivi: ");
            System.Console.WriteLine(str);


            // Luodaan uusi string muuttuja joka on "binääri" -muodossa..
            string encodedString = tree.EncodeMessage(codeTable, str, rivinPituus);
            System.Console.WriteLine();
            System.Console.WriteLine("Encodattu rivi: ");
            System.Console.WriteLine(encodedString);


            /* Decode() metodille annetaan parametreiksi binaariPuu ja encodattu viesti. */
            System.Console.WriteLine();
            System.Console.WriteLine("Decodattu rivi:");
            tree.Decode(tree.root, encodedString);
            System.Console.WriteLine();
            System.Console.WriteLine();
        }

        static void TulostaCodeTable(Dictionary<Node, string> EncodeTable)
        {
            System.Console.WriteLine("Encodattu taulu:");
            System.Console.WriteLine("char:\t\tbinary: \tfrequency:");
            foreach (KeyValuePair<Node, string> item in EncodeTable)
            {
                System.Console.WriteLine(item.Key.c + "\t\t" + item.Value + "\t\t" + item.Key.freq);
                rivinPituus += item.Key.freq * item.Value.Length;
            }
        }
    }
}
