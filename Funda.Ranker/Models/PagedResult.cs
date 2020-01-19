using System;
using System.Collections.Generic;
using System.Text;

namespace Funda.Ranker.Models
{
    public class PagedResult<T>
    {
        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }

        public T Result { get; set; }
    }
}
