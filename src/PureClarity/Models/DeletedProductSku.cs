namespace PureClarity.Models
{
    public class DeletedProductSku : PCModelBase
    {
        public string Sku { get => Id; set => Id = value; }

        public DeletedProductSku(string sku){
            Sku = sku;
        }
    }
}