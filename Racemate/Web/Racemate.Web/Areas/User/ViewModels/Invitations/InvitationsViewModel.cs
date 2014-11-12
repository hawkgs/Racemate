namespace Racemate.Web.Areas.User.ViewModels.Invitations
{
    using Racemate.Web.Models.Common;
    using System.Collections.Generic;

    public class InvitationsViewModel : IPaging<InvitationCodeViewModel>
    {
        public IEnumerable<InvitationCodeViewModel> Collection { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }
    }
}