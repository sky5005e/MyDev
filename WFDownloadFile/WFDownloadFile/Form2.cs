using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using LinqToExcel;

namespace WFDownloadFile
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {

            var FD = new System.Windows.Forms.OpenFileDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileToOpen = FD.FileName;
                label3.Text = fileToOpen;
                label3.Visible = true;
               // System.IO.FileInfo File = new System.IO.FileInfo(FD.FileName);

                //OR

               // System.IO.StreamReader reader = new System.IO.StreamReader(fileToOpen);
                //etc
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {


            var FD = new System.Windows.Forms.OpenFileDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileToOpen = FD.FileName;
                label4.Text = fileToOpen;
                label4.Visible = true;
                //System.IO.FileInfo File = new System.IO.FileInfo(FD.FileName);

                //OR

                //System.IO.StreamReader reader = new System.IO.StreamReader(fileToOpen);
                //etc
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var excel = new ExcelQueryFactory(label3.Text);
            //var MainExcelname = from m in excel.Worksheet<ExcelCls>()
            //           select m;
            //var rexcel = new ExcelQueryFactory(label4.Text);
            //var dbExcel = from r in rexcel.Worksheet<DBexcelCls>()
            //           select r;

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            label3.Visible = false;

            label4.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
