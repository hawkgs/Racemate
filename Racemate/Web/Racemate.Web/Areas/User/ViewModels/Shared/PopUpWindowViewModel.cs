namespace Racemate.Web.Areas.User.ViewModels.Shared
{
    public class PopUpWindowViewModel
    {
        public string WindowId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string EncryptedId { get; set; }
    }
}