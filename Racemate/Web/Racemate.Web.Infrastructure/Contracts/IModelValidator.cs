namespace Racemate.Web.Infrastructure.Contracts
{
    using System.Web.Mvc;

    public interface IModelValidator
    {
        ModelStateDictionary ModelState { get; }

        TempDataDictionary TempData { get; }

        ViewDataDictionary ViewData { get; }
    }
}
