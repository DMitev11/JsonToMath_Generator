namespace Experiment { 
    class Generator{ 
        private Tokenizer _tokenizer; 

        private Generator(Tokenizer tokenizer) {
            _tokenizer = tokenizer;
        }

        public double run() {  
            double number = 0;
            Token token = _tokenizer.GenToken();
            //set
            if(token == Token.OpenCurly) {  
                _tokenizer.Skip();   
                List<double> numbersInQue = new List<double>{0d};
                while(_tokenizer.token != Token.EOF && _tokenizer.token != Token.CloseCurly) { 
                    switch(_tokenizer.token) {
                        case Token.Subtract: {
                            if(numbersInQue.Last() != 0) {
                                Console.WriteLine("Cannot write a {0} sign after {1} number", '-', numbersInQue.Last());
                                throw new Exception("Invalid character");
                            }
                            _tokenizer.Skip();
                            numbersInQue[numbersInQue.Count -1] = -1 * _tokenizer.number;
                        }break;
                        case Token.Comma: {
                            if(numbersInQue.Count == 0) {
                                Console.WriteLine("Cannot write a {0} without preceeding numbers", ',');
                                throw new Exception("Invalid character");
                            }
                            numbersInQue.Add(0d);
                        }break;
                        case Token.Number: {
                            numbersInQue[numbersInQue.Count -1] = 
                                numbersInQue[numbersInQue.Count -1] * 10 + numbersInQue[numbersInQue.Count -1] >= 0?
                                 _tokenizer.number : -1*_tokenizer.number;
                        }break;
                        default: 
                            Console.WriteLine("Cannot have number set with invalid {0} character", _tokenizer.character);
                            throw new Exception("Invalid character");
                    }
                    _tokenizer.Skip();   
                }
                number = numbersInQue[(int)Math.Round(new Random().NextDouble() * (numbersInQue.Count-1))];
            } // range 
            else {
                double numMin = genRange();
                        _tokenizer.Skip();
                double numMax = genRange(); 
                //Generate Range 
                if(numMax < numMin) { 
                    Console.WriteLine("You can have range from {0} to {1} ", numMin, numMax);
                    throw new Exception("Generator-genRange: Invalid number range");
                }

                List<GeneratorExpression> operationQue = new List<GeneratorExpression>{}; 
                bool inQue = false; 
                Token mathOperationInQue = Token.Unknown;
                List<List<double>> numbersInQue = new List<List<double>> (); 
                double refValue = 0;
                GeneratorExpression.MathDelegateBool? delegated = null;
                while(_tokenizer.token != Token.EOF) {  
                    switch(_tokenizer.token){  
                        case Token.Modulo : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = _tokenizer.token;
                        } break;
                        case Token.LessThan : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = Token.Subtract;
                            delegated = GeneratorExpression.LessThan;
                        } break;
                        case Token.LessOrEqual : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = Token.Subtract;
                            delegated = GeneratorExpression.LessOrEqual;
                        } break;
                        case Token.GreaterThan : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = Token.Subtract;
                            delegated = GeneratorExpression.GreaterThan;
                        } break;
                        case Token.GreaterOrEqual : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = Token.Subtract;
                            delegated = GeneratorExpression.GreaterOrEqual;
                        } break;
                        case Token.OpenParens:  { 
                            if(inQue) {
                                Console.WriteLine("Cannot have a {0}, after already having {0} written", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            inQue = true;
                            numbersInQue = new List<List<double>> ();
                            numbersInQue.Add(new List<double>{0}); 
                        } break;
                        case Token.CloseParens:  {
                            if(!inQue) {
                                Console.WriteLine("Cannot have a {0}, after already having {0} written", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            inQue = false;
                        } break;
                        case Token.And:  { 
                            if(!inQue) {
                                Console.WriteLine("Cannot have a {0}, if ( not present", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }  
                            if(numbersInQue.Count < 0 || numbersInQue.Last().Count < 0) { 
                                Console.WriteLine("Cannot have {0}, before having numbers", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            numbersInQue[numbersInQue.Count -1].Add(0d);
                        } break;
                        case Token.Or:  { 
                            if(!inQue) {
                                Console.WriteLine("Cannot have a {0}, if ( not present", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }  
                            numbersInQue.Add(new List<double>{0d});
                        } break;
                        case Token.Equal:  { 
                            if(inQue) {
                                Console.WriteLine("Cannot have a == in ()", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            if(numbersInQue.Count == 0 || numbersInQue.Last().Count == 0) { 
                                Console.WriteLine("Cannot have == condition without numbers", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            delegated = GeneratorExpression.EqualTo;

                        } break;
                        case Token.NotEqual:  { 
                            if(inQue) {
                                Console.WriteLine("Cannot have a != in ()", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            if(numbersInQue.Count == 0 || numbersInQue.Last().Count == 0) { 
                                Console.WriteLine("Cannot have != condition without numbers", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            delegated = GeneratorExpression.NotEqualTo;
                        } break;
                        case Token.Number:  { 
                            if(!inQue && numbersInQue.Last().Count == 0) {
                                Console.WriteLine("Cannot have {0}, without () que present", _tokenizer.character);
                                throw new Exception("Invalid character");
                            } 
                            //@TODO remove after testing
                            if(numbersInQue.Count == 0 || numbersInQue.Last().Count == 0) { 
                                Console.WriteLine("Cannot have {0}, without previous numbers present", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            if (!inQue) { 
                                refValue = refValue * 10 + refValue >= 0? _tokenizer.number : -1*_tokenizer.number;
                            } else { 
                                double num = numbersInQue.Last().Last();
                                numbersInQue[numbersInQue.Count -1][numbersInQue[numbersInQue.Count -1].Count -1] 
                                    = num * 10 + num >= 0 ? _tokenizer.number : -1 *_tokenizer.number;
                                num = numbersInQue.Last().Last();

                            } 
                        } break;
                        case Token.Subtract:  { 
                            if(!inQue) {
                                Console.WriteLine("Cannot have {0}, without () que present", _tokenizer.character);
                                throw new Exception("Invalid character");
                            } 
                            //@TODO remove after testing
                            if(numbersInQue.Count == 0 || numbersInQue.Last().Count == 0) { 
                                Console.WriteLine("Cannot have {0}, without previous numbers present", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            if(_tokenizer.PeekToken() != Token.Number) {  
                                Console.WriteLine("Cannot have {0}, without preceeding a number", _tokenizer.character);
                                throw new Exception("Invalid character");
                            }
                            _tokenizer.Skip(); 
                            if (delegated != null) { 
                                refValue = -1 * _tokenizer.number; 
                            } else { 
                                numbersInQue[numbersInQue.Count -1][numbersInQue[numbersInQue.Count -1].Count -1]  = -1 * _tokenizer.number; 
                            }
                        } break;
                        default: {
                            Console.WriteLine("Invalid character {0}", _tokenizer.character);
                            throw new Exception("Invalid character");
                        }
                    } 
                    _tokenizer.Skip();  
                } 
                delegated = delegated != null? delegated: GeneratorExpression.EqualTo;

                for(int i = 0; i < numbersInQue.Count; i++) { 
                    operationQue.Add(new GeneratorExpression(mathOperationInQue, numbersInQue[i], refValue, delegated));
                } 

                number = generateNumber(operationQue, numMin, numMax);
                
            }
            

            return number;
        }

        private double generateNumber(in List<GeneratorExpression> que, double min, double max) { 
            double number = 0;
            while(true) { 
                double def = Math.Floor(min + (max - min) * (new Random()).NextDouble());
                bool found = false;
                foreach(GeneratorExpression gen in que) { 
                    if(gen.Run(def)){
                        number = def;
                        found = true;
                        break;
                    };
                }
                if(found) break;
            }
            return number;
        }
        private double genRange() {
            double? number = null; 
            while(_tokenizer.token == Token.Number || _tokenizer.token ==Token.Subtract) {  
                if(number != null && _tokenizer.token == Token.Subtract){
                    Console.WriteLine("Invalid character sequence `{0}{1}`", number, _tokenizer.character);
                    throw new Exception("Generator-genRange: Invalid sign");
                } 
                switch(_tokenizer.token) { 
                    case Token.Subtract: number = -1; break;
                    default: {
                        if(number == null) number = _tokenizer.number;
                        else { 
                            number = number * 10 + number >= 0? _tokenizer.number : -1* _tokenizer.number;
                        }
                    } break;
                }
                _tokenizer.Next();
                _tokenizer.GenToken();
            } 
            number = number != null? number : 0d;
            return (double) number;
        }

        private static double gen(Tokenizer tokenizer){ 
            Generator _generator = new Generator(tokenizer); 
            return _generator.run();
        }

        public static double gen(StringReader reader){ 
            return gen(new Tokenizer(reader));
        }
        
        public static double gen(string numberString){ 
            return gen(new Tokenizer(new StringReader(numberString)));
        } 
    }
}