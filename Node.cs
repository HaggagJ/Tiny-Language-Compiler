using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilers
{
     public class Node
    {
        public List<Node> children = new List<Node>();
        public string Text = "";
        int counter = 0;
        public int token_number=0;
        public Node parent;
        public Node r_sib;
        public Node(string s) { Text = s;
        }

        public Node addchild(Node s)
        {
            children.Add(s);
            counter++;
            s.parent = this;
            return children[children.Count - 1];
        }
        public void addSibling(Node s)
        {
            r_sib = s;
        }
        public void removechild(Node s)
        {
            if (children.Contains(s)) children.Remove(s);
        }
        public Node getnextchild()
        {
            return children[counter++]; 
        }
        public Node addchild(string s)
        {
            children.Add(new Node(s));
            counter++;
            return children[children.Count - 1];
        }
        
    }
}
