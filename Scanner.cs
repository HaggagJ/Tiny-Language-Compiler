using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
/*
Tiny Language:

Reserved Words  SpecialSymbols  Other
if              +               number
then            -               (1 or more digits)
else            *
end             /
repeat          =               identifier
until           <               (1 or more letters)
read            (
write           )
                ;
                :=

-identifiers are letters only no numbers
-Comments are enclosed in curly brackets {...} and cannot be nested.
-White space consists of blanks, tabs, and newlines.
*/

namespace Compilers
{
    class Scanner
    {
        public enum State
        {
            START,
            INCOMMENT,
            INNUM,
            INID,
            INASSIGN,
            ERROR,
            DONE
        };

        public enum charType
        {
            letter,
            number,
            specialSymbol,/*any symbol that is valid except comments and := symbols*/
            invalidEntry,
            openCommentSymbol,
            closeCommentSymbol,
            assign,
            blank
        }
        public enum TokenType
        {
            IF, THEN, ELSE, END, REPEAT, UNTIL, READ, WRITE, /*keywords*/
            OPENBRACKET = 40, CLOSEDBRACKET = 41, MUL = 42, PLUS = 43, MINUS = 45, DIV = 47, ASSIGN = 58, DELIMITER = 59, LESSTHAN = 60, EQUAL = 61, /*special symbols have enum values = ascii number to be easier to get their tokentype*/
            NUM, ID,/*constants and identifiers*/
            COMMENT, /*COMMENTS*/
            ERROR
        }
        public struct Token
        {
            public TokenType t;
            public string val;
            public int token_number;
        }

        /*this function is used to get the type of character that is read from input file*/
        static public charType getCharType(ref char c)
        {
            if (c == ' '/*TAB*/ | c == ' '/*space*/ | (int)c == 10 /*new line*/ | (int)c==13 | (int)c == 65535 /*EOF is ignored too like blanks*/)
            {
                return charType.blank;
            }
            else if ((c >= 'A' && c <= 'Z')/*capital letters*/ | (c >= 'a' && c <= 'z') /*small letters*/ )
            {
                return charType.letter;
            }
            else if (c >= '0' && c <= '9' /*numbers*/)
            {
                return charType.number;
            }
            else if ((c >= '(' && c <= '+') /* ( ) * + */ | c == '-'| c == '/' | c == ';' | c == '<' | c == '=')
            {
                return charType.specialSymbol;
            }
            else if (c == '{')
            {
                return charType.openCommentSymbol;
            }
            else if(c==':'){
                return charType.assign;/*do nothing, handled within code*/}
            else
            {
                return charType.invalidEntry;
            }
        }

        public static void getTokens(string input_path, ref List<Token> tokens)
        {
            //List<Token> tokens = new List<Token>();
            StreamReader streamReader = new StreamReader(input_path);
            char c=' ';
            int token_counter = 0;
            //string tokenTypeName="";
            State state = State.START;
            Token tempToken = new Token{ };
            string token_value = "";
            while (streamReader.Peek() != -1 || state!=State.START)
            {
                c = (char)streamReader.Peek();//peak function doesnt consume the character
                
                charType type = getCharType(ref c);
                switch (state)
                {
                    case State.START:
                        /*letters*/
                        if (type==charType.letter)
                        {
                            token_value += (char)streamReader.Read();//read method consumes character;
                            state = State.INID;
                        }
                        /*numbers*/
                        else if (type==charType.number)
                        {
                            token_value += (char)streamReader.Read();
                            state = State.INNUM;
                        }
                        /*special symbols*/
                        else if (type==charType.specialSymbol)
                        {
                            token_value += (char)streamReader.Read();
                            tempToken.t = (TokenType)(int)c;
                            state = State.DONE;
                        }
                        /*comment*/
                        else if (type==charType.openCommentSymbol)
                        {
                            state = State.INCOMMENT;
                            token_value += (char)streamReader.Read();
                        }
                        /*assign*/
                        else if (type==charType.assign)
                        {
                            token_value += (char)streamReader.Read();
                            state = State.INASSIGN;
                        }
                        /*blanks*/
                        else if (type==charType.blank)
                        {
                              streamReader.Read();
                             /*BLANKS stay in same start state , just consume them*/
                        }
                        /*invalid entry*/
                        else if (type==charType.invalidEntry)
                        {
                            state = State.ERROR;
                        }

                        /*White spaces and blanks go to state start by default*/
                        /*end of start state*/
                        break;

                    case State.INNUM:
                        if (type==charType.number)
                        {
                            token_value += (char)streamReader.Read();
                            /*state stays the same*/
                        }
                        else if(type==charType.letter || type==charType.invalidEntry){
                            /*error state since either the number has letters concatenated to it or invalid character is typed*/
                            state=State.ERROR;
                            
                        }
                        else
                        {
                            state = State.DONE;
                            tempToken.t = TokenType.NUM;
                        }
                        break;

                    case State.INID:
                        if (type==charType.letter)
                        {
                            token_value += (char)streamReader.Read();
                            /*state stays the same*/
                        }
                        else if(type==charType.number || type==charType.invalidEntry){
                            /*error state since either the word has numbers concatenated to letters or invalid character is typed*/
                            state=State.ERROR;
                            
                        }
                        else /*special symbols or assign*/
                        {
                            state = State.DONE;
                            /*if it is not from the keywords it is an identifier*/
                            tempToken.t = TokenType.ID;
                            for (TokenType i = 0; (int)i < 8; i++)
                            {
                                /*compare the letters string to the tokentypes to determine wether it is a keyword or identifier*/
                                if (token_value.Equals(i.ToString(), StringComparison.OrdinalIgnoreCase)/*compare and ignore case*/)
                                {
                                    tempToken.t = (TokenType)i; /*i represents the number of tokentype in tokentype enum since the token types have same indices like their string names array*/
                                    break;                                
                                }
                            }
                        }
                        break;

                    case State.INASSIGN:
                        if (c == '=')
                        {
                            token_value += (char)streamReader.Read();
                            tempToken.t = TokenType.ASSIGN;
                            state = State.DONE;

                        }
                        else /*incorrect assign operator error*/
                        {   
                            state = State.ERROR;
                        }
                        break;

                    case State.INCOMMENT:
                        if (c == '}')
                        {
                            state = State.DONE;
                            token_value += (char)streamReader.Read();
                            tempToken.t = TokenType.COMMENT;
                        }
                        else
                        {
                            token_value += (char)streamReader.Read();
                            /*state stays as it is*/
                        }
                        break;

                    case State.DONE:
                        tempToken.val = token_value;
                        tempToken.token_number = token_counter;
                        token_value = "";
                        token_counter++;
                        tokens.Add(tempToken);
                        state = State.START;

                        break;

                    case State.ERROR:
                        /*dont leave the error state until blank space or special symbol "different token"*/
                        /*consume the erroring character here*/
                        if(type==charType.letter || type==charType.number || type==charType.invalidEntry){
                            token_value+=(char)streamReader.Read();
                        }
                        else{
                        tempToken.val = token_value;
                        tempToken.t=TokenType.ERROR;
                        tempToken.token_number = token_counter;
                        token_counter++;
                        tokens.Add(tempToken);
                        state=State.START;
                        token_value = "";
                        }
                    break; 

                }
            }


        }

        public static void Main(string[] args)
        {
            string input = "input.txt";
            List<Token> tokens =new List<Token>();
            getTokens(input,ref tokens);

            for(int i =0; i<tokens.Count;i++){
               Console.WriteLine("{0}\t{2}\t{1}" , tokens[i].val , tokens[i].token_number ,tokens[i].t.ToString());
            }

        }
    }
}