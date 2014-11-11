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

        public static MvcHtmlString CarExtra(this HtmlHelper helper, bool isOn, string name)
        {
            string @class = isOn ? "on" : String.Empty;
            string htmlString = String.Format("<div class=\"extra {0}\"><p>{1}</p></div>", @class, name);

            return new MvcHtmlString(htmlString);
        }

        public static MvcHtmlString CarHpPerTon(this HtmlHelper helper, int hp, int weight)
        {
            float inTons = (float)weight / (float)1000;
            float hpPerTon = hp / inTons;

            return new MvcHtmlString(((int)hpPerTon).ToString());
        }
    }
}