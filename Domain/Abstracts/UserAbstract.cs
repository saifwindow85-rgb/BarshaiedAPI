using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstracts
{
    public abstract  class UserAbstract // this is not dublicate its for later use for additonal informations
    {
        public int CreatedByUserId { get; set; }
        public User Creator { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public DateTime ?LastUpdate {  get; set; }
        public int ?UpdatedByUserId { get; set; }
        public User?UpdatedByUser { get; set; }
    }
}
