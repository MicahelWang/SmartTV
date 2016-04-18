using System.Linq;
using System.Text;
using System.Web;

namespace YeahAppCentre.Web.Utility
{
    public class StyleRenderExtension
    {
        public static IHtmlString Render(params string[] paths)
        {

            var bundles = CustomBundleCollection.StyleBundleCollection.Where(b => paths.Contains(b.Item1));
            var styleBuilder = new StringBuilder();

            bundles
                .ToList()
                .ForEach(b =>
                {
                    b.Item3.ForEach(v =>
                    {
                        var sourceUrl = string.Format("<link href=\"{0}\" rel=\"stylesheet\" />", v.Replace("~", b.Item2));
                        styleBuilder.AppendLine(sourceUrl);
                    });
                });

            return new HtmlString(styleBuilder.ToString());
        }
    }

    public class ScriptRenderExtension
    {
        public static IHtmlString Render(params string[] paths)
        {

            var bundles = CustomBundleCollection.ScriptBundleCollection.Where(b => paths.Contains(b.Item1));
            var scriptBuilder = new StringBuilder();

            bundles
                .ToList()
                .ForEach(b =>
                {
                    b.Item3.ForEach(v =>
                    {
                        var sourceUrl = string.Format("<script src=\"{0}\"></script>", v.Replace("~", b.Item2));
                        scriptBuilder.AppendLine(sourceUrl);
                    });
                });

            return new HtmlString(scriptBuilder.ToString());
        }
    }
}