namespace PureClarity.Models
{
    public class DeletedProductSku : PCModelBase
    {
        public string Sku { get => Id; set => Id = value; }

        public DeletedProductSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new System.ArgumentException($"{nameof(sku)} is a mandatory field and must be populated", nameof(sku));
            }

            Sku = sku;
        }
    }
}