using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildberries.Server.Domain.Entity
{
    public class UserEntity
    {
        [Key]
        public long Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsBot { get; set; } = false;

        public List<CardEntity> UserProduct { get; set; } = new List<CardEntity>();
    }
}
