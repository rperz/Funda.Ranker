using System;

namespace Funda.Ranker.Models
{
    public class Realtor : IEquatable<Realtor>
    {
        public Realtor(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Id { get; }
        public string Name { get; }

        public bool Equals(Realtor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Realtor) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}