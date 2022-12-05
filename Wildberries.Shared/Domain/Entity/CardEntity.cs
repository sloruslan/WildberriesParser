using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildberries.Shared.Domain.Entity
{
    public class CardEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public int AveragePrice { get; set; }
        public int MainPrice { get; set; }
        public int SalePrice { get; set; }
        public string Url { get; set; } = string.Empty;

        public List<TimePoint> TimePoint { get; set; } = new List<TimePoint>();

        public UserEntity? User { get; set; }

    }


    public class TimePoint
    {
        [Key]
        public int Id { get; set; }

        public DateTimeOffset Time { get; set; }

        public long Price { get; set; }

        public CardEntity? Card { get; set; }
    }
}
