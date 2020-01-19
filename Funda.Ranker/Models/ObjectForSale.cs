using System;
using System.Collections.Generic;
using System.Text;

namespace Funda.Ranker.Models
{
    public class ObjectForSale
    {
        public ObjectForSale(Guid id, string address, Realtor realtor)
        {
            Id = id;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Realtor = realtor ?? throw new ArgumentNullException(nameof(realtor));
        }

        public Guid Id { get; }
        public string Address { get; }
        public Realtor Realtor { get; }
    }
}
