using System.Text.RegularExpressions;
namespace csharp_ej1
{
    class Githubscraper2
    {
        // Retorna un diccionario con los topics que cumplen el criterio establecido
        public static Dictionary<string, int> getTopics(string langAlias)
        {
            Dictionary<string, int> topicsDic = new Dictionary<string, int>();
            // Configuracion basica (no tengo garra para hacer un archivo)
            int maxPage = 30;
            int maxDays = 30;
            bool ignoreMainTopic = true;

            // Se recorren las paginas y se actua dependiendo del codigo enviado
            for (int currPage = 1; currPage <= maxPage; currPage++)
            {
                var statusCode = getTopicByPage(topicsDic, langAlias, currPage, maxDays, ignoreMainTopic);
                
                if (statusCode == 1)
                {
                    Console.WriteLine("No hay mas elementos entre la franja de dias");
                    break;
                }
                else if (statusCode == -1)
                {
                    Console.WriteLine("Error, se aborto el programa");
                    return new Dictionary<string, int>();
                }
                else if (statusCode == 0 && currPage == maxPage)
                {
                    Console.WriteLine("Se llego al limite de paginas");
                }
            }

            Console.WriteLine("Se termino de manera satisfactoria");

            return topicsDic;
        }

        // Imprime los elementos de un Dictionary y de un IOrderedEnumerable
        public static void printElements(Object elements)
        {
            if (elements is Dictionary<string, int>)
            {
                foreach (var element in (Dictionary<string, int>)elements)
                {
                    Console.WriteLine($"{element.Key}, {element.Value}");
                }
            }
            else if (elements is IOrderedEnumerable<KeyValuePair<string, int>>)
            {
                foreach (var element in (IOrderedEnumerable<KeyValuePair<string, int>>)elements)
                {
                    Console.WriteLine($"{element.Key}, {element.Value}");
                }
            }
            else {
                Console.WriteLine("Tipo de dato no soportado");
            }
        }

        // Ordena el diccionario retornando tipo Enumerable
        public static IOrderedEnumerable<KeyValuePair<string, int>> sortDictionary(Dictionary<string, int> dic)
        {
            return dic.OrderByDescending(x => x.Value);
        }

        // Busca los topics por pagina, agregando los valores al diccionario cuando sean menores a la fecha
        private static int getTopicByPage(Dictionary<string, int> topicsDic, string langAlias, int pageNum, int maxDays, bool ignoreMainTopic)
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            IEnumerable<HtmlAgilityPack.HtmlNode> nodes;
            
            // Se accede a la pagina de topics y se crea un vector de elementos con la clase my-4
            try
            {
                doc = web.Load($"https://github.com/topics/{langAlias}?o=desc&s=updated&page={pageNum}");
                nodes = doc.DocumentNode.Descendants().Where(item => item.HasClass("my-4"));
            }
            catch (System.Net.WebException err)
            {
                Console.WriteLine(err.Message);
                return -1;
            }

            // Recorre cada nodo que contiene el div con la fecha y los topics
            foreach (var node in nodes)
            {
                // Se guarda el html del div y se busca la fecha
                var nodeData = node.InnerHtml;
                var timeData = Regex.Match(nodeData, "mr-4[\\S\\s]*datetime=\"[^\"]+").Value;
                timeData = Regex.Match(timeData, "datetime=\"[^\"]+").Value;
                timeData = Regex.Match(timeData, "[^\"]*$").Value;

                // Si encuentra el patron de fecha, se buscan los topics relacionados
                if (!String.IsNullOrEmpty(timeData))
                {
                    DateTime enteredDate = DateTime.Parse(timeData);
                    TimeSpan difDate = DateTime.Now - enteredDate;

                    // Si la diferencia de dias es mayor o igual al maximo de dias
                    if (difDate.Days >= maxDays)
                        return 1;
                    
                    // Se buscan los topics asociados al div
                    var topics = Regex.Matches(nodeData, "Topic: [^\"]+");

                    foreach (var topic in topics)
                    {
                        var strTopic = topic.ToString();

                        // Revisa si encuentra un topic
                        if (!String.IsNullOrEmpty(strTopic))
                        {
                            var cleanTopic = Regex.Match(strTopic, "[^ ]*$").Value;

                            // Se ignora el topic con el nombre principal
                            if (ignoreMainTopic && cleanTopic == langAlias) continue;

                            // Se agrega el topic al diccionario sumando las entradas e ignorando el nombre topic
                            if (!topicsDic.TryGetValue(cleanTopic, out var value))
                            {
                                topicsDic[cleanTopic] = 1;
                            }
                            else
                            {
                                topicsDic[cleanTopic]++;
                            }
                        }
                    }
                }
            }
            
            return 0;
        }
    }
}