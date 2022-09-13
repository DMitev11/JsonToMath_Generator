namespace Experiment { 
    class GeneratorParser { 

        public static List<double> Parse(in string[] set_, Random random) { 
            List<double> generatedSet = new List<double>{}; 
        
            for(int i = 0; i < set_.Length; i++) {
                string builtSet = BuildSet(set_[i], generatedSet);
                generatedSet.Add(Generator.Generate(builtSet, random));
            }
            return generatedSet;
        }

        public static string BuildSet(string set, in List<double> numberList, bool ignoreCurly = false) { 
            while(true) { 
                int index = 0;
                int indexOpen = set.IndexOf('{');
                int indexClose = set.IndexOf('}');
                if((ignoreCurly == false && indexOpen <= 0) || indexClose == -1) { 
                    return set;
                }
                for(int i = 1; i <= indexClose - indexOpen -1; i++) {  
                    if(set[indexOpen + i] == '-') { 
                        index = -1 * (set[indexOpen + i + 1] - '0');
                        i++;
                    } else { 
                        index =  index * 10 + (set[indexOpen + i] - '0'); 
                    } 
                } 
                if(index >= numberList.Count) {
                    Console.WriteLine("There are only {0} numbers generated. Request for {1} number index failed", numberList.Count);
                    throw new Exception("Invalid character");
                }
                if(index >= 0) { 
                    set = set.Substring(0, indexOpen) + numberList[index] + set.Substring(indexClose + 1); 
                } else { 
                    set = set.Substring(0, indexOpen) + numberList[numberList.Count + index] + set.Substring(indexClose + 1); 
                }
            }
        }
    }
}