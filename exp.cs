using System;

namespace Experiment { 
    public static class Program { 

        public static int Main(string[] args) { 
            string[] set = new[]{"-10:999%(9||4&&8)==2", "-999:999%(4&&3&&2)", "0:9<=(4&&7&&1)"}; 
            for(uint i = 0; i < 5; i++ ) {
                Console.WriteLine("Set `{0}` generated: {1}", set[0], Generator.gen(set[0]));
                Console.WriteLine("Set `{0}` generated: {1}", set[1], Generator.gen(set[1]));
                Console.WriteLine("Set `{0}` generated: {1}", set[2], Generator.gen(set[2])); 
                Console.WriteLine();
            }
            return 0;
        }
 
    } 
}