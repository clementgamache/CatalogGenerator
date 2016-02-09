using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CatalogGenerator
{
    class Catalog
    {
        private List<Tuple<Frame, Frame>> doubles = new List<Tuple<Frame, Frame>>();
        private string title;
        private int pageIndex;
        private bool no4;
        private const int headerHeight = 60;
        private const int distanceBetweenElements = 25;
        private const int tagHeight = 30;
        private System.Drawing.Font headerFont = new System.Drawing.Font("Arial", 16);
        private List<List<double>> layouts;
        public Catalog(string catTitle, string path, double width, double height, double moulding)
        {
            pageIndex = 0;
            no4 = Math.Max(width, height) / Math.Min(width, height) > 2;
            layouts = new List<List<double>>();
            layouts.Add(new List<double>(new double[] { 0.5, 0.5 }));
            layouts.Add(new List<double>(new double[] { 0.36, 0.28, 0.36 }));
            layouts.Add(new List<double>(new double[] { 0.4, 0.3, 0.3 }));
            layouts.Add(new List<double>(new double[] { 0.36, 0.36, 0.28 }));
            layouts.Add(new List<double>(new double[] { 0.3, 0.3, 0.4 }));
            layouts.Add(new List<double>(new double[] { 0.28, 0.36, 0.36 }));
            layouts.Add(new List<double>(new double[] { 0.3, 0.4, 0.3 }));
            layouts.Add(new List<double>(new double[] { 1 }));
            title = catTitle;
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            DirectoryInfo d = new DirectoryInfo(path);
            List<FileInfo> allPaths = new List<FileInfo>();
            foreach (FileInfo f in d.GetFiles())
            {
                if (ImageExtensions.Contains(Path.GetExtension(f.FullName).ToUpperInvariant()))
                        allPaths.Add(f);
            }
            if (allPaths.Count % 2 != 0) throw new Exception("Error: number of pictures should be an even number");
            for (int i = 0; i < allPaths.Count; i += 2)
            {

                    Frame frame1 = new Frame(allPaths[i].FullName, width, height, moulding);
                    Frame frame2 = new Frame(allPaths[i+1].FullName, width, height, moulding);
                    if (frame1.image.Height / frame1.image.Width >= 1 != frame2.image.Height / frame2.image.Width >= 1)
                        throw new Exception("Error: pair #" + (i/2).ToString() + " is composed of different ratios");

                    doubles.Add(new Tuple<Frame, Frame>(frame1, frame2));
            }
        }
        public void print(System.Drawing.Printing.PrintPageEventArgs e)
        {
            System.Drawing.Bitmap background = Properties.Resources.Background;
            e.Graphics.DrawImage(background, 0, 0, e.PageBounds.Width, e.PageBounds.Height);
            
            printHeader(e);
            printPictures(e);
            printFooter(e);
            pageIndex++;
        }
        
        private void printHeader(System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.FillRectangle(
                System.Drawing.Brushes.Black,
                new System.Drawing.Rectangle(e.MarginBounds.Location, new System.Drawing.Size(e.MarginBounds.Right - e.MarginBounds.Left, headerHeight)));
            e.Graphics.DrawString("THE PRINT MINT", headerFont, System.Drawing.Brushes.White,
                e.MarginBounds.X + 10,
                e.MarginBounds.Top + headerHeight / 2 - headerFont.Height / 2);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            drawFormat.FormatFlags = System.Drawing.StringFormatFlags.DirectionRightToLeft;
            e.Graphics.DrawString(title, headerFont, System.Drawing.Brushes.White,
                e.MarginBounds.Right -10,
                e.MarginBounds.Top + headerHeight / 2 - headerFont.Height / 2, drawFormat);
        }

        
        private void printPictures(System.Drawing.Printing.PrintPageEventArgs e)
        {
            //defining page layout
            List<double> layout;
            if (!no4)
            {
                if (doubles.Count == 1) layout = layouts[layouts.Count - 1];
                else if (doubles.Count <= 3) layout = layouts[0];
                else if (doubles.Count == 4) layout = layouts[(pageIndex % 3) * 2 + 1];
                else layout = layouts[pageIndex % 7];
            }
            else
            {
                if (doubles.Count == 1) layout = layouts[layouts.Count - 1];
                else if (doubles.Count == 2) layout = layouts[0];
                else layout = layouts[pageIndex % 7];
            }

            int nDoubles = 0;
            if (no4) nDoubles = layout.Count;
            else
            {
                foreach (double proportion in layout)
                {
                    if (proportion == 0.28 || proportion == 0.3) nDoubles += 2;
                    else nDoubles++;
                }
            }
            List<Tuple<Frame, Frame>> usedTuples = new List<Tuple<Frame, Frame>>();
            for (int i = 0; i < nDoubles; i++)
            {
                Tuple<Frame, Frame> transfer = doubles[0];
                doubles.Remove(transfer);
                usedTuples.Add(transfer);
            }
            if (pageIndex == 2)
                pageIndex = 2;
            bool ok = correctOrder(usedTuples, layout);
            if (!ok)
            {
                for (int i = 0; i < usedTuples.Count; i++)
                {
                    for (int j = i+1; j < usedTuples.Count; j++)
                    {
                        
                        List<Tuple<Frame, Frame>> oneSwap = new List<Tuple<Frame, Frame>>();
                        oneSwap.AddRange(usedTuples);
                        Tuple<Frame, Frame> tmp = usedTuples[i];
                        oneSwap[i] = usedTuples[j];
                        oneSwap[j] = tmp;
                        if (correctOrder(oneSwap, layout))
                        {
                            ok = true;
                            usedTuples = oneSwap;
                            break;
                        }
                    }
                    if (ok) break;
                }
            }

            int yPosition = e.MarginBounds.Y +headerHeight+distanceBetweenElements;
            for (int i = 0; i < layout.Count; i++)
            {
                double proportion = layout[i];
                bool high = !(proportion == 0.28 || proportion == 0.3);
                int ySpace = (int)((double)(e.MarginBounds.Height - headerHeight -distanceBetweenElements) * proportion);
                int maxWidth, maxHeight;
                Tuple<Frame, Frame> x = usedTuples[0];
                bool vertical = x.Item1.image.Height >= x.Item1.image.Width;
                if ((no4 && !high ) || (high && !no4) || (no4 && high && vertical))//display only 2
                {
                    maxWidth = (e.MarginBounds.Width - distanceBetweenElements) / 2;
                    maxHeight = ySpace - distanceBetweenElements - tagHeight;
                    Tuple<Frame, Frame> t = usedTuples[0];
                    int newHeight = Math.Min(maxHeight, (int)((double)maxWidth * (double)(t.Item1.image.Height) / (double)(t.Item1.image.Width)));
                    t.Item1.setHeight(newHeight);
                    t.Item2.setHeight(newHeight);
                    int xFreeSpace = e.MarginBounds.Width - 2 * t.Item1.image.Width - distanceBetweenElements;
                    int xSpacing = xFreeSpace / 3;
                    int yFreeSpace = ySpace - t.Item1.image.Height - tagHeight - distanceBetweenElements;
                    int ySpacing = yFreeSpace / 2;

                    t.Item1.setLocation(e.MarginBounds.Left + xSpacing, yPosition + ySpacing);
                    t.Item2.setLocation(e.MarginBounds.Right - xSpacing - t.Item2.image.Width, yPosition + ySpacing);
                    t.Item1.print(e);
                    t.Item2.print(e);

                    usedTuples.Remove(t);

                }
                else if (no4 && high)
                {
                    maxWidth = e.MarginBounds.Width;
                    maxHeight = (ySpace - distanceBetweenElements*2 - tagHeight*2)/2 ;
                    Tuple<Frame, Frame> t = usedTuples[0];
                    int newHeight = Math.Min(maxHeight, (int)(maxWidth * (double)(t.Item1.image.Height) / (double)(t.Item1.image.Width)));
                    t.Item1.setHeight(newHeight);
                    t.Item2.setHeight(newHeight);
                    int yFreeSpace = ySpace - 2*t.Item1.image.Height - 2*tagHeight - 2*distanceBetweenElements;
                    int ySpacing = yFreeSpace / 3;
                    t.Item1.setLocation(e.PageBounds.Width / 2 - t.Item1.image.Width / 2, yPosition + ySpacing);
                    t.Item2.setLocation(e.PageBounds.Width / 2 - t.Item1.image.Width / 2, yPosition + 2*ySpacing + t.Item1.image.Height + distanceBetweenElements + tagHeight);
                    t.Item1.print(e);
                    t.Item2.print(e);
                    usedTuples.Remove(t);
                }
                else
                {
                    maxWidth = (e.MarginBounds.Width-3*distanceBetweenElements)/4;
                    maxHeight = ySpace - distanceBetweenElements - tagHeight;
                    
                    Tuple<Frame, Frame> t1 = usedTuples[0];
                    Tuple<Frame, Frame> t2 = usedTuples[1];
                    int newHeight = Math.Min(maxHeight, (int)(maxWidth * (double)(t1.Item1.image.Height) / (double)(t1.Item1.image.Width)));
                    t1.Item1.setHeight(newHeight);
                    t1.Item2.setHeight(newHeight);
                    t2.Item1.setHeight(newHeight);
                    t2.Item2.setHeight(newHeight);
                    int xFreeSpace = e.MarginBounds.Width - 4 * t1.Item1.image.Width - 3*distanceBetweenElements;
                    int xSpacing = xFreeSpace / 5;
                    int yFreeSpace = ySpace - t1.Item1.image.Height - tagHeight - distanceBetweenElements;
                    int ySpacing = yFreeSpace / 3;
                    t1.Item1.setLocation(e.MarginBounds.X + xSpacing, yPosition + ySpacing);
                    t1.Item2.setLocation(e.MarginBounds.X + t1.Item1.image.Width + distanceBetweenElements + 2 * xSpacing, yPosition + ySpacing);
                    t2.Item1.setLocation(e.MarginBounds.X + 2 * t1.Item1.image.Width + 2 * distanceBetweenElements + 3 * xSpacing, yPosition + ySpacing);
                    t2.Item2.setLocation(e.MarginBounds.X + 3 * t1.Item1.image.Width + 3 * distanceBetweenElements + 4 * xSpacing, yPosition + ySpacing);
                    t1.Item1.print(e);
                    t1.Item2.print(e);
                    t2.Item1.print(e);
                    t2.Item2.print(e);
                    usedTuples.Remove(t1);
                    usedTuples.Remove(t2);
                }
                yPosition = yPosition + ySpace;
            }
            int a = 0;
           
            if (doubles.Count > 0)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }
        private void printFooter(System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private bool correctOrder(List<Tuple<Frame, Frame>> list, List<double> layout)
        {
            if (no4) return true;
            int layoutId = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (layout[layoutId] == 0.28 || layout[layoutId] == 0.3)
                {
                    if ((list[i].Item1.image.Height > list[i].Item1.image.Width) !=
                        (list[i+1].Item1.image.Height > list[i+1].Item1.image.Width))
                    {
                        return false;
                    }
                    i++;
                }
                layoutId++;
            }
            return true;
}

        
    }
}
