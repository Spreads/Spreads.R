using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDotNet.Spreads {
    public static class SpreadsFunctions {
        public static GenericVector Echo(this REngine engine, params SymbolicExpression[] args) {
            var env = engine.GetSymbol("Spreads").AsEnvironment();
            var f = env.GetSymbol("Echo").AsFunction();
            //return f.Invoke(new Dictionary<string, SymbolicExpression>() { { "x", x } });
            return f.Invoke(args).AsList();
        }

        public static NumericVector Incr(this REngine engine, NumericVector x) {
            var env = engine.GetSymbol("Spreads").AsEnvironment();
            var f = env.GetSymbol("Incr").AsFunction();
            //return f.Invoke(new Dictionary<string, SymbolicExpression>() { { "x", x } });
            return f.Invoke(x).AsNumeric();
        }
    }
}
