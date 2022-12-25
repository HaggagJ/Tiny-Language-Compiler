using System.Configuration;
using System.Windows.Forms;
using static Compilers.Scanner;

namespace Compilers
{
    public partial class Form1 : Form
    {
        string file, inputPath, input_file_name;
        bool tokenFlag = false;

        public Form1()
        {
            InitializeComponent();
           
        }
        static int id=0;
        string graphVizString="digraph G { \ngraph[ordering=out];\n";

        private void NotChange(KeyPressEventArgs e) /* function to prevent writing in textbox */
        {
            e.Handled = true;
        }

        private void inputFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotChange(e);
        }

        void addsibling(int p, int c)
        {
            graphVizString += "{rank = \"same\" " + p.ToString() + "->" + c.ToString() + "};\n" ;
        }

        int addNode(string nodeLabel, string nodeShape)
        {
            id++;
            string labelString = id.ToString() + " [label= \"" + nodeLabel + "\""  ;
            if (nodeShape.Contains("rectangle"))
            {
                graphVizString+= labelString + " , shape =  \"rectangle\"];\n" ;
            }
            else if (nodeShape.Contains("ellipse"))
            {
                graphVizString += labelString + " , shape =  \"ellipse\"];\n" ;
            }
            return id;
        }



        public string setshape(string s)
        {
            string d = "";
            if (s.Contains("READ\n(") || s.Contains("IF") || s.Contains("ASSIGN\n(") || s.Contains("WRITE") || s.Contains("REPEAT"))
            {
                d = "rectangle";
                return d;
            }
            else
            {
                d = "ellipse";
                return d;
            }
        }
        void endGraph()
        {
            graphVizString+= "} \n";
        }

        void addChild(int p, int c)
        {
            graphVizString+=  p.ToString() + "->" + c.ToString() + "; \n" ;
        }
        void addInvisibleChild(int p , int c)
        {
            graphVizString += p.ToString() + "->" + c.ToString() + " [style=invis];\n";
        }

        Queue<Node> queue = new Queue<Node>();

        //BFS graph traversal and drawing using graphviz
        public void draw(Node root)
        {
            Node n = root;
            queue.Enqueue(root);
            n.token_number = addNode(n.Text, setshape(n.Text));
            while (n.r_sib != null && n.r_sib.Text!="")
            {
                queue.Enqueue(n.r_sib);
                n.r_sib.token_number = addNode(n.r_sib.Text, setshape(n.r_sib.Text));
                addsibling(n.token_number,n.r_sib.token_number);
                n = n.r_sib;
            }
            while (queue.Count > 0)
            {
                n = queue.Dequeue();
                if (n.children.Count > 0)
                {
                    for (int i =0; i <n.children.Count ; i++)
                    {//add all nodes within the same level , which are children of nodes and their siblings
                        queue.Enqueue(n.children[i]);
                        n.children[i].token_number = addNode(n.children[i].Text, setshape(n.children[i].Text));
                        addChild(n.token_number, n.children[i].token_number);
                        Node q=n.children[i];//used for iteration
                        while (q.r_sib != null && q.r_sib.Text!="")
                        {
                            queue.Enqueue(q.r_sib);
                            q.r_sib.token_number = addNode(q.r_sib.Text, setshape(q.r_sib.Text));
                            addsibling(q.token_number, q.r_sib.token_number);
                            addInvisibleChild(n.token_number, q.r_sib.token_number);
                            q=q.r_sib;
                        }               
                    }
                }
               
            }
            graphVizString += "}";

        }

        private void Btn_Scan_Click(object sender, EventArgs e)
        {
            if (tokenFlag)
            {
                MessageBox.Show("Please enter valid code file", "INFO");
                return;
            }
            try
            {               
                string input_path = file;
                string output_path = Path.Combine(inputPath, input_file_name + "_Scanned.txt");
                StreamWriter x = new StreamWriter(output_path, false);
                Scanner.tokens.Clear();
                Scanner.getTokens(input_path);
                if (Scanner.empty == true)
                {
                    MessageBox.Show("The entered file is empty", "Alert");
                    x.Close();
                    Scanner.empty = false;
                    label1.Text = " ";
                    return;
                }
                Scanner.write(ref x);
                Scanner.tokens.Clear();
                x.Close();
                outputFile.Text = File.ReadAllText(Path.Combine(inputPath, input_file_name + "_Scanned.txt"));
                if (Scanner.error_flag == true)
                {
                    MessageBox.Show("Error State Is Reached", "Alert");
                    Scanner.error_flag = false;
                    return;
                }
                else MessageBox.Show("The file has been Scanned Successfully", "INFO!");

                label1.Text = "The output file has been created in the same path of the input file";
            }
            catch (Exception x)
            {
                if (file == null)
                {
                    MessageBox.Show("Please put an input file!", "Alert");

                }
                else MessageBox.Show(x.Message, "Alert");
            }
        }

        private void btn_Tokens_Click(object sender, EventArgs e)
        {
            try
            {
                
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                ofd.Filter = "TXT files (*.txt)|*.txt"; /* chose type of files*/                
                DialogResult dialogResult= ofd.ShowDialog();
                if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
                StreamReader streamReader1 = new StreamReader(ofd.FileName);
                bool flag = false;
                while (!streamReader1.EndOfStream)
                {
                    if ((char)streamReader1.Read() == ',')
                    {
                        flag = true;
                    }
                }
                streamReader1.Close();
                if (!flag)
                {
                    MessageBox.Show("Please enter valid token file", "Warning");
                    return;
                }
                tokenFlag = true;
                inputFile.Text = File.ReadAllText(ofd.FileName);
                file = ofd.FileName;
                input_file_name = System.IO.Path.GetFileNameWithoutExtension(file);
                inputPath = System.IO.Path.GetDirectoryName(file);
                Scanner.tokens.Clear();
                StreamReader streamReader = new StreamReader(file);
                string c = "";
                string value = "";
                Scanner.Token temptoken = new Scanner.Token { };
                Scanner.TokenType temptype;
                bool isvalid=true;
                bool beforecomma = true;//this will be used to handle comments with \n
                while (!streamReader.EndOfStream)
                {
                    c += (char)streamReader.Read();
                    if (beforecomma &&(char)streamReader.Peek() == ',')
                    {
                        value = c;
                        streamReader.Read();//skip comma
                        c = "";
                        beforecomma = false;   
                    }
                    if ( !beforecomma && (streamReader.Peek() == 10 || streamReader.Peek() == 13 || (char)streamReader.Peek() == '\t' || streamReader.Peek()== -1 ))
                        {               
                        isvalid=Enum.TryParse<Scanner.TokenType>(c, out temptype);
                        beforecomma = true;
                        if (isvalid)
                        {
                            temptoken.val = value;
                            temptoken.t = temptype;
                            Scanner.tokens.Add(temptoken);
                            while(streamReader.Peek() == 10 || streamReader.Peek() == 13 || (char)streamReader.Peek() == '\t' || streamReader.Peek() == 32 /*spaces*/)
                                streamReader.Read();
                            c = "";
                            value = "";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (!isvalid)
                {
                    Scanner.tokens.Clear();
                    streamReader.Close();
                    MessageBox.Show("Error in token file", "Warning");
                    return;
                }
                streamReader.Close();
                if (Scanner.tokens.Count == 0)
                {
                    MessageBox.Show("Token file is empty", "Warning");
                    return;
                }    
                
            }
            catch (Exception x)
            {

            }
        }

        private void inputFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void inputFile_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            NotChange(e);
        }

        private void outputFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotChange(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (file == null) {
                MessageBox.Show("Please enter input file","Warning");
                return;
            }
            graphVizString = "digraph G { \ngraph[ordering=out];\n";
            if (!tokenFlag) {
                Scanner.getTokens(file);
            }
            if (Scanner.empty == true)
            {
                MessageBox.Show("The entered file is empty", "Alert");
                Scanner.empty = false;
                return;
            }
            if(Scanner.error_flag == true)
            {
                MessageBox.Show("Scanner error, cannot parse, check your syntax!", "Warning");
                Scanner.error_flag = false;
                Scanner.tokens.Clear();
                return;
            }
            Parser d = new Parser();
            Node root = d.parse();
            if (Parser.error == true)
            {
                MessageBox.Show("Please check your code", "Parsing erorr",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Parser.error = false;
                return;
            }
            draw(root);

            if(!tokenFlag)
            {
                Scanner.tokens.Clear();
            }
                     
            //graphviz usage
            try
            {        
                System.Drawing.Image image = Graphviz.RenderImage(graphVizString, "dot", "png");                
                string output_path = Path.Combine(inputPath, input_file_name + "_Tree.png");
                FileStream file1 = new FileStream(output_path, FileMode.Create);
                image.Save(file1, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Accepted","INFO");
                outputFile.Text = null;
                file1.Close();
                label1.Text = "The syntax tree has been created in the same path of the input file";
                Form2 f = new Form2();
                f.pictureBox1.Image= image;
                f.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;              
                f.Show();               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Browse_Click(object sender, EventArgs e)
        {
            try
            {               
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                ofd.Filter = "TXT files (*.txt)|*.txt"; /* chose type of files*/
                DialogResult dialogResult = ofd.ShowDialog();
                if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
                tokenFlag = false;
                inputFile.Text = File.ReadAllText(ofd.FileName);
                file = ofd.FileName;
                input_file_name = System.IO.Path.GetFileNameWithoutExtension(file);
                inputPath = System.IO.Path.GetDirectoryName(file);
            }
            catch (Exception x)
            {

            }
        }
    }
}