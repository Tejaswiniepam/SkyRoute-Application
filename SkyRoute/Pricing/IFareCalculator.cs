namespace SkyRoute.Pricing
{
    public interface IFareCalculator
    {
        decimal CalculateBaseFare(int distanceMiles, CabinClass cabinClass);

        decimal CalculateFinalFare(
            string airline,
            int distanceMiles,
            CabinClass cabinClass);

        decimal CalculateAdjustment(
            string airline,
            int distanceMiles,
            CabinClass cabinClass);
    }
}