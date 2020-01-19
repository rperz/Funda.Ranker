using System;
using System.Collections.Generic;
using System.Text;

namespace Funda.Ranker.Models
{
    public class ObjectDTO
    {
        public int MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }

        public Guid Id { get; set; }

        public string Adres { get; set; }
    }
}
