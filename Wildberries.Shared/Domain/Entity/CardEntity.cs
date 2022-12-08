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
        public int Id { get; set; }
        public long Article { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AveragePrice { get; set; }
        public int MainPrice { get; set; }
        public List<int> SalePrice { get; set; } = new List<int>();
        public string Url { get; set; } = string.Empty;
        public List<DateTimeOffset> Time { get; set; } = new List<DateTimeOffset>();
        public UserEntity User { get; set; } = new UserEntity();

    }

}
