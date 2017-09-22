
namespace EventConsole.Model.Entity
{
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;

        public partial class Runner
        {
                public Guid Id { get; set; }

                [Required(AllowEmptyStrings = false)]
                [MaxLength(64)]
                public String Name { get; set; }

                private ICollection<Event> _events;
                public ICollection<Event> Events {
                        get => _events ?? (_events = new HashSet<Event>());
                        set => _events = value;
                }
        }
}
