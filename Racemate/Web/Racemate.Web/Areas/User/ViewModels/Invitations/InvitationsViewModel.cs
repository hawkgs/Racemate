namespace Racemate.Web.Areas.User.ViewModels.Invitations
{
    using System.Collections.Generic;

    public class InvitationsViewModel
    {
        public IEnumerable<InvitationCodeViewModel> Codes { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }
    }
}