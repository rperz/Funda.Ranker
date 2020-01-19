using System.Collections.Generic;

namespace Funda.Ranker.DTOs
{
    public class FundaResultDTO
    {
        public List<ObjectDTO> Objects { get; set; } = new List<ObjectDTO>();
        public PagingDTO Paging { get; set; }
    }
}
