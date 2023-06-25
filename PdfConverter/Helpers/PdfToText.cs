using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using PdfConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PdfConverter.Helpers
{
    public static class PdfToText
    {
        public static List<PdfData> GetDataFromPdf(string path)
        {
            try
            {
                List<PdfData> danePdf = new List<PdfData>();
                PdfData pdfData = new PdfData();
                PdfReader reader = new PdfReader(path);

                string textFromPdf = string.Empty;
                bool isContratorFilled = false;
                bool isDescriptionFilled = false;

                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                int lp = 1;
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    textFromPdf = PdfTextExtractor.GetTextFromPage(reader, page, its);
                    textFromPdf = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(textFromPdf)));
                    if (string.IsNullOrEmpty(textFromPdf))
                    {
                        break;
                    }

                    string[] theLines = textFromPdf.Substring(textFromPdf.IndexOf("Nr wiersza")).Split('\n');
                    for (int index = 0; index < theLines.Length; index++)
                    {

                        if (theLines[index].StartsWith(lp.ToString()) && theLines[index].Length > 15)
                        {
                            isContratorFilled = false;
                            pdfData = new PdfData();
                            pdfData.Lp = lp;
                            bool ifDate = false;
                            string[] firstLine = theLines[index].Split(' ');
                            ifDate = DateTime.TryParse(firstLine[1], out DateTime date);

                            if (ifDate)
                            {
                                pdfData.Date = date;

                                string description = string.Empty;
                                description = theLines[index].Substring(theLines[index].IndexOf(firstLine[2]));

                                FillContrator(ref pdfData, description, ref isContratorFilled);
                                lp++;
                                continue;
                            }
                        }
                        else if (theLines[index].All(c => (c >= 48 && c <= 57) || c == 46 || c == 44))
                        {
                            var kwotaStr = theLines[index].Replace(".", string.Empty);
                            decimal kwota = 0;
                            bool ifKwota = false;
                            ifKwota = decimal.TryParse(kwotaStr, out kwota);
                            if (ifKwota)
                            {
                                pdfData.Amount = kwota;
                            }
                            if (!danePdf.Any(x => x.Lp == pdfData.Lp))
                            {
                                pdfData.Descrpition = pdfData.Descrpition.Replace(";", string.Empty);
                                danePdf.Add(pdfData);
                            }

                            isDescriptionFilled = true;
                            continue;
                        }

                        if (!isContratorFilled)
                        {
                            FillContratorDescription(ref pdfData, theLines[index], ref isContratorFilled, ref isDescriptionFilled);
                            continue;
                        }

                        if (!isDescriptionFilled)
                        {
                            pdfData.Descrpition += " " + theLines[index];
                            continue;
                        }

                    }

                }
                reader.Close();
                return danePdf;
            }
            catch
            {
                throw;
            }
        }

        private static void FillContratorDescription(ref PdfData pdfData, string description,
            ref bool isContratorFilled, ref bool isDescriptionFilled)
        {
            int indexOfSpecialCharacters = description.FindDescription();

            if (indexOfSpecialCharacters == -1)
            {
                pdfData.Contrator += " " + description;
            }
            else if (indexOfSpecialCharacters == 0)
            {
                pdfData.Descrpition += description;
                isContratorFilled = true;
            }
            else
            {
                int lenghtOpis = description.Length;
                pdfData.Contrator += " " + description.Substring(0, indexOfSpecialCharacters);
                pdfData.Descrpition += description.Substring(indexOfSpecialCharacters);
                isContratorFilled = true;
                isDescriptionFilled = false;
            }
        }

        private static void FillContrator(ref PdfData pdfData, string description, ref bool isContratorFilled)
        {
            int indexOfSpecialCharacters = description.FindDescription();

            if (indexOfSpecialCharacters == -1)
            {
                pdfData.Contrator = description;
            }            
            else
            {
                int lenghtOpis = description.Length;
                pdfData.Contrator = description.Substring(0, lenghtOpis - indexOfSpecialCharacters);
                pdfData.Descrpition += description.Substring(indexOfSpecialCharacters);
                isContratorFilled = true;               
            }
        }
    }
}