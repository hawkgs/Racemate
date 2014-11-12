namespace Racemate.Web.Helpers.Html
{
    using System;
    using System.Web.Mvc;

    public static class RaceHelpersExtensions
    {
        public static MvcHtmlString StripRaceAddress(this HtmlHelper helper, string address)
        {
            const int MAX_ADDRESS_LEN = 17;

            if (address.Length > MAX_ADDRESS_LEN)
            {
                address = String.Concat(address.Substring(0, MAX_ADDRESS_LEN), "...");
            }

            return new MvcHtmlString(address);
        }

        public static MvcHtmlString RaceStatusTag(this HtmlHelper helper,
            DateTime dateTime,
            int duration,
            bool isFinished,
            bool isCanceled,
            bool isForHome)
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