using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Arcomage.Core
{
    public class ParseDescription
    {
        public static string Parse(string originalText)
        {
            if (originalText == null)
            {
                return string.Empty;
            }
            string pattern = "(<[^>]*>)";
            string value = originalText;
               

            string text = Regex.Replace(value, pattern, String.Empty);

            text = text.Replace("&gt;", ">").Replace("&lt;", "<");

            string pattern2 = @"(\W+\w+;)";


            foreach (Match match in Regex.Matches(value, pattern2, RegexOptions.IgnoreCase))
            {
                text = AddTag(match.Value, text);
            }
          
            return text;
        }

        private static string AddTag(string paramOut, string text)
        {
            string returnVal = text;
            string param = paramOut.Replace(": ", "").Replace(";", "").Trim();

            switch (param)
            {
                case "bold":
                    returnVal = "<b>" + text + "</b>";
                    break;
                case "italic":
                    returnVal = "<i>" + text + "</i>";
                    break;
                default:
                    if (param.Contains("#"))
                    {
                        returnVal = "<color=" + param + ">" + text + "</color>";
                    }
                    else if (param.Contains("pt"))
                    {
                        returnVal = "<size=" + param.Replace("pt", "") + ">" + text + "</size>";
                    }
                    break;
            }

            return returnVal;
        }
    }
}
