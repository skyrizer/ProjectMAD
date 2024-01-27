namespace CatViP_API.Helpers
{
    public class ConvertFileToByteArrayHelper
    {
        public static byte[] ConvertPDFFileToByteArray(string fileName)
        {
            var filePath = Path.Combine("Properties\\Resources\\PDFs", fileName);
            return File.ReadAllBytes(filePath);
        }

        public static byte[] ConvertImageFileToByteArray(string fileName)
        {
            var filePath = Path.Combine("Properties\\Resources\\Images", fileName);
            return File.ReadAllBytes(filePath);
        }
    }
}
