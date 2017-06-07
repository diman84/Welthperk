namespace Wealthperk.ViewModel.Account
{
    public class Forecast
    {
        public string currentAmount ;
        public string currentAge ;
        public string byAmount ;
        public string byAge ;
        public ChartPoint[] forecast;
        public bool forRetirement;
    }

    public class ChartPointReal : ChartPoint
    {
        public double y;
    }

    public class ChartPoint
    {
        public int x;
        public double z;
        public string label ;

    }
}