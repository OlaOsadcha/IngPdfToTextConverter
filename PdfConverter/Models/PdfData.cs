using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConverter.Models
{
    public class PdfData
    {
        public int Lp { get; set; }

        public string Contrator { get; set; }

        public string Descrpition { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}