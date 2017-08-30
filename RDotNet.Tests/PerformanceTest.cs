using System.IO;
using NUnit.Framework;
using Spreads.R.Tests.Base;

namespace Spreads.R.Tests
{
    public class PerformanceTest : RDotNetTestFixture
    {
        [Test]
        public void TestCreateNumericVector()
        {
            var engine = this.Engine;
            RuntimeDiagnostics r = new RuntimeDiagnostics(engine);
            int n = (int)1e6;
            var dt = r.MeasureRuntime(RuntimeDiagnostics.CreateNumericVector, n);
            Assert.LessOrEqual(dt, 100, "Creation of a 1 million element numeric vector is less than a tenth of a second");
        }

        [Test]
        public void StackOverflowTest()
        {
            // get engine instance
            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();
            engine.EnableLock = true;
            engine.Initialize();

            // open a log file
            var logFileInfo = new FileInfo("r.log");
            var rCmd = @"sink('" + logFileInfo.FullName.Replace("\\", "\\\\") + "')";
            engine.Evaluate(rCmd); //.Dispose();

            // write lines to log file
            string newLine = @"""\n""";
            for (int i = 1; i <= 50000; i++)
            {
                engine.Evaluate(string.Format("cat(date(), \" Line # {0}\", {1})", i, newLine)); //.Dispose();
            }
        }
    }
}