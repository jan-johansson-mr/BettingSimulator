
namespace EventConsole.Model.Entity
{
        using System;
        using System.ComponentModel.DataAnnotations;

        public partial class Wage
        {
                public Guid PoolId { get; set; }
                public Guid WagerId { get; set; }
                public Guid RunnerId { get; set; }

                [Required]
                public int Money { get; set; }

                [Required]
                public Event Event { get; set; }

                [Required]
                public Wager Wager { get; set; }
        }
}
