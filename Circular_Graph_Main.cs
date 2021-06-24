using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainFile.Library;
namespace MainFile
{
    
    public partial class Form1 : Form
    {
        Graphics g;
        Pen p;
        int Bitflag = 0;
        double centerpointx;
        double centerpointy;
        int totalNode;
        int[,] Adj_Metrix;
        public Form1()
        {
            
            InitializeComponent();
            panel2.Visible = false;
            panel3.Visible = false;


        }
        public void initialize_adjacent_metrix()
        {
            Adj_Metrix = new int[Convert.ToInt32(node_total.Text), Convert.ToInt32(node_total.Text)];
            for (int i = 0; i < Convert.ToInt32(node_total.Text); i++)
            {

                for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
                {
                    Adj_Metrix[i, j] = 0;

                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (node_total.Text != "" && density.Text != "")
            {
                panel3.Visible = false;
                initialize_adjacent_metrix();
                panel2.Visible = false;
                Bitflag = 1;
                panel2.Visible = true;
            }

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        { 
        
        }
        public Points getPoint(double angle,double r)
        {
            double X = centerpointx + (r * Math.Cos(angle));
            double Y = centerpointy + (r * Math.Sin(angle));
            return new Points(Convert.ToSingle(X),Convert.ToSingle(Y));
        }
        public Points[] getAllPoint()
        {
            Points[] ps = new Points[Convert.ToInt32(node_total.Text)];
            int start = 0;
            int end = 360;
            double res = Convert.ToDouble(node_total.Text) / totalNode;
            int totalcircle = Convert.ToInt32(Math.Ceiling(res));
            int step = (end / totalNode);
            int counter = 0;
            int r = 150;
            double res_rad = (r/totalcircle);
            int rad_step = Convert.ToInt32(Math.Floor(res_rad));
            while (totalcircle != 0 && counter != Convert.ToInt32(node_total.Text))
            {

               ps[counter++] = getPoint(start,r);
               start = start + step;
               if (start >= 360)
               {
                   totalcircle = totalcircle - 1;
                   start = 0;
                   r = r - rad_step;
               }
               
            }
            return ps;
        }
        public void ShowNode(Graphics g,Points[] ps)
        {
            int size_x = 5;
            int size_y = 5;
            int offsetx = 2;
            int offsety = 2;
            for (int i = 0; i < ps.Length; i++)
            {                
               g.FillEllipse(Brushes.Black, new RectangleF(ps[i].getx() - offsetx, ps[i].gety() - offsety, size_x, size_y));
            }
        }
        public int getNo_Edges()
        {
            int edges = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(density.Text) * Convert.ToInt32(node_total.Text) * (Convert.ToInt32(node_total.Text) - 1)) / 2) - Convert.ToInt32(node_total.Text)/2);
            return edges;
        }
        public int[,] BuildAdjacent_Metrix(Points[] ps)
        {
            int[,] aa = new int[2, 2];
            int total_edges =  getNo_Edges();
               // self.edges = math.ceil((self.density * self.nodes * (self.nodes - 1))/2)
           // Math.ran
            Random rnd = new Random();
            for(int i =0;i<total_edges;i++)
            {
                int p1 = rnd.Next(Convert.ToInt32(node_total.Text));
                int p2 = rnd.Next(Convert.ToInt32(node_total.Text));
                Adj_Metrix[p1,p2]  = 1;
                Adj_Metrix[p2, p1] = 1;
            }
          //  Adj_Metrix

            return Adj_Metrix;
        }
        public void showMetrix(int[,] ajj_metrix)
        {
            String str_adj = "";
            for (int i = 0; i < Convert.ToInt32(node_total.Text); i++)
            {
                str_adj = str_adj + "[" + (i) + "]";
                for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
                {
                    if (ajj_metrix[i, j] != 0)
                    {
                        str_adj = str_adj +  " -> " + (j);
                    }
                }
                str_adj = str_adj + "\n";
             //   int p1 = rnd.Next(Convert.ToInt32(node_total.Text));
            //    int p2 = rnd.Next(Convert.ToInt32(node_total.Text));
               // Adj_Metrix[p1, p2] = 1;
///Adj_Metrix[p2, p1] = 1;
            }
            showadjacent.Text = str_adj;
        }
        public void ShowEdges(Points[] ps, int[,] ajj_metrix, Graphics g)
        {
            for (int i = 0; i < Convert.ToInt32(node_total.Text); i++)
            {
                for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
                {
                    if (ajj_metrix[i, j] != 0)
                    {
                        Pen pen = new Pen(Color.Red);
                        g.DrawLine(pen, new PointF(ps[i].getx(), ps[i].gety()), new PointF(ps[j].getx(), ps[j].gety())); 

                    }
                }
              
            }
        }
        public void getNodeDegree(Points[] ps, int[,] ajj_metrix, Graphics g,int node_number)
        {
            int counter = 0;
            for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
            {
                if (ajj_metrix[node_number, j] != 0)
                {

                    //  Pen pen = new Pen(Color.Red);
                    // g.DrawLine(pen, new PointF(ps[node_number].getx(), ps[i].gety()), new PointF(ps[j].getx(), ps[j].gety()));
                    counter = counter + 1;
                }
            }
               g.FillEllipse(Brushes.Green, new RectangleF(ps[node_number].getx() - 2, ps[node_number].gety() - 2, 10, 10));
               label3.Text = "Total Number of Node : " + counter.ToString();
           
        }
        public void getMaxNodeDegree(Points[] ps, int[,] ajj_metrix, Graphics g)
        {
            ////////
            int Max_find = 0;
            int counter = 0;
            int node_num = 0;
            for (int i = 0; i < Convert.ToInt32(node_total.Text); i++)
            {
                for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
                {
                    if (ajj_metrix[i, j] != 0)
                    {
                        // if(Max_find>
                        //  Pen pen = new Pen(Color.Red);
                        // g.DrawLine(pen, new PointF(ps[node_number].getx(), ps[i].gety()), new PointF(ps[j].getx(), ps[j].gety()));
                        counter = counter + 1;
                    }
                }
                if (Max_find < counter)
                {
                    Max_find = counter;
                    node_num = i;
                }
                counter = 0;
            }
            g.FillEllipse(Brushes.Green, new RectangleF(ps[node_num].getx() - 2, ps[node_num].gety() - 2, 10, 10));
            label3.Text = " Node Number :" + node_num + " Total Number of Node : " + Max_find.ToString();
            
            //////////
            
        }
        public void getMinNodeDegree(Points[] ps, int[,] ajj_metrix, Graphics g)
        {
            ////////
            int Min_find = 9999;
            int counter = 0;
            int node_num = 0;
            for (int i = 0; i < Convert.ToInt32(node_total.Text); i++)
            {
                for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
                {
                    if (ajj_metrix[i, j] != 0)
                    {
                        // if(Max_find>
                        //  Pen pen = new Pen(Color.Red);
                        // g.DrawLine(pen, new PointF(ps[node_number].getx(), ps[i].gety()), new PointF(ps[j].getx(), ps[j].gety()));
                        counter = counter + 1;
                    }
                }
                if (Min_find > counter)
                {
                    Min_find = counter;
                    node_num = i;
                }
                counter = 0;
            }
            g.FillEllipse(Brushes.Green, new RectangleF(ps[node_num].getx() - 2, ps[node_num].gety() - 2, 10, 10));
            label3.Text = " Node Number :" + node_num + " Total Number of Node : " + Min_find.ToString();

            //////////

        }
        public void FindAllNeigbourNodeDegree(Points[] ps, int[,] ajj_metrix, Graphics g,int node_num)
        {
            ////////
            int Min_find = 9999;
           // int counter = 0;
            g.FillEllipse(Brushes.Red, new RectangleF(ps[node_num].getx() - 2, ps[node_num].gety() - 2, 10, 10));
                for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
                {
                    if (ajj_metrix[node_num, j] != 0)
                    {
                        // if(Max_find>
                        //  Pen pen = new Pen(Color.Red);
                        // g.DrawLine(pen, new PointF(ps[node_number].getx(), ps[i].gety()), new PointF(ps[j].getx(), ps[j].gety()));
                        //counter = counter + 1;
                        Pen pen = new Pen(Color.Green);
                        g.DrawLine(pen, new PointF(ps[node_num].getx(), ps[node_num].gety()), new PointF(ps[j].getx(), ps[j].gety())); 
                    }
                }
               
            
           // g.FillEllipse(Brushes.Green, new RectangleF(ps[node_num].getx() - 2, ps[node_num].gety() - 2, 10, 10));
           // label3.Text = " Node Number :" + node_num + " Total Number of Node : " + Min_find.ToString();

            //////////

        }

        public void FindCommonNode(Points[] ps, int[,] ajj_metrix, Graphics g,int node1,int node2)
        {
            ////////
            int Min_find = 9999;
            int counter = 0;
          //  int node_num = 0;
            g.FillEllipse(Brushes.Red, new RectangleF(ps[node1].getx() - 2, ps[node1].gety() - 2, 10, 10));
            g.FillEllipse(Brushes.Red, new RectangleF(ps[node2].getx() - 2, ps[node2].gety() - 2, 10, 10));

            for (int j = 0; j < Convert.ToInt32(node_total.Text); j++)
            {
                if (ajj_metrix[node1, j] == 1 && ajj_metrix[node2, j] == 1)
                {
                    // if(Max_find>
                    //  Pen pen = new Pen(Color.Red);
                    // g.DrawLine(pen, new PointF(ps[node_number].getx(), ps[i].gety()), new PointF(ps[j].getx(), ps[j].gety()));
                    counter = counter + 1;
                    Pen pen = new Pen(Color.Green);
                    g.DrawLine(pen, new PointF(ps[node1].getx(), ps[node1].gety()), new PointF(ps[j].getx(), ps[j].gety()));
                    g.DrawLine(pen, new PointF(ps[node2].getx(), ps[node2].gety()), new PointF(ps[j].getx(), ps[j].gety())); 

                }
            }
           // g.FillEllipse(Brushes.Green, new RectangleF(ps[node_num].getx() - 2, ps[node_num].gety() - 2, 10, 10));
            if (counter != 0)
            {
                label3.Text = " Node Number :" + node1 + "," + node2 + " Total Number of Node : " + counter;
            }
            else {
                label3.Text = "No Common Node (: ";
            }
            //////////

        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            //  Pen pen = new Pen(Color.Red);
           // Points 
            Points[] ps = getAllPoint();
            if (Bitflag == 1)
            {
                ShowNode(g, ps);
            }
            else if (Bitflag == 2)
            {
               ShowNode(g, ps);
               int[,] adj_metrix = BuildAdjacent_Metrix(ps);
               showMetrix(adj_metrix);
               ShowEdges(ps,adj_metrix,g);
            
            }
            else if (Bitflag == 3)
            {
                if (test_node.Text != "")
                {
                    ShowNode(g, ps);

                    getNodeDegree(ps, Adj_Metrix, g, Convert.ToInt32(test_node.Text));
                    showMetrix(Adj_Metrix);
                }
                //ShowEdges(ps, Adj_Metrix, g);
            }
            else if (Bitflag == 4)
            {
                ShowNode(g, ps);
                getMaxNodeDegree(ps, Adj_Metrix, g);
                showMetrix(Adj_Metrix);
               // ShowEdges(ps, Adj_Metrix, g);
            }
            else if (Bitflag == 5)
            {
                ShowNode(g, ps);
                getMinNodeDegree(ps, Adj_Metrix, g);
                showMetrix(Adj_Metrix);
               // ShowEdges(ps, Adj_Metrix, g);
            }
            else if (Bitflag == 6)
            {
                if (test_node.Text != "")
                {
                    ShowNode(g, ps);
                    FindAllNeigbourNodeDegree(ps, Adj_Metrix, g, Convert.ToInt32(test_node.Text));
                    showMetrix(Adj_Metrix);
                }
               // ShowEdges(ps, Adj_Metrix, g);
            }
            else if (Bitflag == 7)
            {
                if (node1.Text != "" && node2.Text != "")
                {
                    ShowNode(g, ps);
                    FindCommonNode(ps, Adj_Metrix, g, Convert.ToInt32(node1.Text), Convert.ToInt32(node2.Text));
                    showMetrix(Adj_Metrix);
                }
                // ShowEdges(ps, Adj_Metrix, g);
            }

           //  float xx = -159.175171f;
           // float yy = 164.147f;
          //  g.FillEllipse(Brushes.Black, new RectangleF(ps[0].getx() - 2, ps[0].gety() - 2, 5, 5));
           // ShowNode(g,ps);
           /* Points[] ps = new Points[10];
            ps[0] = new Points(30, 100);
            ps[1] = new Points(40, 50);
            float X = ps[0].getx();
            float y = ps[0].gety() ;
            int size_x = 5;
            int size_y = 5;
            int offsetx = 2;
            int offsety = 2;
            float X1 = ps[1].getx();
            float y1 = ps[1].gety(); 
            int offsetx1 = 2;
            int offsety1 = 2;
            if (Bitflag == 1)
            {
                //// for node creation ////
              

                g.FillEllipse(Brushes.Black, new RectangleF(X - offsetx, y - offsety, size_x, size_y));
                g.FillEllipse(Brushes.Black, new RectangleF(X1 - 2, y1 - 5, size_x, size_y));
                //  Graphics g = e.Graphics;
             //   Pen pen = new Pen(Color.Red);
              //  g.DrawLine(pen, new Point(X, y), new Point(X1, y1)); 
            }
            else
            {
                g.FillEllipse(Brushes.Black, new RectangleF(X - offsetx, y - offsety, size_x, size_y));
                g.FillEllipse(Brushes.Black, new RectangleF(X1 - 2, y1 - 5, size_x, size_y));
                //  Graphics g = e.Graphics;
                Pen pen = new Pen(Color.Red);
                g.DrawLine(pen, new PointF(X, y), new PointF(X1, y1)); 
            }
            */

           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (node_total.Text != "" && density.Text != "")
            {
                panel3.Visible = true;
                initialize_adjacent_metrix();
                panel2.Visible = false;
                Bitflag = 2;
                panel2.Visible = true;
            }
            //panel2.Invalidate();  // this triggers the Paint event!
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            centerpointx = panel2.Size.Width/2;
            centerpointy = panel2.Size.Height/2;
            totalNode = 40;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;

            Bitflag = 3;  // Show Node Degree

            panel2.Visible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;

            Bitflag = 4;  // Show Node Degree

            panel2.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;

            Bitflag = 5;  // Show Node Degree

            panel2.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;

            Bitflag = 6;  // Show Node Degree

            panel2.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;

            Bitflag = 7;  // Show Node Degree

            panel2.Visible = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

       
    }
    
}
