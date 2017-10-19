using Newtonsoft.Json;

namespace PureClarity.Models
{
    public class ProductPrice
    {
        public string Currency;
        public decimal Price;

        public ProductPrice(decimal price, string currency)
        {
            Price = price;
            Currency = currency ?? throw new System.ArgumentNullException(nameof(currency));
        }
    }
}