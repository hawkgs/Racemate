namespace Racemate.Web.Areas.User.ViewModels.Invitations
{
    using System;

    public class InvitationCodeViewModel
    {
        public string Code { get; set; }

        public string User { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}