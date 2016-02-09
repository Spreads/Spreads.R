using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spreads;
using Spreads.Collections;
using Spreads.Generation;
using RDotNet.Spreads;
using System.Diagnostics;
using System.IO;


namespace RDotNet {
    public class SpreadsTest : RDotNetTestFixture {
        [Test]
        public void CouldRoundTripRPanelInPlace() {
            var engine = this.Engine;
            engine.Evaluate("library(Spreads)");
            var length = 1000;
            var width = 100;
            var rounds = 1;
            var series = SeriesGenerator.DummySeries(length, width, startValue: 1.0);

            var panel = new RNumericPanel(engine, length, null, width);

            var sw = new Stopwatch();

            sw.Start();
            for (int r = 0; r < rounds; r++) {
                panel.CopySeries(series);
            }
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
            var items = 0.000001 * length * width * rounds;
            Console.WriteLine($"Mops: {items * 1000.0 / sw.ElapsedMilliseconds}");

            var result = engine.Echo(panel.AsDataFrame).First();
            var list = new GenericVector(engine, result.DangerousGetHandle());
            var names = list.Names;
            var ticks = list.GetAttribute("TimeStamp").AsNumeric();
            var len = list.Length;


            var vl = new List<NumericVector>();
            for (int i = 0; i < len; i++)
            {
                var value = list[i].AsNumeric();
                vl.Add(value);
            }
            var vector = vl.ToArray();

            panel.Dispose();
        }


        [Test]
        public void CouldRoundTripRPanelAllocate() {
            var engine = this.Engine;
            engine.Evaluate("library(Spreads)");
            var length = 1000;
            var width = 100;
            var rounds = 100;
            var series = SeriesGenerator.DummySeries(length, width, startValue: 1.0);
            var sw = new Stopwatch();

            sw.Start();
            for (int r = 0; r < rounds; r++) {
                var panel = new RNumericPanel(engine, length, null, width);
                panel.CopySeries(series);
                panel.Dispose();
            }
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
            var items = 0.000001 * length * width * rounds;
            Console.WriteLine($"Mops: {items * 1000.0 / sw.ElapsedMilliseconds}");

        }

        [Test]
        public void CouldCallFunctionRegisteredFromR()
        {
            var engine = this.Engine;
            engine.Evaluate("library(Spreads)");

            // function must be registered: Spreads$RegisterFunction('Incr', function(x){x+1})
            var x = new NumericVector(this.Engine, 10);
            for (int i = 0; i < 10; i++)
            {
                x[i] = i;
            }
            var res = engine.Incr(x);
            for (int i = 0; i < 10; i++) {
                Assert.AreEqual(res[i], i+1);
            }
        }
    }
}