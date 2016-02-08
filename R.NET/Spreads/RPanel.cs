using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spreads;
using Spreads.Collections;

namespace RDotNet.Spreads {

    // TODO RMatrix. R is column-major, need to create a single long vector and copy data to 
    // it in the same way we do with RPanel.

    public class RNumericPanel : IDisposable {
        private readonly REngine _engine;
        private readonly int _length;
        private readonly int _width;
        private readonly string[] _names;
        private NumericVector _ticks;
        private GenericVector _panel;
        private NumericVector[] _numericVectors;

        public RNumericPanel(REngine engine, int length, string[] names, int width = 0) {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            _engine = engine;
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            _length = length;

            if (names == null) {
                if (width <= 0) {
                    throw new ArgumentOutOfRangeException(nameof(width));
                }
                names = new string[width];
                for (int i = 0; i < width; i++) {
                    names[i] = $"C{i}";
                }
            } else {
                if (width <= 0) {
                    width = names.Length;
                }
                if (names.Length > width) {
                    throw new ArgumentException("More names than columns");
                }
            }
            _width = names.Length;
            _names = names;
            _numericVectors = new NumericVector[_width];
            for (int i = 0; i < _width; i++) {
                _numericVectors[i] = new NumericVector(engine, length);
            }
            _ticks = new NumericVector(engine, length);
            _panel = new GenericVector(engine, _numericVectors);
            _panel.SetNames(_names);
            _panel.SetAttribute("TimeStamp", _ticks);
        }

        public GenericVector AsDataFrame
        {
            get { return _panel; }
        }


        /// <summary>
        /// Return number os seconds as double
        /// </summary>
        public static double DateTimeToUnixTimestamp(DateTime dateTime) {
            // ReSharper disable once PossibleLossOfFraction
            return 0.001 * ((dateTime.Ticks - 621355968000000000L) //new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).Ticks)
                / TimeSpan.TicksPerMillisecond);
        }


        // NB with the current simple implementation we do not need a separate project at all
        // However, having access to internals is good and at least it is not a blackbox
        // We could use Spreads.R project as a place for default implementations, e.g. linear
        // gegreassion, etc.

        // Also, we really do not want to program in R from F#. We only want to call it.
        // On R side, we could call Spreads and get exacty the same objects as we construct here.
        // In doing so, we could completely separate two environments and separate people could 
        // do exploratory work in R.

        public void CopySeries(Series<DateTime, double>[] series) {
            if (series.Length != _numericVectors.Length) throw new ArgumentException("Wrong number of series");
            // TODO (perf) chck if series are SortedMaps and their keys are equal 
            // - then we could just copy memory direclty without iterations
            // Also, we need a sliding materialized panel, it could be implemented similarly to
            // SortedSequeMap, but keys and values should be separate, and SortedDeque method
            // should be internal and always return offsets so that we could access value buffers.

            var c = 0;
            foreach (var row in series.Zip((k, vArr) => vArr)) {
                _ticks[c] = DateTimeToUnixTimestamp(row.Key);
                for (int column = 0; column < _width; column++) {
                    _numericVectors[column][c] = row.Value[column];
                }
                c++;
                if (c == _length) break;
            }
        }

        public void Dispose() {
            foreach (var vector in _numericVectors) {
                vector.Dispose();
            }
        }
    }
}
