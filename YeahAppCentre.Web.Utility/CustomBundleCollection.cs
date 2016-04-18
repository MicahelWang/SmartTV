using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahAppCentre.Web.Utility
{
    public static class CustomBundleCollection
    {
        public static List<Tuple<string,string, List<string>>> StyleBundleCollection;
        public static List<Tuple<string, string, List<string>>> ScriptBundleCollection;

        public static void IncludeStyle(string styleName, string path, params string[] virtualPaths)
        {
            if (StyleBundleCollection == null)
                StyleBundleCollection = new List<Tuple<string, string, List<string>>>();

            StyleBundleCollection.Add(new Tuple<string, string, List<string>>(styleName, path, virtualPaths.ToList()));
        }

        public static void IncludeScript(string scriptName, string path, params string[] virtualPaths)
        {
            if (ScriptBundleCollection == null)
                ScriptBundleCollection = new List<Tuple<string, string, List<string>>>();

            ScriptBundleCollection.Add(new Tuple<string, string, List<string>>(scriptName, path, virtualPaths.ToList()));
        }
    }
}
