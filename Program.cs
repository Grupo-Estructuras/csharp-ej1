// See https://aka.ms/new-console-template for more information

namespace csharp_ej1 {
    class Program {
        static void Main(string[] args) {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("");
            Console.WriteLine("Hello, World!");

        }
    }
}

