using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PDFtoExcell
{
    class Program
    {
        const string format = "xlsx-single";
        const string apiKey = "a972r6985cxt";
        const string uploadURL = "https://pdftables.com/api?key=" + apiKey + "&format=" + format;
        static int Main(string[] args)
        {

            //if (args.Length != 2)
            //{
            //    Console.WriteLine("Usage: <PDF file name> <Output file name>");
            //    return 1;
            //}

            Console.WriteLine("Uploading content...");

            var task = PDFToExcel("D:\\PT PG Berjangka_Daily Statement_22102021.pdf", "D:\\test.xlsx");
            //task.Wait();

            Console.WriteLine("Response status {0} {1}", (int)task.Result, task.Result);

            if ((int)task.Result != 200)
            {
                return 1;
            }

            Console.WriteLine("Written {0} bytes", new System.IO.FileInfo("D:\\test.xlsx").Length);
            return 0;
        }

        static async Task<HttpStatusCode> PDFToExcel(string pdfFilename, string xlsxFilename)
        {
            using (var f = System.IO.File.OpenRead(pdfFilename))
            {
                var client = new HttpClient();
                var mpcontent = new MultipartFormDataContent();
                mpcontent.Add(new StreamContent(f));

                using (var response = await client.PostAsync(uploadURL, mpcontent))
                {
                    if ((int)response.StatusCode == 200)
                    {
                        using (
                            Stream contentStream = await response.Content.ReadAsStreamAsync(),
                            stream = File.Create(xlsxFilename))
                        {
                            await contentStream.CopyToAsync(stream);
                        }
                    }
                    return response.StatusCode;
                }
            }
        }

    }
}

