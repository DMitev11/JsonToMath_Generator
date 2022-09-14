namespace Experiment {

    class ParserNode { 

        private Tokenizer _tokenizer;
        private ParserNode(Tokenizer tokenizer_) { 
            _tokenizer = tokenizer_;
        }

        private Node ParseExpression(){
            Node expr = AddAndSubtract();

            if(_tokenizer.token != Token.EOF) { 
                Console.WriteLine("Cannot utilize {0}, as its unknown character to operate with", _tokenizer.character);
                throw new Exception("Invalid character");
            }
            return expr;
        }

        private Node AddAndSubtract() { 
            Node lhs = MultiplyAndDivide();
            while(true) { 
                Func<double, double, double> op = null;
                if(_tokenizer.token == Token.Add) { 
                    op = (a, b)=> a+b;
                } else if (_tokenizer.token == Token.Subtract) { 
                    op = (a, b)=> a-b;
                }
                else {
                    return lhs;
                }

                _tokenizer.Skip();
                Node rhs = MultiplyAndDivide();

                lhs = new NodeBinary(lhs, rhs,op);
            }
        }

        private Node MultiplyAndDivide() { 
            Node lhs = ParseUnary();

            while(true) { 
                Func<double, double, double> op = null;
                if(_tokenizer.token == Token.Multiply) { 
                    op = (a,b)=> a*b;
                } else if(_tokenizer.token == Token.Divide) { 
                    op = (a,b)=> a/b;
                } else {
                    return lhs;
                }

                _tokenizer.Skip();
                Node rhs = ParseUnary();

                lhs = new NodeBinary(lhs, rhs, op);
            }
        }

        private Node ParseUnary() { 
            while(true) { 
                if(_tokenizer.token == Token.Add) { 
                    _tokenizer.Skip();
                    continue;
                } else if(_tokenizer.token == Token.Subtract) { 
                    _tokenizer.Skip();
                    
                    Node rhs = ParseUnary();
                    return new NodeUnary(rhs, (a) => -1*a);
                }

                return ParseLeaf();
            }
        }

        private Node ParseLeaf() { 
            
            if( _tokenizer.token == Token.Number) { 
                
                Node node = new NodeNumber(_tokenizer.number);
                _tokenizer.Skip();
                return node; 
            } else if (_tokenizer.token == Token.OpenParens) {

                _tokenizer.Skip();
                Node node = AddAndSubtract();

                if(_tokenizer.token != Token.CloseParens) { 
                    Console.WriteLine("{0} is missing in the que", ')');
                    throw new Exception("Invaild character");
                }

                _tokenizer.Skip();
                return node;
            }

            Console.WriteLine("Invalid {0} character in the expression", _tokenizer.character);
            throw new Exception("Invalid character");
        }

        ///<summary>  Returns result of a math expression, passed as a string </summary>
        ///<param name="str"> Math expression.  ex."10 + 5/2"</param>
        public static Node Parse(string str) { 
            Tokenizer tokenizer = new Tokenizer(new StringReader(str));
            tokenizer.GenToken();
            return Parse(tokenizer);
        }
        
        ///<summary>  Returns result of a math expression, passed as a string </summary>
        ///<param name="tokenizer"> Passed math expression string to a tokenizer. Ex. new Tokenizer("10 + 5/2")</param>
        public static Node Parse(Tokenizer tokenizer) { 
            ParserNode parser = new ParserNode(tokenizer);
            return parser.ParseExpression();

        }
    }
}