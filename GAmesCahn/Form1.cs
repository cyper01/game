using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GAmesCahn
{

    public partial class Form1 : Form
{
    Clues clues_window = new Clues();
    List<id_cells> idc = new List<id_cells>();
    public String puzzle_file = Application.StartupPath + @"\\Puzzles\\puzzle_1.pzl";



    public Form1()
    {
        builWordList();
        InitializeComponent();

    }

    private void fileToolStripMenuItem_Click(object sender, EventArgs e)
    {


    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    public void builWordList()
    {
        String line = "";
        using (StreamReader s = new StreamReader(puzzle_file)) 
        {
            line = s.ReadLine(); //igonres the first line
            while ((line = s.ReadLine()) != null)
            {
                String[] l = line.Split('|');
                idc.Add(new id_cells(Int32.Parse(l[0]), Int32.Parse(l[1]), l[2], l[3], l[4], l[5]  ));
                clues_window.clue_table.Rows.Add(new String[] { l[3], l[2], l[5],   });




            }



        }//end using


    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show("1.love" +
            "|2.closed" +
            "|3.eraser" +
            "|4.red" +
            "|5.dallas" +
            "|6.dare" +
            "|7.relapse" +
            "|8.cap", "Answers");
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {

        InitializeBoar();
        clues_window.SetDesktopLocation(this.Location.X + this.Width + 1, this.Location.Y);
        clues_window.StartPosition = FormStartPosition.Manual;
        clues_window.Show();
        clues_window.clue_table.AutoResizeColumns();

    }
    private void InitializeBoar()

    {
        board.BackgroundColor = Color.Black;
        board.DefaultCellStyle.BackColor = Color.Black;

        for (int i = 0; i < 22; i++)
            board.Rows.Add();

        //set width of colum
        foreach (DataGridViewTextBoxColumn c in board.Columns)
            c.Width = board.Width / board.Columns.Count;

        //set Heigth of row
        foreach (DataGridViewRow r in board.Rows)
            r.Height = board.Height / board.Rows.Count;

        //make all cell read only
        for (int row = 0; row < board.Rows.Count; row++)
        {
            for (int col = 0; col < board.Columns.Count; col++)
                board[col, row].ReadOnly = true;

        }


        foreach (id_cells i in idc)
            {
                int start_col = i.X; 
                int start_row = i.Y;
                char [] word = i.word.ToCharArray();

                for (int j = 0; j < word.Length; j++)
                {

                    if (i.direction.ToUpper() == "ACROSS")
                        formaCell(start_row , start_col + j, word[j].ToString());

                    if (i.direction.ToUpper() == "DOWN")
                        formatCell(start_row + j, start_col, word[j].ToString());
                    


                }

            }

    }

        private void formatCell(int row, int col, String letter)
        {
            DataGridViewCell c = board[col, row];
            c.Style.BackColor = Color.White;
            c.ReadOnly = false;
            c.Style.SelectionBackColor = Color.Cyan;
            c.Tag = letter;

        }

        private void formaCell(int row, int col, String letter)
    {
        DataGridViewCell c = board[col, row];
        c.Style.BackColor = Color.White;
        c.ReadOnly = false;
        c.Style.SelectionBackColor = Color.Cyan;
        c.Tag = letter;
    }


    private void Form1_LocationChanged(object sender, EventArgs e)
    {
        clues_window.SetDesktopLocation(this.Location.X + this.Width + 1, this.Location.Y);

    }

        private void board_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //make letter uppercase
            try
            {
               board[e.ColumnIndex, e.RowIndex].Value = board[e.ColumnIndex, e.RowIndex].Value.ToString().ToLower();

            }
            catch { }

            //truncate to one letter
            try
            {
               if (board[e.ColumnIndex, e.RowIndex].Value.ToString().Length> 1)
                   board[e.ColumnIndex, e.RowIndex].Value = board[e.ColumnIndex, e.RowIndex].Value.ToString().Substring(0,1);

            }
            catch { }

            //format color if correct
            try
            {
                if (board[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper().Equals(board[e.ColumnIndex, e.RowIndex].Tag.ToString().ToUpper()))
                    board[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Green;
                else
                    board[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;


            }
            catch { }



        }

        private void exsitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"\\Puzzles|*pzl";
            if (ofd.ShowDialog().Equals(DialogResult.OK)) 
            {
                puzzle_file= ofd.FileName;

                board.Rows.Clear();
                clues_window.clue_table.Rows.Clear();
                idc.Clear();

                builWordList();
                InitializeBoar();
            
            
            }


        }

        private void board_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            String number = "";

            //foreach(item c in List of items)
            if (idc.Any(c => (number = c.number) != "" && c.X == e.ColumnIndex && c.Y == e.RowIndex))  
            {
                Rectangle r =new Rectangle(e.CellBounds.X,e.CellBounds.Y,e.CellBounds.Width,e.CellBounds.Height);  
                e.Graphics.FillRectangle(Brushes.White, r);
                Font f = new Font(e.CellStyle.Font.FontFamily, 7);
                e.Graphics.DrawString(number, f, Brushes.Black, r);
                e.PaintContent(e.ClipBounds);
                e.Handled= true;


            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программу разработал для квалификационного экзамена студент группы ИС-32 Внуков Александр Александрович");



        }
    }
    public class id_cells
{
    public int X;
    public int Y;
    public String direction;
    public String number;
    public String word;
    public String clue;

    public id_cells(int x, int y, String d, String n, String w, String c)
    {
        this.X = x;
        this.Y = y;
        this.direction = d;
        this.number = n;
        this.word = w;
        this.clue = c;

    }



}//end calss
}