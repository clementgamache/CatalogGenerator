﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.IO;

namespace CatalogGenerator
{
    class Frame
    {
        public System.Drawing.Bitmap image;
        public System.Drawing.Point location = new System.Drawing.Point();
        public string name;
        public static bool makeEveryImageSquare = true;
        

        public void setLocation(int x, int y)
        {
            location = new System.Drawing.Point(x, y);
        }
        

        public void print(System.Drawing.Printing.PrintPageEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show(e.Graphics.DpiY.ToString());
            image.SetResolution(100, 100);
            e.Graphics.DrawImage(image, location.X, location.Y);
            e.Graphics.DrawString(
                name,
                CatalogProperties.tagFont,
                System.Drawing.Brushes.Black,
               location.X,
               location.Y + image.Height + CatalogProperties.tagFont.Height/2);
            //image.Dispose();

        }

        public Frame(string file, double widthInch, double heightInch, double mouldingInch)
        {
            image  = new Bitmap(file);
            name = System.IO.Path.GetFileNameWithoutExtension(file);
            if (makeEveryImageSquare)
            {


                int dim = Math.Max(image.Width, image.Height);
                Bitmap dest = new Bitmap(dim,dim);

                /*Bitmap tempBitmap = new Bitmap(image.Width, image.Height);
                using (Graphics g = Graphics.FromImage(tempBitmap))
                {
                    // Draw the original bitmap onto the graphics of the new bitmap
                    g.DrawImage(image, 0, 0);
                    // Use g to do whatever you like
                    dest = new Bitmap(dim, dim, g);
                }*/

                //using (Graphics origG = Graphics.FromImage(image))
                {
                    //dest = new Bitmap(dim, dim, origG);
                }
                using (Graphics g = Graphics.FromImage(dest))
                {
                    Pen white = new Pen(Color.White, 22);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, dim, dim);
                    g.DrawImage(image, (dim - image.Width) / 2, (dim - image.Height) / 2, image.Width, image.Height);
                }

                image = dest;
            }
            else
            {
                crop(widthInch, heightInch);
                putInFrame(widthInch, heightInch, mouldingInch);
            }

        }

        public void setHeight(int height)
        {
            double factor = (double)height / (double)image.Height;
            Bitmap resized = new Bitmap(image, new System.Drawing.Size((int)((double)image.Width * factor), (int)((double)image.Height * factor)));
            image.Dispose();
            image = resized;
        }

        private void crop(double widthInch, double heightInch)
        {
            //crop
            double wantedRatioBS = Math.Max(widthInch, heightInch) / Math.Min(widthInch, heightInch);
            double realRatioHW = (double)(image.Height) / (double)(image.Width);
            double realRatioBS = realRatioHW >= 1 ? realRatioHW : (1.0 / realRatioHW);
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle();
            
            if (((wantedRatioBS < realRatioBS) && (realRatioHW >= 1.0)) || //Biggest is too big and biggest is height
                ((wantedRatioBS > realRatioBS) && (realRatioHW <= 1.0)))  //Smallest is too big and smallest is height
            {
                cropArea.Width = image.Width;
                cropArea.Height = (int)((double)image.Width / (realRatioHW <= 1.0 ? wantedRatioBS : (1.0/wantedRatioBS)));
                cropArea.X = 0;
                cropArea.Y = (image.Height - cropArea.Height) / 2;
            }
            else if (((wantedRatioBS < realRatioBS) && (realRatioHW < 1.0)) ||
                    ((wantedRatioBS > realRatioBS) && (realRatioHW > 1.0))) 
            {
                cropArea.Height = image.Height;
                cropArea.Width = (int)((double)image.Height / (realRatioHW > 1.0 ? wantedRatioBS : (1.0 / wantedRatioBS)));
                cropArea.Y = 0;
                cropArea.X = (image.Width - cropArea.Width) / 2;
            }
            else
            {
                cropArea.Location = new System.Drawing.Point(0, 0);
                cropArea.Size = image.Size;
            }
            System.Drawing.Bitmap im = image.Clone(cropArea, image.PixelFormat);
            image.Dispose();
            image = im;
        }

        private void putInFrame(double widthInch, double heightInch, double mouldingInch)
        {
            double d = (double)mouldingInch * Math.Max((double)image.Width, (double)image.Height)/Math.Max(widthInch, heightInch);
            d = Math.Ceiling(d);
            System.Drawing.Bitmap frame = new System.Drawing.Bitmap(image.Width + (int)(2*d), image.Height + (int)(2*d));
            using (Graphics g = Graphics.FromImage(frame))
            {
                g.FillRectangle(Brushes.Black, 0, 0, frame.Width, frame.Height);
                g.DrawImage(image, new Rectangle((int)d,(int)d,image.Width, image.Height));
            }
            image = frame;
        }

        public bool isHorizontal()
        {
            return image.Width > image.Height;
        }

    }
}
