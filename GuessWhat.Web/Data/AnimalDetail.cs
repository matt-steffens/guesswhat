using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GuessWhat.Web.Data
{
    public class AnimalDetail
    {
        public int Id { get; set; }

        [DisplayName("Animal")]
        public int AnimalId { get; set; }

        public virtual Animal Animal { get; set; }

        [Required]
        public string Detail { get; set; }
    }
}