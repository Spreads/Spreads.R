using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDotNet.Spreads {
    public static class SpreadsFunctions {
        public static SymbolicExpression Echo(this REngine engine, SymbolicExpression x) {
            var f = engine.GetSymbol("spreads_echo").AsFunction();
            return f.Invoke(new Dictionary<string, SymbolicExpression>() { { "x", x } });
        }
    }
}
