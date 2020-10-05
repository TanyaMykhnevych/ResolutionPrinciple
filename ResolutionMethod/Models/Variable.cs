using System;

namespace ResolutionMethod
{
    public class Variable : IComparable<Variable>
    {
        public string Name { get; private set; } //Holds the name of the Variable
        public bool Negated { get; private set; } //True if the variable is negated

        public Variable(string name, bool negated)
        {
            this.Name = name;
            this.Negated = negated;
        }

        // Compare by negation (- before +) then in lexicographic order
        public int CompareTo(Variable other)
        {
            if (Negated)
            {
                if (other.Negated)
                {
                    return Name.CompareTo(other.Name);
                }
                return -1;
            }
            else if (other.Negated)
            {
                return 1;
            }
            return Name.CompareTo(other.Name);
        }

        public override bool Equals(Object obj)
        {
            if (obj is Variable other)
            {
                return Negated == other.Negated && Name.Equals(other.Name);
            }
            return false;
        }

        public override int GetHashCode() => Negated ? -(Name.GetHashCode()) : Name.GetHashCode();

        public override string ToString() => Negated ? $"~{Name}" : Name;
    }
}
