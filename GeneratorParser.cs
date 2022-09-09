namespace Experiment { 
    class GeneratorParser { 

        public static List<double> parse(in string[] set_) { 
            List<double> generatedSet = new List<double>{};
        
            for(int i = 0; i < set_.Length; i++) {
                string builtSet = buildSet(set_[i], generatedSet);
                generatedSet.Add(Generator.gen(builtSet));
            }
            return generatedSet;
        }

        static string buildSet(string set, in List<double> numberList) { 
            while(true) { 
                int index = 0;
                int indexOpen = set.IndexOf('{');
                int indexClose = set.IndexOf('}');
                if(indexOpen <= 0 || indexClose == -1) { 
                    return set;
                }
                for(int i = 1; i <= indexClose - indexOpen -1; i++) {  
                    index =  index * 10 + (set[indexOpen + i] - '0');

                }
                if(index >= numberList.Count) {
                    Console.WriteLine("There are only {0} numbers generated. Request for {1} number index failed", numberList.Count);
                    throw new Exception("Invalid character");
                }
                set = set.Substring(0, indexOpen) + numberList[index] + set.Substring(indexClose + 1); 
            }
        }
    }
}