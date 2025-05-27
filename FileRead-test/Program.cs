using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;

internal class Program
{
    public static string ExtractText(string path)
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
    public static void Main(string[] args)
    {
        var text = ExtractText("D:\\data\\software_engineer_cv.pdf");
        Console.WriteLine(text);
    }
}