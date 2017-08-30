using System.Diagnostics;
using System.Linq;

namespace Spreads.R.Diagnostics
{
    internal class FactorDebugView
    {
        private readonly Factor factor;

        public FactorDebugView(Factor factor)
        {
            this.factor = factor;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public string[] Value
        {
            get
            {
                return this.factor.GetFactors().ToArray();
            }
        }
    }
}