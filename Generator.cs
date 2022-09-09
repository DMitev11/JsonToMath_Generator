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

                List<GeneratorExpr> operationQue = new List<GeneratorExpr>{}; 
                bool inQue = false; 
                Token mathOperationInQue = Token.Unknown;
                List<List<double>> numbersInQue = new List<List<double>> (); 
                double refValue = 0;
                GeneratorExpr.MathDelegateBool? delegated = null;
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
                            delegated = GeneratorExpr.LessThan;
                        } break;
                        case Token.LessOrEqual : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = Token.Subtract;
                            delegated = GeneratorExpr.LessOrEqual;
                        } break;
                        case Token.GreaterThan : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = Token.Subtract;
                            delegated = GeneratorExpr.GreaterThan;
                        } break;
                        case Token.GreaterOrEqual : { 
                            if(mathOperationInQue != Token.Unknown){
                                Console.WriteLine("Cannot have a {0} sign after queing numbers for condition", _tokenizer.character );
                                throw new Exception("Invalid character");
                            } 
                            mathOperationInQue = Token.Subtract;
                            delegated = GeneratorExpr.GreaterOrEqual;
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
                            delegated = GeneratorExpr.EqualTo;

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
                            delegated = GeneratorExpr.NotEqualTo;
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
                delegated = delegated != null? delegated: GeneratorExpr.EqualTo;

                for(int i = 0; i < numbersInQue.Count; i++) { 
                    operationQue.Add(new GeneratorExpr(mathOperationInQue, numbersInQue[i], refValue, delegated));
                } 

                while(true) { 
                    double def = Math.Floor(numMin + (numMax - numMin) * (new Random()).NextDouble());
                    bool found = false;
                    foreach(GeneratorExpr gen in operationQue) { 
                        if(gen.Run(def)){
                            number = def;
                            found = true;
                            break;
                        };
                    }
                    if(found) break;
                }
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
    
    class GeneratorExpr {  
        public delegate double MathDelegateDouble(double lhs, double rhs);
        public delegate bool MathDelegateBool(double lhs, double rhs);

        private Token _operator;
        private List<double> _rhs = new List<double>{};
        private double _refValue = 0d;
        private MathDelegateBool _delegated;
        
        public GeneratorExpr(Token operator_, List<double> rhs_, double refValue, MathDelegateBool delegateBool){  
            _operator = operator_;
            _rhs = rhs_;
            _refValue = refValue;
            _delegated = delegateBool;
        }

        public bool Run (double lhs_) {  
           MathDelegateDouble operation_;
           switch (_operator) { 
                case Token.Modulo: { 
                    operation_ = new MathDelegateDouble(Modulo);
                }break;
                case Token.Subtract: { 
                    operation_ = new MathDelegateDouble(Subtract);
                }break;
                case Token.Add: { 
                    operation_ = new MathDelegateDouble(Subtract);
                }break;
                default: 
                    operation_ = new MathDelegateDouble(DefaultDouble);
                    break;
            } 
            for(int i = 0; i < _rhs.Count; i++) { 
                if(!_delegated(operation_(lhs_, _rhs[i]), _refValue)) {
                    return false;
                }
            }
            return true;
        }
        
        public static double DefaultBool(double lhs_, double rhs_) { 
            throw new Exception("Invalid math operation"); 
        } 
        
        
        public static double DefaultDouble(double lhs_, double rhs_) { 
            throw new Exception("Invalid math operation"); 
        } 
        
        public static double Modulo(double lhs_, double rhs_) { 
            return lhs_ % rhs_;
        } 
        
        
        public static double Subtract(double lhs_, double rhs_) { 
            return lhs_ - rhs_;
        } 
        
        
        public static double Add(double lhs_, double rhs_) { 
            return lhs_ + rhs_;
        } 
        
        public static bool EqualTo(double lhs_, double rhs_){ 
            return lhs_ == rhs_;
        }

        public static bool NotEqualTo(double lhs_, double rhs_) { 
            return lhs_ != rhs_;
        }
        public static bool GreaterThan(double lhs_, double rhs_){ 
            return lhs_ > rhs_;
        }

        public static bool LessThan(double lhs_, double rhs_) { 
            return lhs_ < rhs_;
        }
        public static bool GreaterOrEqual(double lhs_, double rhs_){ 
            return lhs_ >= rhs_;
        }

        public static bool LessOrEqual(double lhs_, double rhs_) { 
            return lhs_ <= rhs_;
        }
    }
}