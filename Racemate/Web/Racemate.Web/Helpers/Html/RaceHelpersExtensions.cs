namespace Racemate.Web.Helpers.Html
{
    using System;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Racemate.Common;

    public static class RaceHelpersExtensions
    {
        private const string RaceController = "Race";

        public static MvcHtmlString StripRaceAddress(this HtmlHelper helper, string address)
        {
            const int MAX_ADDRESS_LEN = 17;

            if (address.Length > MAX_ADDRESS_LEN)
            {
                address = String.Concat(address.Substring(0, MAX_ADDRESS_LEN), "...");
            }

            return new MvcHtmlString(address);
        }

        public static MvcHtmlString RaceDetailsLink(this HtmlHelper helper, int id, string name)
        {
            string encryptedId = QueryStringBuilder.EncryptRaceId(id, name);

            return LinkExtensions.ActionLink(helper, name, "Details", RaceController, new { Id = encryptedId }, new { @class = "title" });
        }

        public static MvcHtmlString RaceTypeImage(this HtmlHelper helper, string typeName)
        {
            var div = new TagBuilder("div");

            div.AddCssClass(typeName.ToLower().Replace(' ', '-'));
            div.MergeAttribute("id", "race-type");

            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString RaceStatusTag(this HtmlHelper helper,
            DateTime dateTime,
            int duration,
            bool isFinished,
            bool isCanceled,
            bool isForHome = false)
        {
            string name = String.Empty;
            string homeClass = isForHome ? "home" : String.Empty;
            DateTime endTime = dateTime.AddHours(duration);

            if (isCanceled)
            {
                name = "CANCELED";
            }
            else if (isFinished || DateTime.Now > endTime)
            {
                name = "FINISHED";
            }
            else if (dateTime > DateTime.Now)
            {
                name = "UPCOMING";
            }
            else if (dateTime < DateTime.Now && DateTime.Now < endTime)
            {
                name = "IN PROGRESS";
            }

            string @class = name.ToLower().Replace(' ', '-');
            string htmlString = String.Format("<span class=\"race-status {2} {0}\">{1}</span>", @class, name, homeClass);

            return new MvcHtmlString(htmlString);
        }
    }
}