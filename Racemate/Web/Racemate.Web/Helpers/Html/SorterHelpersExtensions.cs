namespace Racemate.Web.Helpers.Html
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public static class SorterHelpersExtensions
    {
        public static MvcHtmlString SorterAnchor(this HtmlHelper helper,
            string action,
            string controller,
            string sortBy = null,
            bool? isDesc = null,
            string area = "User")
        {
            string url = String.Format("/{0}/{1}/{2}", area, controller, action);

            if (sortBy != null && isDesc != null)
            {
                url = String.Concat(url, String.Format("?sortBy={0}&order={1}", sortBy, isDesc.Value ? "desc" : "asc"));
            }

            TagBuilder anchor = new TagBuilder("a");

            anchor.SetInnerText(HttpUtility.HtmlDecode("&#8645;"));
            anchor.AddCssClass("sorter");
            anchor.MergeAttribute("href", url);

            return new MvcHtmlString(anchor.ToString());
        }
    }
}