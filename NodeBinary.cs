namespace Experiment { 
    class NodeBinary : Node { 

        private Node _lhs;
        private Node _rhs;
        private Func<double, double, double> _op;

        public NodeBinary(Node lhs_, Node rhs_, Func<double, double, double> op_){
            _lhs = lhs_;
            _rhs = rhs_;
            _op = op_;
        }

        public override double Eval()
        {
            return _op(_lhs.Eval(), _rhs.Eval());
        }
    }
}