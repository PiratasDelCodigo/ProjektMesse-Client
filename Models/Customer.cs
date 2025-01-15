using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messe_Client.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Image { get; set; }
        public int FavoriteId { get; set; }
        public int CompanyId { get; set; }
        public Company? PendingCompany { get; set; }
    }
}
