namespace Racemate.Web.Infrastructure.Validators
{
    using System.Web.Mvc;
    using Racemate.Web.Infrastructure.Contracts;

    public class RaceDetailsValidator : IModelValidator
    {
        public RaceDetailsValidator(
            ModelStateDictionary modelState,
            TempDataDictionary tempData,
            ViewDataDictionary viewData)
        {
            this.ModelState = modelState;
            this.TempData = tempData;
            this.ViewData = viewData;
        }

        public ModelStateDictionary ModelState { get; private set; }

        public TempDataDictionary TempData{ get; private set; }

        public ViewDataDictionary ViewData { get; private set; }

        public void NoSuchCarError()
        {
            this.ModelState.AddModelError(string.Empty, "The provided vehicle ID is invalid!");
            this.TempData["ViewData"] = this.ViewData;
        }

        public void RaceHasStartedError()
        {
            this.ModelState.AddModelError(string.Empty, "The race has already started!");
            this.TempData["ViewData"] = this.ViewData;
        }

        public void NoAvailableRacePositionsError()
        {
            this.ModelState.AddModelError(string.Empty, "All race positions has been already taken!");
            this.TempData["ViewData"] = this.ViewData;
        }
    }
}
