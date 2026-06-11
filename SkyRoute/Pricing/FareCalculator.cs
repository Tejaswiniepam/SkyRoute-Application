namespace SkyRoute.Pricing
{
    public class FareCalculator : IFareCalculator
    {
        private const decimal EconomyRatePerMile = 0.15m;
        private const decimal BusinessRatePerMile = 0.35m;
        private const decimal FirstClassRatePerMile = 0.65m;

        private const decimal FuelSurchargeRate = 0.15m;

        private const decimal DiscountRate = 0.10m;
        private const decimal MinFinalPrice = 29.99m;

        public decimal CalculateBaseFare(
            int distanceMiles,
            CabinClass cabinClass)
        {
            return cabinClass switch
            {
                CabinClass.Economy =>
                    Math.Round(distanceMiles * EconomyRatePerMile, 2),

                CabinClass.Bussiness =>
                    Math.Round(distanceMiles * BusinessRatePerMile, 2),

                CabinClass.FirstClass =>
                    Math.Round(distanceMiles * FirstClassRatePerMile, 2),

                _ =>
                    Math.Round(distanceMiles * EconomyRatePerMile, 2)
            };
        }

        public decimal CalculateAdjustment(
            string airline,
            int distanceMiles,
            CabinClass cabinClass)
        {
            var baseFare = CalculateBaseFare(distanceMiles, cabinClass);

            return airline switch
            {
                "GlobalAir" =>
                    Math.Round(baseFare * FuelSurchargeRate, 2),

                "BudgetWings" =>
                    Math.Round(baseFare * DiscountRate, 2),

                _ => 0
            };
        }

        public decimal CalculateFinalFare(
            string airline,
            int distanceMiles,
            CabinClass cabinClass)
        {
            var baseFare = CalculateBaseFare(distanceMiles, cabinClass);

            return airline switch
            {
                "GlobalAir" =>
                    baseFare +
                    Math.Round(baseFare * FuelSurchargeRate, 2),

                "BudgetWings" =>
                    Math.Max(
                        MinFinalPrice,
                        baseFare -
                        Math.Round(baseFare * DiscountRate, 2)
                    ),

                _ => baseFare
            };
        }
    }
}