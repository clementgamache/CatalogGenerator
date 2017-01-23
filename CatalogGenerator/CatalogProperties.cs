using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogGenerator
{
    static class CatalogProperties
    {
        public static readonly int headerHeight = 48;  // TODO : CHANGE for bigger
        public static readonly int distanceBetweenElementsX = 15;
        public static readonly int distanceBetweenElementsY = 15;
        public static readonly int distanceBetweenImageAndTag = 6;
        public static readonly int tagHeight = 17;
        public static readonly int footerHeight = 30;
        public static readonly System.Drawing.Font headerFont = new System.Drawing.Font("Baskerville Old Face", 18, System.Drawing.FontStyle.Bold);
        public static readonly System.Drawing.Font titleFont = new System.Drawing.Font("Trebuchet MS", 18, System.Drawing.FontStyle.Bold);
        public static readonly System.Drawing.Font footerFont = new System.Drawing.Font("Calibri", 10);
        public static readonly System.Drawing.Font tagFont = new System.Drawing.Font("Arial", 8);
    }
}
