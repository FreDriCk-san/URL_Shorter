using System;
using System.Collections.Generic;
using System.Text;

namespace URL_Shorter
{
    public class URL_Processing
    {

        public static string Decode(string url)
        {
            try
            {
                var result = System.Web.HttpUtility.UrlDecode(url);

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Decode Error: " + exception.Message);
            }

            return null;
        }

    }
}
