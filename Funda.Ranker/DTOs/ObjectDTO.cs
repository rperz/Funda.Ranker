using System;

namespace Funda.Ranker.DTOs
{
    public class ObjectDTO
    {
        public int MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }

        public Guid Id { get; set; }

        public string Adres { get; set; }
    }
}
