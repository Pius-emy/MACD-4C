using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class MACD4C : Indicator
    {
        [Output("Bull Up", IsHistogram = true, PlotType = PlotType.Histogram, LineColor = "Lime", Thickness = 3)]
        public IndicatorDataSeries StrongBullish { get; set; }

        [Output("Bull Down", IsHistogram = true, PlotType = PlotType.Histogram, LineColor = "Green", Thickness = 3)]
        public IndicatorDataSeries WeakBulish { get; set; }

        [Output("Bear Down", IsHistogram = true, PlotType = PlotType.Histogram, LineColor = "Maroon", Thickness = 3)]
        public IndicatorDataSeries StrongBearish { get; set; }

        [Output("Bear Up", IsHistogram = true, PlotType = PlotType.Histogram, LineColor = "Red", Thickness = 3)]
        public IndicatorDataSeries WeakBearish { get; set; }


        [Parameter("SlowMA", DefaultValue = 26, MinValue = 7)]
        public int SlowMA { get; set; }

        [Parameter("FastMA", DefaultValue = 12, MinValue = 7)]
        public int FastMA { get; set; }

        private int SignalPeriod = 9;
        private MacdCrossOver macd;


        protected override void Initialize()
        {
            this.macd = this.Indicators.MacdCrossOver(source: this.Bars.ClosePrices, longCycle: this.SlowMA, shortCycle: this.FastMA, signalPeriods: this.SignalPeriod);
        }

        public override void Calculate(int index)
        {
            var currentMacd = this.macd.MACD.LastValue;
            var previousMacd = this.macd.MACD.Last(1);

            if (currentMacd > 0)
            {
                if (currentMacd > previousMacd)
                {
                    this.StrongBullish[index] = currentMacd;
                }
                else if (currentMacd < previousMacd)
                {
                    this.WeakBulish[index] = currentMacd;
                }
            }
            else if (currentMacd < 0)
            {
                if (currentMacd < previousMacd)
                {
                    this.StrongBearish[index] = currentMacd;
                }
                else if (currentMacd > previousMacd)
                {
                    this.WeakBearish[index] = currentMacd;
                }
            }
        }
    }
}
