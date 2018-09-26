using System;
using System.Threading.Tasks;

namespace URL_Shorter
{
    class Program
    {
        static void Main(string[] args)
        {

            Task.Run(async () =>
            {
                var result = await API.ShorterAsync(URL_Processing.Decode(Resources.url));
                Console.WriteLine(result);
            });

            Console.ReadKey();
        }
    }
}
