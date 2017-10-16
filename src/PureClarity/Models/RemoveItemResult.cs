namespace PureClarity.Models
{
    public class RemoveItemResult<T> : PCResultBase
    {
        public string Error;
        public T Item;
    }
}