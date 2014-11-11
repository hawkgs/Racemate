namespace Racemate.Web.Helpers.Html
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    public static class HelpersExtensions
    {
        public static MvcHtmlString CarRaceType(this HtmlHelper helper, string title)
        {
            string @class = title.ToLower().Replace(' ', '-');
            string htmlString = String.Format("<div class=\"type {0}\" title=\"{1}\"></div>", @class, title);

            return new MvcHtmlString(htmlString);
        }
    }
}