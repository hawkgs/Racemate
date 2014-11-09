namespace Racemate.Web.App_Start
{
    using System.Web.Mvc;

    public static class ViewEngineConfig
    {
        public static void TurnOnRazorOnly()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}