
namespace EventConsole.Model.Entity
{
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;

        public partial class Wager
        {
                public Guid Id { get; set; }

                [Required(AllowEmptyStrings = false)]
                [MaxLength(64)]
                public String Name { get; set; }

                private ICollection<Wage> _wages;
                public ICollection<Wage> Wages {
                        get => _wages ?? (_wages = new HashSet<Wage>());
                        set => _wages = value;
                }
        }
}
