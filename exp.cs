using System;

namespace Experiment { 
    public static class Program { 

        public static int Main(string[] args) {  
             string[] set = new[]{
                 "-999:999%(9||4&&8)==2",
                 "0:999%(4&&3&&2)",
                 "0:9 <= (4&&7&&1)",
                 "0:99 <= ({1})", "{0, 15, 24, 36}"}; 
             const string expression = "{0} - {1} + {4} / {3}";
             int seed = Guid.NewGuid().GetHashCode();
             Random randomGenerator = new Random(seed);
             for(uint i = 0; i < 5; i++ ) {
                 List<double> generatedSet = GeneratorParser.Parse(set, seed);
                 Console.WriteLine("Set `{0}` generated: {1}", set[0], generatedSet[0]);
                 Console.WriteLine("Set `{0}` generated: {1}", set[1], generatedSet[1]);
                 Console.WriteLine("Set `{0}` generated: {1}", set[2], generatedSet[2]); 
                 Console.WriteLine("Set `{0}` generated: {1}", set[3], generatedSet[3]); 
                 Console.WriteLine("Set `{0}` generated: {1}", set[4], generatedSet[4]); 
                 string expressionBuilt = GeneratorParser.BuildSet(expression, generatedSet, true);
                 Console.WriteLine("Expression {0} would be: {1}", expression, expressionBuilt);
                 double number = ParserNode.Parse(expressionBuilt).Eval(); 
                 double answer1 = number + (randomGenerator.NextDouble() *2 - 1) * 15;
                 double answer2 = number + (randomGenerator.NextDouble() *2 - 1) * 15;
                 double answer3 = number + (randomGenerator.NextDouble() *2 - 1) * 15;
                 Console.WriteLine("Option 0 is: {0}", answer1);
                 Console.WriteLine("Option 1 is: {0}", answer2);
                 Console.WriteLine("Option 2 is: {0}", answer3);
                 Console.WriteLine("Result is: {0}", number);
                 Console.WriteLine();
            }

            return 0;
        }
 
    } 
}