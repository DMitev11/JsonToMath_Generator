using System;

namespace Experiment { 
    public static class Program { 
 
        public static int Main(string[] args) { 
            // ex.
                // "0:9",
                // "10:99%({0})",
                // "0:9" 
            Random random = new Random(Guid.NewGuid().GetHashCode());
            List<string> set = new List<string>(); 
            List<double> generatedSet = new List<double>(); 
            Console.WriteLine("Please write several lines of number generation expressions");
            while(true) { 
                string? read = Console.ReadLine();  
                if(read != null && read != ""){
                    try { 
                        List<string> test = new List<string>(set);  
                        test.Add(read); 
                        generatedSet = GeneratorParser.Parse(test.ToArray(), random); 
                        set.Add(read); 
                    } catch(Exception e) { 
                        Console.WriteLine(e);
                    }
                } else break;
            } 
            // ex.
                // {2} * ({1} / {0})
                // {4} ( the previous expression ) * {2} / {0}
            List<string> expressions = new List<string>();
            Console.WriteLine("Please write several lines of calculatory expressions");
            while(true) { 
                string? read = Console.ReadLine();
                if(read != null && read != ""){ 
                    try { 
                        string builtExpression = GeneratorParser.BuildSet(read, generatedSet, true);
                        double expressionResult = ParserNode.Parse(builtExpression).Eval(); 
                        generatedSet.Add(expressionResult);
                        expressions.Add(read); 
                    } catch (Exception e) { 
                        Console.WriteLine(e);
                    }
                } else break;
            }
            // ex.
                // same
            string resultExpression = "";
            Console.WriteLine("Please write the result expression");
            while(true) { 
                string? read = Console.ReadLine();
                if(read != null && read != ""){ 
                    try { 
                        string builtExpression = GeneratorParser.BuildSet(read, generatedSet, true);
                        double expressionResult = ParserNode.Parse(builtExpression).Eval();  
                        resultExpression = read; 
                        break;
                    } catch (Exception e) { 
                        Console.WriteLine(e);
                    } 
                }
            }
            // ex. 
                // " Мъж купува {0} топки и {2} кукли. Всички играчки струват {3}. {2}-те кукли струват {4} лв. Колко струва 1 топка?"
            string title = "";
            Console.WriteLine("Please write the title");
            while(true) { 
                string? read = Console.ReadLine();
                if(read != null && read != ""){
                    try { 
                        string builtExpression = GeneratorParser.BuildSet(read, generatedSet, true); 
                        title = read;  
                        break;
                    } catch (Exception e) { 
                        Console.WriteLine(e);
                    } 
                }
            }
            int times = 5;
            Console.WriteLine("How many times would you like the excercise to be generated");
            while(true) { 
                string? read = Console.ReadLine();
                if(read != null){
                    times = Int32.Parse(read); 
                    break;
                }
            }
            
            Console.WriteLine("Generating...."); 
            for(int i = 0; i < times; i++) { 
                generatedSet = GeneratorParser.Parse(set.ToArray(), random); 
                foreach(string expression in expressions){ 
                    string builtExpression = GeneratorParser.BuildSet(expression, generatedSet, true);
                    double expressionResult = ParserNode.Parse(builtExpression).Eval(); 
                    generatedSet.Add(expressionResult);
                }
                string titleBuilt = GeneratorParser.BuildSet(title, generatedSet, true); 
                string resultBuilt = GeneratorParser.BuildSet(resultExpression, generatedSet, true);
                double result = ParserNode.Parse(resultBuilt).Eval();
                double[] answers = generateAnswers(result);
                
                for(int j = 0; j < set.Count; j++) { 
                    Console.WriteLine("Number set {0} generated number: {1}", set[j], generatedSet[j]);
                }
                for(int j = 0; j < expressions.Count; j++) { 
                    Console.WriteLine("Expression set {0} generated number: {1}", expressions[j], generatedSet[j + set.Count]); 
                }
                Console.WriteLine("Generated title is: {0}", titleBuilt);
                Console.WriteLine("Result expression {0} generated: {1}", resultExpression, resultBuilt);
                Console.WriteLine("Result is: {0}", result);
                foreach(double answer in answers) { 
                    Console.WriteLine("Generated answer: {0}", answer);
                }
            }
            Console.ReadLine();
            return 0;
        }

        public static double[] generateAnswers(double correctAnswer) { 
            Random random = new Random(Guid.NewGuid().GetHashCode() * (int) DateTimeOffset.Now.ToUnixTimeSeconds());
            double coeff = 0.15;
            if (correctAnswer <= 10) {
              coeff = 4;
            } else if (correctAnswer <= 20) {
              coeff = 1;
            } 
            int min = (int) Math.Round(correctAnswer == 0 ? -10 : Math.Round(correctAnswer - correctAnswer * coeff));
            int max = (int) Math.Round(correctAnswer == 0 ? 10 : Math.Round(correctAnswer + correctAnswer * coeff));
            List<double> answers = new List<double> {correctAnswer}; 

            while (answers.Count != 4) {
              double incorrectAnswer = Math.Round(min + (max - min) * random.NextDouble());
              bool set = true;
              foreach(double answer in answers) { 
                if(answer == incorrectAnswer) {set = false; break;}
              }
              if(set) answers.Add(incorrectAnswer);
            } 
            return shuffleArray(answers).ToArray();
        }

        public static List<double> shuffleArray(List<double> array) {
            Random random = new Random(Guid.NewGuid().GetHashCode() * (int) DateTimeOffset.Now.ToUnixTimeSeconds());
            for (int i = array.Count - 1; i > 0; i--) {
                int j = (int) Math.Floor(random.NextDouble() * (i + 1));
                double temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return array;
        }

        public static int Example(string[] args) {  
            string[] set = {
                "0:9",
                "10:99%({0})",
                "0:9"
            };
            string[] expression = {"{2} * ({1} / {0})"};  
            const string title = " {1} / ? = {-1} ?"; 
            Random randomGenerator = new Random(Guid.NewGuid().GetHashCode() * (int) DateTimeOffset.Now.ToUnixTimeSeconds()); 
            for(uint i = 0; i < 5; i++ ) {
                List<double> generatedSet = GeneratorParser.Parse(set, randomGenerator);
                Console.WriteLine("Set `{0}` generated: {1}", set[0], generatedSet[0]); 
                Console.WriteLine("Set `{0}` generated: {1}", set[1], generatedSet[1]); 
                Console.WriteLine("Set `{0}` generated: {1}", set[2], generatedSet[2]); 
               //  Console.WriteLine("Set `{0}` generated: {1}", set[3], generatedSet[3]); 
               //  Console.WriteLine("Set `{0}` generated: {1}", set[4], generatedSet[4]); 
                string expressionBuilt = GeneratorParser.BuildSet(expression[0], generatedSet, true);
                Console.WriteLine("Expression {0} would be: {1}", expression[0], expressionBuilt);
                double number = ParserNode.Parse(expressionBuilt).Eval(); 
                Console.WriteLine("Result is: {0}", number);
                generatedSet.Add(number);
                string titleBuilt = GeneratorParser.BuildSet(title, generatedSet, true);
                Console.WriteLine("Title {0} would be: {1}", title, titleBuilt); 
                double answer1 = number + Math.Round((randomGenerator.NextDouble() *2 - 1) * 15);
                double answer2 = number + Math.Round((randomGenerator.NextDouble() *2 - 1) * 15);
                double answer3 = number + Math.Round((randomGenerator.NextDouble() *2 - 1) * 15);
                Console.WriteLine("Option 0 is: {0}", answer1);
                Console.WriteLine("Option 1 is: {0}", answer2);
                Console.WriteLine("Option 2 is: {0}", answer3);
                Console.WriteLine();
            } 
            return 0;
        }
 
    } 
}