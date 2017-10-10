using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GuessWhat.Web.Data
{
    public class Animal
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Animal")]

        public string Name { get; set; }

        public virtual ICollection<AnimalDetail> AnimalDetails { get; set; }
    }
}