namespace Experiment { 
    class NodeUnary : Node { 

        private Node _rhs;
        Func<double, double> _op;

        public NodeUnary(Node rhs_, Func<double, double> op_) { 
            _rhs = rhs_;
            _op = op_;
        }

        public override double Eval() {
            return _op(_rhs.Eval());
        }
    }
}