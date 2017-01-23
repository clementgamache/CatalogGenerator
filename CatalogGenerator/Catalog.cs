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
        private List<Tuple<string, List<Tuple<Frame, Frame>>>> doubles = new List<Tuple<string, List<Tuple<Frame, Frame>>>>();
        private string title;
        private int pageIndex;
        private bool no4;
        
        public static int vertical3 = 0;
        public static int horizontal3 = 0;
        private static int horizontal4 = 0;
        private static int vertical2 = 0;
        private static int normal4 = 0;
        private static int normal5 = 0;

        public Catalog(string catTitle, string path, double width, double height, double moulding)
        {
            pageIndex = 0;
            no4 = Math.Max(width, height) / Math.Min(width, height) > 2;
            

            title = catTitle;
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            DirectoryInfo d = new DirectoryInfo(path);
            foreach (DirectoryInfo dir in d.GetDirectories())
            {
                List<FileInfo> allPaths = new List<FileInfo>();
                foreach (FileInfo f in dir.GetFiles())
                {
                    if (ImageExtensions.Contains(Path.GetExtension(f.FullName).ToUpperInvariant()))
                        allPaths.Add(f);
                }
                if (allPaths.Count % 2 != 0) throw new Exception("Error: number of pictures should be an even number");

                doubles.Add(new Tuple<string, List<Tuple<Frame, Frame>>>(dir.Name, new List<Tuple<Frame, Frame>>()));
                for (int i = 0; i < allPaths.Count; i += 2)
                {

                    Frame frame1 = new Frame(allPaths[i].FullName, width, height, moulding);
                    Frame frame2 = new Frame(allPaths[i + 1].FullName, width, height, moulding);
                    if (frame1.image.Height / frame1.image.Width >= 1 != frame2.image.Height / frame2.image.Width >= 1)
                        throw new Exception("Error: pair #" + (i / 2).ToString() + " is composed of different ratios \n" + allPaths[i].FullName + "\n" + allPaths[i+1].FullName);

                    doubles.Last().Item2.Add(new Tuple<Frame, Frame>(frame1, frame2));
                }
            }
        }
        public void print(System.Drawing.Printing.PrintPageEventArgs e)
        {
            //System.Drawing.Bitmap background = Properties.Resources.Background;
            //e.Graphics.DrawImage(background, 0, 0, e.PageBounds.Width, e.PageBounds.Height);
            
            printHeader(e);
            printPictures(e);
            printFooter(e);
            pageIndex++;
        }
        
        
        private int getBestDisposition(Frame f, int maxWidth, int maxHeight)
        {
            double ratio = (double)f.image.Height / f.image.Width;
            //disposed horizontally
            int width = (maxWidth - CatalogProperties.distanceBetweenElementsX) / 2;
            int height = maxHeight - CatalogProperties.distanceBetweenElementsY - CatalogProperties.tagHeight;
            height = Math.Min(height, (int)(width * ratio));
            width = (int)(height / ratio);
            int air1 = height * width;
            //disposed horizontally
            width = maxWidth;
            height = (maxHeight - 2 * CatalogProperties.distanceBetweenElementsY - 2 * CatalogProperties.tagHeight) / 2;
            height = Math.Min(height, (int)(width * ratio));
            width = (int)(height / ratio);
            int air2 = height * width;

            return air1 > air2 ? 2 : 1;
        }

        private List<int> getNextLayout(List<Tuple<Frame, Frame>> dbls, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // 1 -> = ; 2 -> || ; 3-> || with more space ; 4 -> |||| ; 
            if (dbls.Count == 1)
            {
                int allowedSpaceY = e.MarginBounds.Height - CatalogProperties.headerHeight - CatalogProperties.footerHeight - CatalogProperties.distanceBetweenElementsY;
                int allowedSpaceX = e.MarginBounds.Width;
                int disposition = getBestDisposition(dbls[0].Item1, allowedSpaceX, allowedSpaceY);
                return new List<int>(new int[] { disposition });
            }
            else if (dbls.Count == 2)
            {
                int allowedSpaceY = (e.MarginBounds.Height - CatalogProperties.headerHeight - CatalogProperties.footerHeight - CatalogProperties.distanceBetweenElementsY)/2;
                int allowedSpaceX = e.MarginBounds.Width;
                Tuple<Frame, Frame> t1 = dbls[0];
                Tuple<Frame, Frame> t2 = dbls[1];
                //if disposed horizontally
                int disposition1 = getBestDisposition(dbls[0].Item1, allowedSpaceX, allowedSpaceY);
                int disposition2 = getBestDisposition(dbls[1].Item1, allowedSpaceX, allowedSpaceY);
                return new List<int>(new int[] { disposition1, disposition2 });
            }
            else if (dbls.Count == 3)
            {
                if (no4)
                {
                    List<bool> verticals = new List<bool>();
                    foreach (Tuple<Frame, Frame> t in dbls)
                    {
                        bool vertical = t.Item1.image.Height > t.Item1.image.Width;
                        verticals.Add(vertical);
                    }
                    if (verticals.Count(p => p) == 3)
                    {
                        int d = vertical3 % 2;
                        vertical3++;
                        return new List<int>(new int[] { 3 + d, 4 - d });
                    }
                    else if (verticals.Count(p => !p) == 3)
                    {
                        List<List<int>> layouts = new List<List<int>>();
                        layouts.Add(new List<int>(new int[] { 2, 1, 2 }));
                        layouts.Add(new List<int>(new int[] { 1, 2, 2 }));
                        layouts.Add(new List<int>(new int[] { 2, 2, 1 }));
                        int d = horizontal3 % layouts.Count;
                        horizontal3++;
                        return layouts[d];
                    }
                    else if (verticals.Count(p => p) == 2)
                    {
                        int idx = verticals.IndexOf(false);
                        if (idx == 1)
                        {
                            Tuple<Frame, Frame> tmp = dbls[1];
                            int d = vertical2 % 2;
                            dbls[1] = dbls[d * 2];
                            dbls[d * 2] = tmp;
                            vertical2++;
                            return new List<int>(new int[] { 1 + 3 * d, 4 - 3 * d });
                        }
                        else if (idx == 0)
                        {
                            return new List<int>(new int[] { 1, 4 });
                        }
                        else// if (idx == 2)
                        {
                            return new List<int>(new int[] { 4, 1 });
                        }
                    }
                    else //if (verticals.Count(p => p) == 1)
                    {
                        int idx = verticals.IndexOf(true);
                        List<int> ret = new List<int>();
                        for (int i = 0; i < 3; i++)
                        {
                            ret.Add(i == idx ? 3 : 2);
                        }
                        return ret;
                    }
                }
                else //!no4
                {
                    return new List<int>(new int[] { 2, 2, 2 });
                }
            }
            else if (dbls.Count == 4)
            {
                if (no4) 
                {
                    return new List<int>(new int[] { 2, 2, 2, 2 });
                }
                else //!no4
                {
                    List<List<int>> layouts = new List<List<int>>();
                    layouts.Add(new List<int>(new int[] { 2, 4, 2 }));
                    layouts.Add(new List<int>(new int[] { 4, 2, 2 }));
                    layouts.Add(new List<int>(new int[] { 2, 2, 4 }));
                    int d = normal4 % layouts.Count;
                    normal4++;
                    return layouts[d];
                }
            }
            else //count == 5
            {
                List<List<int>> layouts = new List<List<int>>();
                layouts.Add(new List<int>(new int[] { 4, 4, 2 }));
                layouts.Add(new List<int>(new int[] { 2, 4, 4 }));
                layouts.Add(new List<int>(new int[] { 4, 2, 4 }));
                int d = normal5 % layouts.Count;
                normal5++;
                return layouts[d];
            }
        }

        private bool isHorizontal(Tuple<Frame, Frame> f)
        {
            return f.Item1.isHorizontal();
        }

        private void printHeader(System.Drawing.Printing.PrintPageEventArgs e)
        {
            int littleSpace = 3;
            System.Drawing.Color blue = System.Drawing.Color.FromArgb(21, 101, 112);
            System.Drawing.Color green = System.Drawing.Color.FromArgb(106, 127, 16);
            System.Drawing.SolidBrush customBrush = new System.Drawing.SolidBrush(blue);

            int firstWidth = e.MarginBounds.Width / 3;
            e.Graphics.FillRectangle(
                customBrush,
                new System.Drawing.Rectangle(e.MarginBounds.Location, new System.Drawing.Size(firstWidth - littleSpace, CatalogProperties.headerHeight)));

            customBrush = new System.Drawing.SolidBrush(green);
            e.Graphics.FillRectangle(
                customBrush,
                new System.Drawing.Rectangle(e.MarginBounds.Left + firstWidth, e.MarginBounds.Top, e.MarginBounds.Width - firstWidth, CatalogProperties.headerHeight));

            e.Graphics.DrawString("THE PRINT MINT", CatalogProperties.headerFont, System.Drawing.Brushes.White,
                e.MarginBounds.X + 10,
                e.MarginBounds.Top + CatalogProperties.headerHeight / 2 - CatalogProperties.headerFont.Height / 2);
            //System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            var drawFormat = new System.Drawing.StringFormat() { Alignment = System.Drawing.StringAlignment.Far };
            //format.FormatFlags = System.Drawing.StringFormatFlags.DirectionRightToLeft;
            string topRight = title + " - " + doubles[0].Item1;
            e.Graphics.DrawString(topRight, CatalogProperties.titleFont, System.Drawing.Brushes.White,
                new System.Drawing.RectangleF(e.MarginBounds.Left, e.MarginBounds.Top + CatalogProperties.headerHeight / 2 - CatalogProperties.headerFont.Height / 2, e.MarginBounds.Width - 10, 1000), drawFormat);
            //e.MarginBounds.Top + headerHeight / 2 - headerFont.Height / 2, drawFormat);

        }
        private void printPictures(System.Drawing.Printing.PrintPageEventArgs e)
        {
            //defining page layout
            List<Tuple<Frame, Frame>> genre = doubles[0].Item2;
            List<Tuple<Frame, Frame>> usedTuples = getUsedTuples(ref genre);
            List<int> layout = getNextLayout(usedTuples, e);

            //correct the order if it has a conflict of ratio
            correctRatioConflicts(ref usedTuples, layout);
            
            setImagesSize(usedTuples, layout, e);
            
            //mainPrint
            printLayout(layout, usedTuples, e);
            
            if (genre.Count > 0)
            {
                e.HasMorePages = true;
            }
            else
            {
                doubles.RemoveAt(0);
                e.HasMorePages = doubles.Count > 0;
            }
        }
        private void printFooter(System.Drawing.Printing.PrintPageEventArgs e)
        {
            int littleSpace = 3;
            System.Drawing.Color yellow = System.Drawing.Color.FromArgb(243, 211, 17);
            System.Drawing.Color red = System.Drawing.Color.FromArgb(213, 43, 30);
            System.Drawing.SolidBrush customBrush = new System.Drawing.SolidBrush(yellow);

            int firstWidth = (int)(e.MarginBounds.Width * 0.61803398874);
            int posY = e.MarginBounds.Bottom - CatalogProperties.footerHeight;
            e.Graphics.FillRectangle(
                customBrush,
                new System.Drawing.Rectangle(e.MarginBounds.Left, posY, firstWidth - littleSpace, CatalogProperties.footerHeight));

            customBrush = new System.Drawing.SolidBrush(red);
            e.Graphics.FillRectangle(
                customBrush,
                new System.Drawing.Rectangle(e.MarginBounds.Left + firstWidth, posY, e.MarginBounds.Width - firstWidth, CatalogProperties.footerHeight));
            string date = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month) + " " + DateTime.Now.Year.ToString();
            e.Graphics.DrawString(date, CatalogProperties.footerFont, System.Drawing.Brushes.Black,
                e.MarginBounds.X + 10,
                posY + CatalogProperties.footerHeight /2 - CatalogProperties.footerFont.Height/2);

            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            drawFormat.FormatFlags = System.Drawing.StringFormatFlags.DirectionRightToLeft;
            e.Graphics.DrawString((pageIndex+1).ToString(), CatalogProperties.footerFont, System.Drawing.Brushes.White,
                e.MarginBounds.Right - 10,
                e.MarginBounds.Bottom - CatalogProperties.footerHeight / 2 - CatalogProperties.footerFont.Height / 2, drawFormat);
        }

        //two sets of pictures cannot have the same proportions
        private bool correctOrder(List<Tuple<Frame, Frame>> list, List<int> layout)
        {
            int layoutId = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (layout[layoutId] == 4)
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

        private List<Tuple<Frame, Frame>> getUsedTuples(ref List<Tuple<Frame, Frame>> genre)
        {
            int nDoubles = 0;
            if (genre.Count <= 3)
            {
                nDoubles = genre.Count;
            }
            else if (no4)
            {

                if (
                    isHorizontal(genre[0]) &&
                    isHorizontal(genre[1]) &&
                    isHorizontal(genre[2]) &&
                    isHorizontal(genre[3]))
                {
                    if (horizontal4 % 2 == 1)
                    {
                        nDoubles = 4;
                    }
                    else
                    {
                        nDoubles = 3;
                    }
                    horizontal4++;
                }

                else nDoubles = 3;
            }
            else //!no4
            {
                if (genre.Count <= 5) nDoubles = genre.Count;
                else
                    nDoubles = 5 - pageIndex % 2;
            }
            List<Tuple<Frame, Frame>> usedTuples = new List<Tuple<Frame, Frame>>();
            for (int i = 0; i < nDoubles; i++)
            {
                Tuple<Frame, Frame> transfer = genre[0];
                genre.Remove(transfer);
                usedTuples.Add(transfer);
            }
            return usedTuples;
        }
        
        private void correctRatioConflicts(ref List<Tuple<Frame, Frame>> usedTuples, List<int> layout)
        {
            bool ok = correctOrder(usedTuples, layout);
            while (!ok)
            {
                for (int i = 0; i < usedTuples.Count; i++)
                {
                    for (int j = i + 1; j < usedTuples.Count; j++)
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
        }
        private int getFirstRemainingSpace(List<int> layout, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int remainingSpace = e.MarginBounds.Height - CatalogProperties.headerHeight - CatalogProperties.distanceBetweenElementsY - CatalogProperties.footerHeight;
            foreach (int n in layout)
            {
                remainingSpace -= CatalogProperties.distanceBetweenElementsY + CatalogProperties.tagHeight;
                if (n == 1) remainingSpace -= CatalogProperties.distanceBetweenElementsY + CatalogProperties.tagHeight;
            }
            return remainingSpace;
        }
        private int getFirstRemainingElements(List<int> layout)
        {
            int remainingElements = layout.Count;
            foreach (int n in layout)
            {
                if (n == 1 || n == 3 || n == 4) remainingElements++;
            }
            return remainingElements;
        }

        private List<int> getHeightOfEachSection(List<Tuple<Frame, Frame>> usedTuples, List<int> layout, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int remainingSpace = getFirstRemainingSpace(layout, e);
            int remainingElements = getFirstRemainingElements(layout);
            List<Tuple<Frame, Frame>> copy = new List<Tuple<Frame, Frame>>();
            copy.AddRange(usedTuples);
            List<int> heights = new List<int>();
            for (int i = 0; i < layout.Count; i++)
            {
                heights.Add(0);
            }
            int block = 0;
            while (copy.Count > 0)
            {
                int copyId = 0;
                int savedCopyId = 0;
                int minHeightMax = 9999999;
                int savedId = 0;
                for (int i = 0; i < layout.Count; i++)
                {
                    block = layout[i];
                    if (heights[i] != 0)
                    {
                        continue;
                    }
                    int maxHeight = 0;
                    int maxWidth = 0;
                    double ratio = (double)copy[copyId].Item1.image.Height / copy[copyId].Item1.image.Width;
                    if (block == 1) //1 in the line
                    {
                        maxWidth = e.MarginBounds.Width;
                        maxHeight = remainingSpace / 2;
                    }
                    else if (block == 4) // 4 in the line
                    {
                        maxWidth = (e.MarginBounds.Width - 3 * CatalogProperties.distanceBetweenElementsX) / 4;
                        maxHeight = remainingSpace;
                    }
                    else //2 on the line
                    {
                        maxWidth = (e.MarginBounds.Width - CatalogProperties.distanceBetweenElementsX) / 2;
                        maxHeight = remainingSpace;
                    }


                    maxHeight = Math.Min((int)(maxWidth * ratio), maxHeight);
                    minHeightMax = Math.Min(minHeightMax, maxHeight);
                    if (minHeightMax == maxHeight)
                    {
                        savedId = i;
                        savedCopyId = copyId;
                    }

                    copyId++;
                    if (block == 4) copyId++;
                }
                int maxAllowed = remainingSpace / remainingElements;
                int b = layout[savedId];
                if (b == 3 || b == 4) maxAllowed *= 2;// (int)(maxAllowed*1.2);
                if (maxAllowed < 0)
                {
                    int a = 0;
                }

                if (minHeightMax >= maxAllowed)
                {
                    minHeightMax = maxAllowed;
                }


                block = layout[savedId];
                //remove remaining elements
                remainingElements--;
                if (block == 1 || block == 3 || block == 4) remainingElements--;
                //remove remaining space
                remainingSpace -= minHeightMax;
                if (block == 1) remainingSpace -= minHeightMax;
                //remove element(s) from list
                copy.RemoveAt(savedCopyId);
                if (block == 4) copy.RemoveAt(savedCopyId);
                // set the height
                heights[savedId] = minHeightMax;
                if (block == 4) heights[savedId] = minHeightMax;
                
            }
            return heights;
        }
        private void setImagesSize(List<Tuple<Frame, Frame>> usedTuples, List<int> layout, System.Drawing.Printing.PrintPageEventArgs e)
        {
            List<int> heights = getHeightOfEachSection(usedTuples, layout, e);
            int block = 0;
            int usedId = 0;
            for (int i = 0; i < layout.Count; i++)
            {
                block = layout[i];
                int times = 1;
                if (block == 4) times++;
                for (int j = 0; j < times; j++)
                {
                    usedTuples[usedId].Item1.setHeight(heights[i]);
                    usedTuples[usedId].Item2.setHeight(heights[i]);
                    usedId++;
                }
            }
        }
        private int getTotalSpace(List<int> layout, List<Tuple<Frame, Frame>> usedTuples)
        {
            int totalSpace = 0;
            int block = 0;
            int usedId = 0;
            for (int i = 0; i < layout.Count; i++)
            {
                block = layout[i];
                int times = (block == 1) ? 2 : 1;
                totalSpace += times * (usedTuples[usedId].Item1.image.Height + CatalogProperties.tagHeight + CatalogProperties.distanceBetweenElementsY);
                usedId++;
                if (block == 4) usedId++;
            }
            return totalSpace;
        }

        private void printLayout(List<int> layout, List<Tuple<Frame, Frame>> usedTuples, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int totalSpace = getTotalSpace(layout, usedTuples);
            int freeSpace = e.MarginBounds.Height - CatalogProperties.headerHeight - CatalogProperties.distanceBetweenElementsY - totalSpace - CatalogProperties.footerHeight;
            int numberOfLevels = layout.Count;
            foreach (int n in layout)
            {
                if (n == 1) numberOfLevels++;
            }
            int additionalDistanceY = freeSpace / (numberOfLevels + 1);
            int yPos = e.MarginBounds.Top + CatalogProperties.headerHeight + CatalogProperties.distanceBetweenElementsY + additionalDistanceY;
            int usedId = 0;
            int block = 0;
            for (int i = 0; i < layout.Count; i++)
            {
                block = layout[i];
                if (block == 1) //1 per line 
                {
                    Tuple<Frame, Frame> t = usedTuples[usedId];
                    int xLoc = e.MarginBounds.Left + e.MarginBounds.Width / 2 - t.Item1.image.Width / 2;
                    t.Item1.setLocation(xLoc, yPos);
                    yPos += t.Item1.image.Height + CatalogProperties.tagHeight + CatalogProperties.distanceBetweenElementsY + additionalDistanceY;
                    t.Item2.setLocation(xLoc, yPos);
                    yPos += t.Item1.image.Height + CatalogProperties.tagHeight + CatalogProperties.distanceBetweenElementsY + additionalDistanceY;
                    t.Item1.print(e);
                    t.Item2.print(e);
                }
                else if (block == 4)
                {
                    Tuple<Frame, Frame> t1 = usedTuples[usedId++];
                    Tuple<Frame, Frame> t2 = usedTuples[usedId];
                    int additionalDistanceX = (e.MarginBounds.Width - 4 * t1.Item1.image.Width - 3 * CatalogProperties.distanceBetweenElementsX) / 5;
                    List<Tuple<Frame, Frame>> ts = new List<Tuple<Frame, Frame>>();
                    ts.Add(t1);
                    ts.Add(t2);
                    int xLoc = e.MarginBounds.Left + additionalDistanceX;
                    foreach (Tuple<Frame, Frame> t in ts)
                    {
                        t.Item1.setLocation(xLoc, yPos);
                        xLoc += t.Item1.image.Width + additionalDistanceX + CatalogProperties.distanceBetweenElementsX;
                        t.Item2.setLocation(xLoc, yPos);
                        xLoc += t.Item1.image.Width + additionalDistanceX + CatalogProperties.distanceBetweenElementsX;
                        t.Item1.print(e);
                        t.Item2.print(e);
                    }
                    yPos += t1.Item1.image.Height + CatalogProperties.tagHeight + CatalogProperties.distanceBetweenElementsY + additionalDistanceY;
                }
                else
                {
                    Tuple<Frame, Frame> t = usedTuples[usedId];
                    int additionalDistanceX = (e.MarginBounds.Width - 2 * t.Item1.image.Width - CatalogProperties.distanceBetweenElementsX) / 3;
                    int locX = e.MarginBounds.Left + additionalDistanceX;
                    t.Item1.setLocation(locX, yPos);
                    locX += t.Item1.image.Width + CatalogProperties.distanceBetweenElementsX + additionalDistanceX;
                    t.Item2.setLocation(locX, yPos);
                    t.Item1.print(e);
                    t.Item2.print(e);
                    yPos += t.Item1.image.Height + CatalogProperties.tagHeight + CatalogProperties.distanceBetweenElementsY + additionalDistanceY;
                }

                usedId++;
            }
        }
    }
}
