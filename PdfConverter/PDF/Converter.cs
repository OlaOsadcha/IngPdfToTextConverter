using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConverter.PDF
{
    public class Converter
    {
        private ILogger _logger;
        private IPdfExecutor _pdfExecutor;

        public Converter(ILogger<Converter> logger, IPdfExecutor pdfExecutor)
        {
            _logger = logger;
            _pdfExecutor = pdfExecutor;
        }

        public void Start()
        {
            _logger.LogInformation($"PdfConverter: Rozpoczęcie zmiany {DateTime.Now}");
            _pdfExecutor.Execute();
        }   

        public void Stop()
        {
            _logger.LogInformation($"PdfConverter: koniec działania {DateTime.Now}");
        }

        public void HandleError(Exception ex)
        {
            _logger.LogInformation($"PdfConverter: Błąd się pojawił o {DateTime.Now}. Błąd to: {ex.Message}");
        }
    }
}