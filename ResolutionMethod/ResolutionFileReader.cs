using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ResolutionMethod
{
    public static class ResolutionFileReader
    {
        public static List<Clause> ReadClausesFromFile(string path)
        {
            List<Clause> clauses = new List<Clause>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    // Parser
                    Clause c; // Clause to be added
                    String readClause;
                    Regex rgx = new Regex("~?\\w+"); // Matches strings which can start with ~ and have one or more alphanumeric symbols
                    int givenID = 1;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Loop through all lines of input
                        c = new Clause(givenID);
                        readClause = line;
                        Match mat = rgx.Match(readClause);
                        while (mat.Success)
                        { // In each line of input, find valid strings which match the pattern
                            String var = mat.Groups[0].Value;
                            if (var[0] == '~')
                            {
                                c.AddVariable(new Variable(var.Substring(1), true)); // Add Varible with negation
                            }
                            else
                            {
                                c.AddVariable(new Variable(var, false)); // Add Variable without negation
                            }
                            mat = mat.NextMatch();
                        }
                        c.Variables.Sort();
                        clauses.Add(c);
                        givenID++;
                    }
                }

                return clauses;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found.");
                return new List<Clause>();
            }
        }
    }
}
