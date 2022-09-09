namespace Experiment { 
    
    class GeneratorExpression {  
        public delegate double MathDelegateDouble(double lhs, double rhs);
        public delegate bool MathDelegateBool(double lhs, double rhs);

        private Token _operator;
        private List<double> _rhs = new List<double>{};
        private double _refValue = 0d;
        private MathDelegateBool _delegated;
        
        public GeneratorExpression(Token operator_, List<double> rhs_, double refValue, MathDelegateBool delegateBool){  
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