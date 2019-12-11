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
            
            //Create a dictionary for tracking down the frequencies for each character            
            Dictionary<char, int> nodeDictionary = new Dictionary<char, int>();

            // Loop through the string parameter, add a new key or increment the frequency of existing one
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
            
            // Loop throught the dicitonary, create new Node objects with the key and value pairs. Add the new node into a list,
            foreach (KeyValuePair<char, int> item in nodeDictionary)
            {
                root = new Node(item.Key, item.Value);
                nodelista.Add(root);
            }
            
            // priorityQueue - Sort the list by frequencies.
            //sort a list of objects by property: https://stackoverflow.com/a/3309230            
            List<Node> priorityQueue = nodelista.OrderBy(node => node.freq).ToList();

            // Loop the list untill there's only one node left.
            while (priorityQueue.Count > 1)
            {
                // Sort the list by frequencies for every loop iteration
                priorityQueue = priorityQueue.OrderBy(node => node.freq).ToList();
                // Create a new branch node, the char is irrelevant. Set the frequency as the sum for the next two nodes on top of the priorityQueue.
                // Nest the two top nodes as left and right and then remove them from the priorityQueue.
                Node newNode = new Node('†', priorityQueue[0].freq + priorityQueue[1].freq, priorityQueue[0], priorityQueue[1]);
                priorityQueue.RemoveAt(0);
                priorityQueue.RemoveAt(0);

                // Add the new Node on to the list.
                priorityQueue.Add(newNode);
                // Repeat while there's only one Node left, which forms our tree.
            }
            
            // Our root node is the only node left in the list. Every other node is now nested into it.
            root = new Node(priorityQueue[0].c, priorityQueue[0].freq, priorityQueue[0].left, priorityQueue[0].right);            
            return root;
        }

        public Dictionary<Node, string> GenerateCodeTable(Node _root, string s)
        {
            // If leaf node == has no left or right children nodes.
            // One of our char's is stored here, add the node and the 'binary'-string to a dictionary that is our code table.
            if (_root.left == null && _root.right == null)
            {
                d.Add(_root, s);
            }
            // if we go left - add a 1 to the binary string
            if (_root.left != null)
            {
                GenerateCodeTable(_root.left, s + "0");
            }
            // if we go right - add a 0 to the binary string
            if (_root.right != null)
            {
                GenerateCodeTable(_root.right, s + "1");
            }
            return d;
        }

        public string EncodeMessage(Dictionary<Node, string> codeTable, string originalmessage, int verificationNumber)
        {
            string encodedMessage = "";
            for (int i = 0; i < originalmessage.Length; i++)
            {
                char x = originalmessage[i];
                // Loop through a dictionary AKA our code table.
                foreach (KeyValuePair<Node, string> item in codeTable)
                {
                    // If our current char is contained in our dictionary
                    if (item.Key.c == x)
                    {
                        // Add this chars "binary"-string value to the return string.
                        encodedMessage += item.Value;
                    }
                }
            }
            if (encodedMessage.Length == verificationNumber)
            {
                return encodedMessage;
            }
            else {
                return "Encode error: Encoded string length and the verification number doesn't match.";
            }
        }


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
                if (_node.left == null && _node.right == null)
                {
                    System.Console.Write(_node.c);
                    // return to the root and continue the for-loop
                    _node = _root;
                }
            }
        }
    }
}
