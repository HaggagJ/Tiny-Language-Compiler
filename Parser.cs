using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilers
{
    public class Parser
    {
        
        public static int p = 0;

        public static bool error;

        static Form1 e = new Form1();
        public Node parse()
        {


            Node root = stmt_seq();
            if (p < Scanner.tokens.Count)
            {
                error = true;
                
                
            }
            p= 0;
            return root;


        }
        private static Node stmt_seq()
        {

            Node n = statement();
            Node cur = n;
            Scanner.Token tok = new Scanner.Token();
            if (p < Scanner.tokens.Count)
                tok = Scanner.tokens[p];
            while (tok.t == Scanner.TokenType.SEMICOLON && p != Scanner.tokens.Count - 1)
            {
                match(Scanner.TokenType.SEMICOLON, Scanner.tokens[p].t);
                cur.addSibling(statement());
                cur = cur.r_sib;
                if (p < Scanner.tokens.Count)
                    tok = Scanner.tokens[p];
                else
                    break;
            }
            return n;
           
        }
       
        private static Node statement()
        {
            //node = node.Nodes.Add("statement");
            //skip comments
            Node cur = new Node("");
            while (p < Scanner.tokens.Count-1 &&( Scanner.tokens[p].t == Scanner.TokenType.COMMENT)  )
            {
                p++;
            }
            if (Scanner.tokens[p].t== Scanner.TokenType.IF)
            {
                cur=if_stmt();
            }
            else if (Scanner.tokens[p].t== Scanner.TokenType.REPEAT)
            {
                cur=repeat_stmt();
            }
            else if (Scanner.tokens[p].t== Scanner.TokenType.READ)
            {
                cur=read_stmt();
            }
            else if (Scanner.tokens[p].t== Scanner.TokenType.WRITE)
            {
                cur=write_stmt();
            }
            else if (Scanner.tokens[p].t == Scanner.TokenType.IDENTIFIER)
            {
                cur=assign_stmt();
            }
            else {  error = true;
                  }
            //cout ("statement ok");
            return cur;
        }
        //
        private static Node if_stmt()
        {
            //TreeNode node;
            Node cur = new Node (Scanner.TokenType.IF.ToString());
            //cur.token_number = p;
            match(Scanner.TokenType.IF, Scanner.tokens[p].t);
            cur.addchild(exp());
            match(Scanner.TokenType.THEN, Scanner.tokens[p].t);
            cur.addchild(stmt_seq());
            Scanner.Token n = new Scanner.Token();
            if(p<Scanner.tokens.Count)
                n=Scanner.tokens[p];
            if (n.t == Scanner.TokenType.ELSE)
            {
                match(Scanner.TokenType.ELSE, Scanner.tokens[p].t);
                cur.addchild(stmt_seq());
            }
            match(Scanner.TokenType.END, Scanner.tokens[p].t);
            return cur;
        }

        //repeat-stmt -> repeat stmt-seq until exp
        private static  Node repeat_stmt()
        {
            Node cur = new Node(Scanner.TokenType.REPEAT.ToString());
            //cur.token_number = p;
            match(Scanner.TokenType.REPEAT, Scanner.tokens[p].t);
            cur.addchild(stmt_seq());
            match(Scanner.TokenType.UNTIL, Scanner.tokens[p].t);
            cur.addchild(exp());
            return cur;
          
        }

        //read-stmt -> read IDENTIFIER
        private static Node read_stmt()
        {
            match(Scanner.TokenType.READ, Scanner.tokens[p].t);
            Node cur =new Node( Scanner.TokenType.READ.ToString() + "\n(" + Scanner.tokens[p].val+ ")");
            //cur.token_number = p;
            match(Scanner.TokenType.IDENTIFIER, Scanner.tokens[p].t);
            return cur;
        }

        //write-stmt -> write exp
        private static  Node write_stmt()
        {
            //TreeNode node;
            Node cur=new Node( Scanner.TokenType.WRITE.ToString());
            //cur.token_number = p;
            match(Scanner.TokenType.WRITE, Scanner.tokens[p].t);
            cur.addchild(exp());
            return cur;
        }

        //assign-stmt -> IDENTIFIER := exp
        private static Node assign_stmt()
        {
            //TreeNode node;
            Node cur=new Node(Scanner.TokenType.ASSIGN.ToString()+ "\n(" + Scanner.tokens[p].val+")");
            //cur.token_number = p;
            match(Scanner.TokenType.IDENTIFIER, Scanner.tokens[p].t);
            match(Scanner.TokenType.ASSIGN, Scanner.tokens[p].t);
            cur.addchild(exp());
            return cur;
        }
        //exp -> simple-exp [comparison-op simple exp]
        private static Node exp()
        {
            Node cur;
            Node n=simple_exp();
           
            Node n1;
            Scanner.Token tok;
            if (p < Scanner.tokens.Count)
            {
                tok=Scanner.tokens[p];
            }
            //should be while
            if (p<Scanner.tokens.Count-1 && (Scanner.tokens[p].t== Scanner.TokenType.LESSTHAN || Scanner.tokens[p].t== Scanner.TokenType.EQUAL))
            {

                cur = comparison_op();
            
                cur.addchild(n);
                n1 = simple_exp();
                //n1.token_number = p;
                cur.addchild(n1);
                n = cur;

            }

            return n;
        }

        //simple-exp -> term {addop term}
        private static  Node simple_exp()
        {
            Node cur;
            Node n = term();
           
            Node n1;
            while (p<Scanner.tokens.Count-1 && (Scanner.tokens[p].t == Scanner.TokenType.PLUS || Scanner.tokens[p].t == Scanner.TokenType.MINUS))
            {
                cur = addop();
                
                cur.addchild(n);
                n1 = term();
                //n1.token_number = p;
                cur.addchild(n1);
                n= cur;
                //fe hena heta na2sa mlhash lazma
            }
            return n;
            
        }
        //term -> factor {mulop factor}
        private static Node term( )
        {
            //this is the case that the next token is an operator, make 
            Node cur;
            Node n = factor();
            
            Node n1;
            while (p<Scanner.tokens.Count-1 && (Scanner.tokens[p].t == Scanner.TokenType.MULT || Scanner.tokens[p].t == Scanner.TokenType.DIV))
            {
                cur = mulop();
                
                cur.addchild(n);
                n1 = factor();
               
                cur.addchild(n1);
                n = cur;

                //fe hena heta na2sa mlhash lazma
            }
            return n;

        }
        //factor -> number | IDENTIFIER | ( exp )
        private static  Node factor( )
        {
            Node cur = new Node("");
            if (Scanner.tokens[p].t == Scanner.TokenType.NUMBER) { 
                cur=new Node("CONST\n(" + Scanner.tokens[p].val+ ")");
                //cur.token_number = p;
                match(Scanner.tokens[p].t, Scanner.TokenType.NUMBER);
            }

            else if (Scanner.tokens[p].t == Scanner.TokenType.OPENBRACKET)
            {
                
                match(Scanner.TokenType.OPENBRACKET, Scanner.tokens[p].t);
                cur=exp();
                match(Scanner.TokenType.CLOSEDBRACKET, Scanner.tokens[p].t);
            }
            else if (Scanner.tokens[p].t == Scanner.TokenType.IDENTIFIER)
            {
                cur=new Node("ID\n(" + Scanner.tokens[p].val+ ")");
                //cur.token_number = p;
                match(Scanner.TokenType.IDENTIFIER, Scanner.tokens[p].t);
            }
            else if(Scanner.tokens[p].t == Scanner.TokenType.MINUS)
            {
                match(Scanner.TokenType.MINUS, Scanner.tokens[p].t);//match minus need number
                cur = new Node("CONST\n(-" + Scanner.tokens[p].val + ")");
                match(Scanner.tokens[p].t, Scanner.TokenType.NUMBER);
            }
            else
            {
                error = true;
            }
            return cur;
        
        }
        //addop -> + | -
        private static  Node addop()
        {
            Node cur = new Node("");
            if (Scanner.tokens[p].t== Scanner.TokenType.PLUS)
            {
                
                cur = new Node("OP\n(" + Scanner.tokens[p].val + ")");
                //cur.token_number = p;
                match(Scanner.tokens[p].t, Scanner.TokenType.PLUS);
     
            }
            else if (Scanner.tokens[p].t== Scanner.TokenType.MINUS)
            {
                cur = new Node("OP\n(" + Scanner.tokens[p].val + ")");
                //cur.token_number = p;
                match(Scanner.TokenType.MINUS, Scanner.tokens[p].t);
            }
            else
            {
                error = true;
                //cout("Expected + or - but found none.");
            }
            return cur;
        }
        public static Node mulop()
        {
            
            
            Node cur = new Node("");
            if (Scanner.tokens[p].t ==Scanner.TokenType.MULT)
            {
                //cur.token_number = p;
                cur = new Node("OP\n(*)");
                match(Scanner.TokenType.MULT, Scanner.tokens[p].t);
            }
            else if (Scanner.tokens[p].t == Scanner.TokenType.DIV)
            {
                //cur.token_number = p;
                cur = new Node("OP\n(/)");
                match(Scanner.TokenType.DIV, Scanner.tokens[p].t);
            }
            else
            {
                error = true;
               
                // qDebug() << QString::fromStdString("mulop error" + to_string(Parser_index));
            }
            return cur;
        }

        //comparison-op -> < | =
        private static  Node comparison_op()
        {
            Node cur=new Node("");
            if (Scanner.tokens[p].t== Scanner.TokenType.EQUAL)
            {
                //cur.token_number = p;
                cur = new Node("OP\n(=)");
                match(Scanner.tokens[p].t, Scanner.TokenType.EQUAL);
            }
            else if (Scanner.tokens[p].t== Scanner.TokenType.LESSTHAN)
            {
                //cur.token_number = p;
                match(Scanner.tokens[p].t, Scanner.TokenType.LESSTHAN);
                cur = new Node("OP\n(<)");
                
            }
            else
            {
                error = true;
                //cout("Expected < or = but found none.");
            }
            return cur;
        }
        private static void match(Scanner.TokenType expr, Scanner.TokenType input)
        {
            //skip comments
            while (Scanner.tokens[p].t == Scanner.TokenType.COMMENT && p < Scanner.tokens.Count)
            {
                p++;
                input = Scanner.tokens[p].t;
            }
            if (expr == input && p < Scanner.tokens.Count)
            {
                p++; 
            }
            else
            {
                error = true;
            }
        }
        
    }
}