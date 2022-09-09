using System;

namespace Experiment { 
    public static class Program { 

        public static int Main(string[] args) { 
            string[] set = new[]{"-999:999%(9||4&&8)==2", "0:999%(4&&3&&2)", "0:9 <= (4&&7&&1)", "0:99 <= ({1})", "{0, 15, 24, 36}"}; 
            for(uint i = 0; i < 5; i++ ) {
                List<double> generatedSet = GeneratorParser.parse(set);
                Console.WriteLine("Set `{0}` generated: {1}", set[0], generatedSet[0]);
                Console.WriteLine("Set `{0}` generated: {1}", set[1], generatedSet[1]);
                Console.WriteLine("Set `{0}` generated: {1}", set[2], generatedSet[2]); 
                Console.WriteLine("Set `{0}` generated: {1}", set[3], generatedSet[3]); 
                Console.WriteLine("Set `{0}` generated: {1}", set[4], generatedSet[4]); 
                Console.WriteLine();
            }
            return 0;
        }
 
    } 
}