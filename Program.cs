using System.Drawing;
using System.Drawing.Imaging;

namespace ImageResize
{
    class Program
    {
        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path"> Path to which the image would be saved. </param> 
        /// <param name="quality"> An integer from 0 to 100, with 100 being the highest quality. </param> 
        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
            Console.WriteLine("Küçültüldü");
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Klasör Konumu: ");
                string? path = Console.ReadLine();

                if (System.IO.Directory.Exists(path))
                {
                    Directory.Move(path, path + "old");
                    Directory.CreateDirectory(path);

                    string[] fileEntries = Directory.GetFiles(path + "old");

                    foreach (string pathToImage in fileEntries)
                    {
                        long fileLength = new FileInfo(pathToImage).Length / 1000000;
                        string targetPath = path + "\\" + new FileInfo(pathToImage).Name;

                        Console.WriteLine(new FileInfo(pathToImage).FullName);

                        if (fileLength >= 2 && (pathToImage.Contains("jpg") || pathToImage.Contains("png")))
                        {
                            Image myImage = Image.FromFile(pathToImage, true);
                            SaveJpeg(targetPath, myImage, 50);
                            //var helloWorld = await GetHelloWorldAsync();
                        }
                        else
                        {
                            File.Copy(pathToImage, targetPath);
                            Console.WriteLine("Hiçbir Değişiklik yapılmadı.");
                        }
                        GC.Collect();
                    }
                    Console.WriteLine("işlem başarılı...");
                }
                else
                {
                    Console.WriteLine("Geçersiz Konum...");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (Console.ReadLine().ToLower() == "e")
            {
                Main(new string[0]);
            }
        }

        static Task<string> GetHelloWorldAsync()
        {
            return Task.FromResult("Hello Async World");
        }
    }
}