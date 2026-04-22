using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstracts
{
    public abstract  class UserAbstract
    {
        public int CreatedByUserId { get; set; }
        public User Creator { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public DateTime ?LastUpdate {  get; set; }
        public int UpdatedByUserId { get; set; }
        public User?UpdatedByUser { get; set; }
    }
}
