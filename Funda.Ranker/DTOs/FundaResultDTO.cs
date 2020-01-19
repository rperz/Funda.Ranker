using System;
using System.Collections.Generic;
using System.Text;
using Funda.Ranker.Models;

namespace Funda.Ranker
{
    public class FundaResultDTO
    {
        public List<ObjectDTO> Objects { get; set; } = new List<ObjectDTO>();
        public PagingDTO Paging { get; set; }
    }
}
