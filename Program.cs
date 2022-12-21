using System;
using AngleSharp.Html.Parser;

//await BiruzaParser.BiruzaParser.ReadWithAngleSharpAsync("sold");
//await BiruzaParser.BiruzaParser.ReadWithAngleSharpAsync("booked");
//await BiruzaParser.BiruzaParser.ReadWithAngleSharpAsync("onsale");
string[] links = new string[] {"biruza_sold.txt", "biruza_booked.txt","biruza_onsale.txt"};
BiruzaParser.BiruzaParser.CreateFinal(links);
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
                filename = "biruza_onsale.txt";
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
            using StreamWriter file = new(filename, append: true);
            await file.WriteLineAsync(data);
        }

        public static void CreateFinal(string[] links){
            foreach (string link in links) {
                string[] lines = System.IO.File.ReadAllLines(link);

                for (int i=0; i<lines.Length; i+=11) {
                    string[] output = {lines[i+5], lines[i+8], "FLOOR", lines[i+1], lines[i], lines[i+2], "PP", lines[i+3], DateTime.Today.ToString()};
                    Console.WriteLine(String.Join('|', output));
                }
            }
        }
    }
}
//flat_id|flat_num|building|floor|area|rooms|price|sprice|status|date
//413484|284|Блок 2|5|45.70|1|6023260|131800.00|В продаже|2020-11-15


   

