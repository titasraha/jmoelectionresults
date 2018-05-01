using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PrepareResult
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                Dictionary<string, Dictionary<string, int>> Results = new Dictionary<string, Dictionary<string, int>>();
                List<string> CandidateCodeList = new List<string>();

                string DirPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                string[] VoteFiles = Directory.GetFiles(DirPath, "*.vote");
                foreach (string Vote in VoteFiles)
                {
                    using (StreamReader r = new StreamReader(Vote))
                    {
                        string s;
                        string VoteCode = null;

                        Dictionary<string, int> Result = new Dictionary<string, int>();
                        while ((s = r.ReadLine()) != null)
                        {
                            string[] parts = s.Split(':');
                            if (parts.Length != 2)
                                throw new Exception("Invalid vote file: " + Vote);
                            if (parts[0] == "Vote Code")
                                VoteCode = parts[1].Trim();
                            else
                            {
                                Result.Add(parts[0], 1);
                                if (!CandidateCodeList.Contains(parts[0]))
                                    CandidateCodeList.Add(parts[0]);
                            }
                        }
                        if (VoteCode == null)
                            throw new Exception("Vote code not found");

                        Results.Add(VoteCode, Result);
                    }
                }

                using (StreamWriter w = new StreamWriter(DirPath + "\\Results.csv", false))
                {
                    w.Write("Vote Code,");
                    foreach (string col in CandidateCodeList)
                    {
                        w.Write(col);
                        w.Write(",");
                    }
                    w.Write("\r\n");

                    foreach (var row in Results)
                    {
                        w.Write(row.Key);
                        w.Write(",");
                        foreach (string col in CandidateCodeList)
                        {
                            if (row.Value.TryGetValue(col, out int v))
                                w.Write(v.ToString());
                            else
                                w.Write("0");
                            w.Write(",");
                        }
                        w.Write("\r\n");
                    }
                }
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press any key to exit.");
                
            Console.ReadKey();
        }
    }
}
