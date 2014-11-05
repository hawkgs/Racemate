﻿namespace Racemate.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Race
    {
        public Race()
        {
            this.Routepoints = new HashSet<RaceRoutePoint>();
            this.Participants = new HashSet<RaceParticipant>();
            this.Spectators = new HashSet<RaceSpectator>();
        }

        [Key]
        public string Id { get; set; }

        public DateTime DateTimeOfRace { get; set; }

        public int Duration { get; set; }

        public string OrganizerId { get; set; }

        public virtual User Organizer { get; set; }

        public RaceType Type { get; set; }

        public bool IsFinished { get; set; }

        // Route information

        public virtual ICollection<RaceRoutePoint> Routepoints { get; set; }

        // Event-related people

        public virtual ICollection<RaceParticipant> Participants { get; set; }

        public virtual ICollection<RaceSpectator> Spectators { get; set; }

        // Optional

        public string Name { get; set; }

        public int MoneyBet { get; set; }

        public string Password { get; set; }

        public bool IsCanceled { get; set; }
    }
}
