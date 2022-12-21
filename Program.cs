using System;
using AngleSharp.Html.Parser;

await BiruzaParser.BiruzaParser.ReadWithAngleSharpAsync("sold");

namespace BiruzaParser 
{

    class BiruzaParser 
    {
        static async Task<string> SendRequestWithHttpClientAsync(string link) {
            var client = new HttpClient();
            var responseBody = await client.GetStringAsync(link);
            return responseBody;
        }

        public static async Task ReadWithAngleSharpAsync(string status) {
            string link;
            string filename;
            string soldFlatsLink = "https://2.ac-biryuzovaya-zhemchuzhina.ru/flats/all?floor=&type=&status=4&minArea=&maxArea=&minPrice=&maxPrice=";
            string bookedFlatsLink = "https://2.ac-biryuzovaya-zhemchuzhina.ru/flats/all?floor=&type=&status=3&minArea=&maxArea=&minPrice=&maxPrice=";
            string onSaleFlatsLink = "https://2.ac-biryuzovaya-zhemchuzhina.ru/flats/all?floor=&type=&status=2&minArea=&maxArea=&minPrice=&maxPrice=";
            if (status == "sold") {
                link = soldFlatsLink;
                filename = "biruza_sold.txt";
            } else if (status == "booked") {
                link = bookedFlatsLink;
                filename = "biruza_booked.txt";
            } else {
                link = onSaleFlatsLink;
                filename = "biruza_onsale";
            }
            var htmlSourceCode = await SendRequestWithHttpClientAsync(link);
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(htmlSourceCode);
            foreach (AngleSharp.Dom.IElement item in document.QuerySelector(".table > tbody:nth-child(2)").GetElementsByTagName("td")) {
                var data = item.TextContent;
                await WriteData(data, filename);
            };
        }

        static async Task WriteData(String data, string filename) {
            data = String.Concat(data.Where(c => !Char.IsWhiteSpace(c)));
            using StreamWriter file = new("Biruza.txt", append: true);
            await file.WriteLineAsync(data);
    }

    }
}

