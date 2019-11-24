using System;

namespace HuffmanTreenausta
{
    public class Node
    {
        public char c { get; set; }
        public int freq { get; set; }
        public Node left { get; set; }
        public Node right { get; set; }

        public Node(char c, int freq)
        {
            this.c = c;
            this.freq = freq;
        }
        public Node(char c, int freq, Node left, Node right)
        {
            this.c = c;
            this.freq = freq;
            this.left = left;
            this.right = right;
        }
        
        
        public override string ToString()
        {
            return c + " " + freq;
        }

    }
}
