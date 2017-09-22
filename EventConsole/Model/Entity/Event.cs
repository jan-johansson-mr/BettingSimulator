
namespace EventConsole.Model.Entity
{
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;

        public partial class Event
        {
                public Guid PoolId { get; set; }

                public Guid RunnerId { get; set; }

                [Required]
                public Pool Pool { get; set; }

                [Required]
                public Runner Runner { get; set; }

                private ICollection<Wage> _wages;
                public ICollection<Wage> Wages {
                        get => _wages ?? (_wages = new HashSet<Wage>());
                        set => _wages = value;
                }
        }
}
