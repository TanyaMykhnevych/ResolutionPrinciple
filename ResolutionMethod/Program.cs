using System;
using System.Collections.Generic;

namespace ResolutionMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Clause> clauses = ResolutionFileReader.ReadClausesFromFile(@"../../../Inputs/task3.in");

            // Run main program
            new Driver().Go(clauses);
            Console.ReadLine();
        }
    }
}
