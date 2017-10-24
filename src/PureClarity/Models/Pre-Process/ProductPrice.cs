using Newtonsoft.Json;

namespace PureClarity.Models
{
    public class Price
    {
        public string Currency;
        public decimal Value;

        public Price(decimal value, string currency)
        {
            Value = value;
            Currency = currency ?? throw new System.ArgumentNullException(nameof(currency));
        }
    }
}