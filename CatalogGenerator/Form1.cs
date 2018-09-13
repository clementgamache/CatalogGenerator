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

namespace CatalogGenerator
{
    public partial class Form1 : Form
    {
        private Catalog cat;
        public Form1()
        {
            InitializeComponent();
        }

        private void testEntries()
        {
            double w, h, m;
            w = h = m = 0;
            bool noError = double.TryParse(textBoxWidth.Text, out w) &&
                double.TryParse(textBoxHeight.Text, out h) &&
                double.TryParse(textBoxMoulding.Text, out m);


            if (folderBrowserDialog1.SelectedPath.ToString().Length < 1) throw (new Exception("No folder chosen"));
            if (!noError) throw (new Exception("Bad numbers"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int marg = 30;
                printDocument1.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(
                    marg,marg,marg,marg);
                printDocument1.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                testEntries();
                double w, h, m;
                w = h = m = 0;
                bool noError = double.TryParse(textBoxWidth.Text, out w) &&
                    double.TryParse(textBoxHeight.Text, out h) &&
                    double.TryParse(textBoxMoulding.Text, out m);
                
                cat = new Catalog(folderBrowserDialog1.SelectedPath.Split('\\').Last(), folderBrowserDialog1.SelectedPath, w, h, m);
                
                printDocument1.Print();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


}

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            cat.print(e);

        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            label2.Text = folderBrowserDialog1.SelectedPath.ToString();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void checkBoxSquares_CheckedChanged(object sender, EventArgs e)
        {
            textBoxHeight.Enabled = textBoxWidth.Enabled = textBoxMoulding.Enabled = !checkBoxSquares.Checked;
            Frame.makeEveryImageSquare = checkBoxSquares.Checked;
        }
    }
}
