using Microsoft.Extensions.Logging;
using PdfConverter.Helpers;
using PdfConverter.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConverter.PDF
{
    public class PdfExecutor : IPdfExecutor
    {
        private ILogger _logger;

        public PdfExecutor(ILogger<PdfExecutor> logger)
        {
            _logger = logger;
        }

        public void Execute() 
        {
            try
            {
                string directoryPdf = ConfigurationManager.AppSettings["SciezkaPdf"];
                DirectoryInfo directory = new DirectoryInfo(directoryPdf);
                var allFiles = directory.GetFiles("*.pdf");
                foreach (var file in allFiles) 
                {
                    ConvertPdf(file);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"PdfConverter: Błąd się pojawił o {DateTime.Now}. Błąd to: {ex.Message}");
            }
        }

        private void ConvertPdf(FileInfo pdf)
        {
            try
            {
                List<PdfData> dataFromPdf = PdfToText.GetDataFromPdf(pdf.FullName);
                WriteInTxt(dataFromPdf, pdf);
            }
            catch
            {
                throw;
            }
        }

        private void WriteInTxt(List<PdfData> pdfs, FileInfo path)
        {
            try
            {
                string txtDirectory = path.DirectoryName + "\\Txt";
                if (!Directory.Exists(txtDirectory))
                {
                    Directory.CreateDirectory(txtDirectory);
                }

                string newFullName = $"{txtDirectory}\\{path.Name.Replace(path.Extension, string.Empty)}_{DateTime.Now.ToShortDateString()}.txt";
                using (TextWriter tw = new StreamWriter(newFullName))
                {
                    foreach (var line in pdfs)
                    {
                        string createdLine = $"{line.Lp}; {line.Contrator}; {line.Descrpition}; {line.Date.ToShortDateString()};" +
                            $"{line.Amount.ToString()}";
                        tw.WriteLine(createdLine);
                    }
                    tw.WriteLine("END OF FILE");
                }
            }
            catch 
            {
                throw;
            }
        }
    }
}