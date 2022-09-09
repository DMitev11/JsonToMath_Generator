using System.Text;

namespace Experiment{
    public class Tokenizer { 
        private TextReader _reader;
        private Token _token;
        private Token _prevToken;
        private char _currentChar;
        private double _currentNumber;

        public Tokenizer(TextReader reader) { 
            _reader = reader;
            Next();
        }

        public Token token {get => _token;}
        public Token prevToken {get => _prevToken;}
        public double number {get => _currentNumber;}
        public char character {get => _currentChar;}
        
        public void Skip() { 
            Next();
            GenToken();
        }
        
        public char Next() {  
            while(true){
                int ch = _reader.Read(); 
                _currentChar = _parseChar(ch);
                if(char.IsWhiteSpace(_currentChar)) continue;
                return _currentChar;  
            } 
        }

        public Token GenToken() { 
            StringBuilder? sb = null; 
            Token generatedToken;
            if (char.IsDigit(_currentChar) || (_currentChar =='.' || _currentChar ==','))
            {
                // Capture digits/decimal point
                sb = sb == null? sb = new StringBuilder(): sb;
                bool haveDecimalPoint = false;
                while (char.IsDigit(_currentChar) || (!haveDecimalPoint && _currentChar == '.'))
                {
                    sb.Append(_currentChar);
                    haveDecimalPoint = _currentChar == '.' || _currentChar ==',';
                    if(char.IsDigit(this.PeekChar())) Next();
                    else break;
                }

                // Parse it
                _currentNumber = double.Parse(sb.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                generatedToken = Token.Number; 
            } else {
                generatedToken = _evalChar(_currentChar, true);
            }

            _prevToken = _token;
            _token = generatedToken;
           return _token;
        }

        public char PeekChar(){ 
            return _parseChar(_reader.Peek());
        }
        
        public Token PeekToken(){ 
            char peek = PeekChar();
            return _evalChar(peek);
        }
        
        private char _parseChar(in int character_) { 
            return character_ < 0? '\0' : (char)character_;
        }

        private Token _evalChar(in char char_, in bool skip_ = false) { 
            Token token = char.IsDigit(char_)? Token.Number : _evalSpecial(char_,skip_);
            if(token != Token.Unknown) {
                return token;
            } 

            // Special characters
            switch (char_)
            {
                case '\0':
                    token = Token.EOF;
                    break;

                case '+': 
                    token = Token.Add;
                    break;

                case '-': 
                    token = Token.Subtract;
                    break;

                case '*': 
                    token = Token.Multiply;
                    break;

                case '/': 
                    token = Token.Divide;
                    break; 

                case '%': 
                    token = Token.Modulo;
                    break; 
                    
                case '!': 
                    token = Token.Not;
                    break; 
                    
                case '(': 
                    token = Token.OpenParens;
                    break; 
                    
                case ')': 
                    token = Token.CloseParens;
                    break; 
                
                case '{': 
                    token = Token.OpenCurly;
                    break; 
                    
                case '}': 
                    token = Token.CloseCurly;
                    break; 
                    
                case ':': 
                    token = Token.To;
                    break; 
                    
                case '<': 
                    token = Token.LessThan;
                    break; 
                    
                case '>': 
                    token = Token.GreaterThan;
                    break; 

                default: 
                    token = Token.Unknown;
                    break;
            }
            return token; 
        }
        
        private Token _evalSpecial(in char char_, in bool skip_) { 
            Token token;
            char peek = PeekChar();
            // Special characters
            switch (char_)
            {
                case '|':
                    if(peek == '|'){
                        token = Token.Or; 
                        if(skip_)
                            Skip();
                    } else token = Token.Unknown;
                    break;

                case '&': 
                    if(peek == '&'){
                        token = Token.And; 
                        if(skip_)
                            Skip();
                    } else token = Token.Unknown;
                    break;

                case '!': 
                    if(peek == '='){
                        token = Token.NotEqual; 
                        if(skip_)
                            Skip();
                    } else token = Token.Unknown;
                    break;
 

                case '=': 
                    if(peek == '='){
                        token = Token.Equal; 
                        if(skip_)
                            Skip();
                    } else token = Token.Unknown;
                    break;

                case '<':
                    if(peek == '='){
                        token = Token.LessOrEqual;
                        if(skip_)
                            Skip();
                    } else token = Token.Unknown;
                    break;

                case '>':
                    if(peek == '='){
                        token = Token.GreaterOrEqual;
                        if(skip_)
                            Skip();
                    } else token = Token.Unknown;
                    break;
 
                default: 
                    token = Token.Unknown;
                    break;
            }
            return token; 
        }
        
    }

    public enum Token  {
        Unknown,
        EOF,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        Number,
        WhiteSpace,
        Not,
        And, 
        Or,
        Equal,
        NotEqual,
        LessOrEqual,
        LessThan,
        GreaterOrEqual,
        GreaterThan,
        OpenParens,
        CloseParens,
        OpenCurly,
        CloseCurly,
        To
    }
}