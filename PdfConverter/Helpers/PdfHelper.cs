using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConverter.Helpers
{
    public static class PdfHelper
    {
        private readonly static string SFS = "SFS";
        private readonly static string VAT = "/VAT/";
        private readonly static string SFS_ = "(S)FS";
        private readonly static string ING = "ING";
        private readonly static string FS = "FS";
        private readonly static string FP = "FP";
        private readonly static string ZaplataZa = "Zapłata";
        private readonly static string PL = ";PL";
        public static int FindDescription(this string text)
        {
            if (text.Contains(SFS) || text.Contains(VAT) || text.Contains(SFS_)
                || text.Contains(ING) || text.Contains(FP) || text.Contains(FP)
                || text.Contains(ZaplataZa) || text.Contains(PL))
            {
                int indexOfSFS = text.IndexOf(SFS);
                int indexOfVAT = text.IndexOf(VAT);
                int indexOfSFS_ = text.IndexOf(SFS_);
                int indexOfIng = text.IndexOf(ING);
                int indexOfFS = text.IndexOf(FS);
                int indexOfFp = text.IndexOf(FP);
                int indexOfZaplata = text.IndexOf(ZaplataZa);
                int indexOfPL = text.IndexOf(PL);

                int[] indexes = { indexOfSFS, indexOfSFS_, indexOfVAT, indexOfIng, indexOfFS, indexOfFp,
                    indexOfZaplata, indexOfPL };
                int[] nonZeroIndex = new int[indexes.Length];
                nonZeroIndex = indexes.Where(x => x != -1).ToArray();

                int minIndex = nonZeroIndex.Min();
                return minIndex;

            }
            return -1;
        }
    }
}