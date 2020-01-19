using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funda.Ranker
{
    public interface IRanker<T>
    {
        IOrderedEnumerable<T> GetRankedList();
    }

    public class RealtorRanker : IRanker<Realtor>
    {
        private readonly FundaClient _fundaClient;

        public RealtorRanker(FundaClient fundaClient)
        {
            _fundaClient = fundaClient;
        }

        public IOrderedEnumerable<Realtor> GetRankedList(params string[] searchTerms)
        {
            
        }
    }

    public class Realtor
    {
        public Realtor(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
