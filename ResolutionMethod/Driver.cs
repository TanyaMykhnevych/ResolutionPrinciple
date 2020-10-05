using System;
using System.Collections.Generic;

namespace ResolutionMethod
{
    public class Driver
    {
        public void Go(List<Clause> clauses)
        {
            PerformResolution(clauses);
        }


        /**
         * Main part of the program which runs the algorithm to resolve clauses.
         */
        private void PerformResolution(List<Clause> clauses)
        {
            PriorityQueue<Clause> queue = new PriorityQueue<Clause>(); // Queue to select next best clause
            HashSet<Clause> searched = new HashSet<Clause>();  // Clauses that have already been mashed
            HashSet<Clause> willSearch = new HashSet<Clause>();   // Clauses that have been added to the queue before

            // Fill queue with initial clauses
            clauses.ForEach(c => queue.Enqueue(c));

            // Loop until every clause has been expanded
            while (queue.Any() && !queue.Peek().ContradictionFound())
            {
                Clause toMash = queue.Dequeue();

                // Check if clause has been expanded already
                if (searched.Contains(toMash)) continue;

                // Checks if clause is subsumed by (more specific than) an already mashed clause
                bool isSubsumed = false;
                foreach (Clause other in searched)
                {
                    if (toMash.SubsumedBy(other))
                    {
                        isSubsumed = true;
                        break;
                    }
                }
                if (isSubsumed) continue;

                // Mash current clause with all previously mashed clauses
                foreach (Clause other in searched)
                {
                    if (toMash.CanResolve(other))
                    {
                        Clause mashed = toMash.Resolve(other, clauses.Count + 1);

                        // Check if mashed clause is already in the queue
                        if (willSearch.Contains(mashed))
                        {
                            continue;
                        }

                        // Add mashed clause to queue and knowledge-base
                        willSearch.Add(mashed);
                        queue.Enqueue(mashed);
                        clauses.Add(mashed);

                        // Check if contradiction was made
                        if (mashed.ContradictionFound())
                        {
                            break;
                        }
                    }
                }

                // Clause has now been mashed before
                searched.Add(toMash);
            }

            if (!queue.Any())
            {
                Console.WriteLine("No contradiction found");
            }
            else
            {
                Console.WriteLine("Contradiction was found");
                PrintClauseTree(clauses);
            }
        }

        // Print the clause tree in descending order with only the used clauses.
        private void PrintClauseTree(List<Clause> clauses)
        {
            bool[] used = new bool[clauses.Count];
            FindClauses(clauses.Count - 1, clauses, used); // Find all of the clauses on the solution path
            for (int i = 0; i < used.Length; i++)
            { // Print out all of the clauses on the solution path
                if (used[i])
                {
                    Console.WriteLine(clauses[i]);
                }
            }

            Console.WriteLine($"Size of final clause set: {clauses.Count}\n");
        }

        // Used to find the clauses which are on the path to the solution.
        private void FindClauses(int index, List<Clause> clauses, bool[] used)
        {
            // Check if already marked
            if (used[index])
            {
                return;
            }

            used[index] = true;

            // Stop if from original knowledge-base, otherwise continue with 2 clauses that were mashed
            // to form the current clause
            Clause curr = clauses[index];
            if (curr.Source.Length != 0)
            {
                FindClauses(curr.Source[0] - 1, clauses, used); // Recurses through the first resolved clause of the given clause
                FindClauses(curr.Source[1] - 1, clauses, used); // Recurses through the second resolved clause of the given clause
            }
        }
    }
}
