using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace HireMate.Common
{
    public static class Utility
    {
        public static string ExtractTextFromPDF(string path)
        {
            try
            {
                using var pdfReader = new PdfReader(path);
                using var pdfDoc = new PdfDocument(pdfReader);
                var result = "";
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var text = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
                    result += text;
                }
                return result;
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine($"File not found: {ex.Message}");
                return "";
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine($"I/O Error: {ex.Message}");
                return "";
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return "";
            }
            
        }
    }
}
