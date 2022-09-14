namespace Experiment { 
    class GeneratorParser { 

        /// <summary>
        /// Generate numbers, 1 per set. Pass a Random as source of RNG.
        /// </summary>
        /// <param name="set_"> set of range and restrictions to parse from 
        /// <para> "0:99%(2)==1", generates 0-99 number, which %2 == 1 </para>
        /// <para> "0:99%({0})", generates 0-99 number, which % of the previous number (0 number in the set) == 0 </para>
        /// <para> "0:99<=({1}&&5||4)==3", generates 0-99 number, which % of the previous number (1 number in the set) and 5 or only %4 == 3 </para>
        /// </param> 
        /// <param name="random"> RNG source </param> 
        public static List<double> Parse(in string[] set_, Random random) { 
            List<double> generatedSet = new List<double>{}; 
        
            for(int i = 0; i < set_.Length; i++) {
                string builtSet = BuildSet(set_[i], generatedSet);
                generatedSet.Add(Generator.Generate(builtSet, random));
            }
            return generatedSet;
        }

        /// <summary>
        /// Replace tokens in string, from passed set of numbers. Optional: ignore strings starting with `{}`
        /// </summary>
        /// <param name="set"> string set
        /// <para> ex. "This is number 0 {0}, This is number 1 {1}
        /// </param>  
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