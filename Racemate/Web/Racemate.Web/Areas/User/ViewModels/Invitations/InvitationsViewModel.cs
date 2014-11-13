namespace Racemate.Web.Areas.User.ViewModels.Invitations
{
    using System.Collections.Generic;
    using Racemate.Common.Contracts;

    public class InvitationsViewModel : IPaging<InvitationCodeViewModel>
    {
        public IEnumerable<InvitationCodeViewModel> Collection { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }
    }
}