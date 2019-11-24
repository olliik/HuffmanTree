using System;
using System.Collections.Generic;
using System.Linq;

namespace HuffmanTreenausta
{
    public class HuffmanTree
    {
        public Dictionary<Node, string> d = new Dictionary<Node, string>();
        private List<Node> nodelista = new List<Node>();
        public Node root { get; set; }

        public Node BuildTree(string str)
        {
            /*
                1. Make a Node object (as seen in tree.java) for each character used in the
                message. For our Susie example that would be nine nodes. Each node has two
                data items: the character and that character’s frequency in the message. Table
                8.4 provides this information for the Susie message.
            */
            Dictionary<char, int> nodeDictionary = new Dictionary<char, int>();

            /* Loopataan käyttäjän antama string läpi ja luodaan niistä dictionary mihin saadaan erittäin helposti kirjattua ylös montako kertaa kyseinen merkki on esiintynyt. */
            for (int i = 0; i < str.Length; i++)
            {
                if (!nodeDictionary.ContainsKey(str[i]))
                {
                    nodeDictionary.Add(str[i], 1);
                }
                else
                {
                    /* https://stackoverflow.com/a/7132978 */
                    nodeDictionary.TryGetValue(str[i], out int currentFreq);
                    nodeDictionary[str[i]] = currentFreq + 1;
                }
            }
            /* 
                Dictionaryn avulla luodaan nyt loopin sisällä Node-oliot jokaisesta esiintynestä merkistä, frequence saadaan poimittua value arvosta.
                Dictionarya ei tämän jälkeen enää käytetä, vaan jatkossa toimitaan listalla joka sisältää kaikki oliot.
             */
            foreach (KeyValuePair<char, int> item in nodeDictionary)
            {
                root = new Node(item.Key, item.Value);
                nodelista.Add(root);
            }

            /*
                3. Insert these trees in a priority queue (as described in Chapter 4). They are
                ordered by frequency, with the smallest frequency having the highest priority.
                That is, when you remove a tree, it’s always the one with the least-used
                character.

                Järjestetään lista frequencyn mukaan.
                sort a list of objects by property: https://stackoverflow.com/a/3309230
            */
            List<Node> priorityQueue = nodelista.OrderBy(node => node.freq).ToList();

            /* Now do the following:
                1. Remove two trees from the priority queue, and make them into children of a
                new node. The new node has a frequency that is the sum of the children’s
                frequencies; its character field can be left blank.
            
                2. Insert this new three-node tree back into the priority queue. 
            
                3. Keep repeating steps 1 and 2. The trees will get larger and larger, and there will
                be fewer and fewer of them. When there is only one tree left in the queue, it is
                the Huffman tree and you’re done 
            */

            // Loopataan niin kauan että listassa on vain yksi rivi jäljellä.
            while (priorityQueue.Count > 1)
            {
                // Järjestetään lista joka loopin yhteydessä uudestaan.
                priorityQueue = priorityQueue.OrderBy(node => node.freq).ToList();
                // Luodaan uusi node. Frequencyksi tulee tällä hetkellä kahden päälimmäisen noden yhteenlaskettu arvo.
                // uuden noden sisälle "Nestataan" left ja right olioiksi kaksi päälimmäistä nodea, jonka jälkeen nämä poistetaan listasta.
                Node newNode = new Node('†', priorityQueue[0].freq + priorityQueue[1].freq, priorityQueue[0], priorityQueue[1]);
                // Poistetaan kaksi päälimmäistä nodea listasta, jotka ovat nyt nestattu newNodeen.
                priorityQueue.RemoveAt(0);
                priorityQueue.RemoveAt(0);

                // 2. Lisätään uusi kolmen-noden puu takaisin listaan
                priorityQueue.Add(newNode);

            }
            System.Console.WriteLine("loopattu lapi - huffman puu luotu");
            System.Console.WriteLine();


            /* Tässä vaiheessa koko lista on tiivistetty yhen Node-olion sisään */
            // Poimitaan listan päällimmäinen (ja ainut) olio.            
            root = new Node(priorityQueue[0].c, priorityQueue[0].freq, priorityQueue[0].left, priorityQueue[0].right);
            
            return root;
        }

        /*
            We start at the root of the Huffman tree and follow every possible path to a leaf node. As we go along the path, we remember the sequence of left
            and right choices, recording a 0 for a left edge and a 1 for a right edge. When we
            arrive at the leaf node for a character, the sequence of 0s and 1s is the Huffman code
            for that character. We put this code into the code table at the appropriate index
            number.
        */
        public Dictionary<Node, string> GenerateCodeTable(Node _root, string s)
        {
            /* 
                Jos leaf node (eli kyseisellä nodella ei ole haarautumia mihinkään suuntaan)
                tänne on säilytetty jokin meidän merkeistä ja nyt tämän metodin sisällä sille määritellään oma "binaari arvo"
            */
            if (_root.left == null && _root.right == null)
            {
                d.Add(_root, s);
            }
            if (_root.left != null)
            {
                GenerateCodeTable(_root.left, s + "0");
            }
            if (_root.right != null)
            {
                GenerateCodeTable(_root.right, s + "1");
            }
            return d;
        }

        public string EncodeMessage(Dictionary<Node, string> codeTable, string alkuperainenviesti, int tarkistusnumero)
        {
            string encodedViesti = "";
            // Käydään yksitellen jokainen merkki käyttäjän syöttämästä rivistä.
            for (int i = 0; i < alkuperainenviesti.Length; i++)
            {
                char x = alkuperainenviesti[i];
                // Dictionary on aikaisemmin muodostettu code table.
                foreach (KeyValuePair<Node, string> item in codeTable)
                {
                    // Käydään foreach dictionaryn rivi läpi jos Noden char == nykyinen merkki
                    if (item.Key.c == x)
                    {
                        // lisätään palautettavaan arvoon noden code tablesta löytyvä "binääri arvo".
                        encodedViesti += item.Value;
                    }
                }
            }
            if (encodedViesti.Length == tarkistusnumero)
            {
                return encodedViesti;
            }
            else {
                return "Encode error: Encodatun rivin pituus ja tarkistusnumero eivät täsmää.";
            }
        }
        /* 
            How do we use this tree to decode the message? For each character you start at the
            root. If you see a 0 bit, you go left to the next node, and if you see a 1 bit, you go
            right 
        */

        public void Decode(Node _root, string encodedMsg)
        {
            Node _node = _root;
            for (int i = 0; i < encodedMsg.Length; i++)
            {
                char x = encodedMsg[i];
                if (x == '0') // char = 0 -> go left
                {
                    if (_node.left != null)
                    {
                        _node = _node.left;
                    }
                }
                if (x == '1') // char = 1 -> go right
                {
                    if (_node.right != null)
                    {
                        _node = _node.right;
                    }
                }
                /* 
                    Jos leaf node (eli kyseisellä nodella ei ole haarautumia mihinkään suuntaan)
                    printataan ulos löytynyt merkki.
                */
                if (_node.left == null && _node.right == null)
                {
                    System.Console.Write(_node.c);
                    // palataan puun juureen ja edetään for-loopissa eteenpäin.
                    _node = _root;
                }
            }
        }
    }
}