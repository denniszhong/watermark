using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Watermark;

namespace Test
{
    [TestClass]
    public class UnitTest_AddWatermark
    {
        [TestMethod]
        public void TestAddTextWalterMark()
        {
            string srcImage = @"C:\Users\suzhong\Desktop\New folder\ora2.jpg";
            string outPath = @"C:\Users\suzhong\Desktop\New folder";
            string srcText = "Chinese American Civic Association";

            WaterMark walterMark = new WaterMark(srcImage, srcText, outPath);
            walterMark.ShortText = "咔咔华人社区";
            var outImage = walterMark.AddTextWalterMark();
        }

        [TestMethod]
        public void TestAddTextWalterMarkVertical()
        {
            string srcImage = @"C:\Users\suzhong\Desktop\New folder\email.png";
            string outPath = @"C:\Users\suzhong\Desktop\New folder";
            string srcText = "Chinese American Civic Association";

            WaterMark walterMark = new WaterMark(srcImage, srcText, outPath);
            walterMark.ShortText = "咔咔华人社区";
            var outImage = walterMark.AddTextWalterMark();
        }

        [TestMethod]
        public void TestAddTextWalterMark_Png_Small()
        {
            string srcImage = @"C:\Users\suzhong\Desktop\New folder\Untitled.png";
            string outPath = @"C:\Users\suzhong\Desktop\New folder";
            string srcText = "Chinese American Civic Association";

            WaterMark walterMark = new WaterMark(srcImage, srcText, outPath);
            walterMark.ShortText = "咔咔华人社区";
            var outImage = walterMark.AddTextWalterMark();
        }
    }
}
