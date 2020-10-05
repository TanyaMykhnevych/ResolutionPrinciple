using System;
using System.Collections.Generic;
using System.Linq;

namespace ResolutionMethod
{
    public class Clause : IComparable, IComparable<Clause>
    {
        private Variable _toMash; // Variable which is going to be resolved in a step
        public Clause(int id)
        {
            Variables = new List<Variable>();
            this.Id = id;
            Source = new int[0];
        }

        public List<Variable> Variables { get; private set; }
        public int[] Source { get; private set; }//Provides the two source Clauses for a resolved Clause
        public int Id { get; private set; }

        public bool AddVariable(Variable toAdd)
        {
            if (Variables.Any(v => string.Equals(v.Name, toAdd.Name)))
            {
                return false;
            }

            Variables.Add(toAdd);
            return true;
        }

        /**
         * Checks to see if this clause and another clause can be resolved. Clauses can
         * be resolved if there is exactly one variable pair (A and ~A).
         */
        public bool CanResolve(Clause other)
        {
            int oppositeCount = 0; //Use a count to track to make sure only one variable can be resolved
            foreach (var v1 in Variables)
            {
                // Check each variable from the clauses to find a mashable variable
                foreach (var v2 in other.Variables)
                {
                    if (string.Equals(v1.Name, v2.Name) && v1.Negated != v2.Negated)
                    {
                        _toMash = v1;
                        oppositeCount++;
                        break;
                    }
                }
            }
            return oppositeCount == 1;
        }


        // Resolves two clauses.
        public Clause Resolve(Clause other, int id)
        {
            Clause mashed = new Clause(id); // Newly resolved clause
            foreach (var v in Variables)
            { //Gets the variables from the first clause
                if (!string.Equals(v.Name, _toMash.Name))
                {
                    mashed.AddVariable(v);
                }
            }
            foreach (var v in other.Variables)
            { //Gets the variables from the second clause
                if (!string.Equals(v.Name, _toMash.Name))
                {
                    mashed.AddVariable(v);
                }
            }
            mashed.Source = new int[] { other.Id, this.Id };
            mashed.Variables.Sort();
            return mashed;
        }


        // Returns if resolution is finished and a contradiction is found.
        public bool ContradictionFound() => !Variables.Any();


        // Checks if this clause is subsumed by (more specific than) another clause.
        public bool SubsumedBy(Clause other)
        {
            foreach (var v in other.Variables)
            {
                if (!Variables.Contains(v))
                {
                    return false;
                }
            }

            return true;
        }

        // Compare by # of variables (greatest to least) then by id (least to greatest)
        public int CompareTo(Clause other)
        {
            if (Variables.Count == other.Variables.Count)
            {
                return Id - other.Id;
            }
            return Variables.Count - other.Variables.Count;
        }

        public int CompareTo(object other) => other is Clause ? CompareTo((Clause)other) : 0;

        public override bool Equals(Object obj)
        {
            if (obj is Clause other)
            {
                if (Variables.Count != other.Variables.Count)
                {
                    return false;
                }
                for (int i = 0; i < Variables.Count; i++)
                {
                    if (!Variables[i].Equals(other.Variables[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode() => Variables.Sum(v => v.GetHashCode());

        public override string ToString()
        {
            string str1 = ContradictionFound() ? "Contradiction Found" : Variables.Aggregate(string.Empty, (curr, next) => curr += $" {next}");
            string str2 = Source.Length == 0 ? "" : $"{Source[0]}, {Source[1]}";
            return $"{Id}. {str1} \t{str2}";
        }
    }
}
