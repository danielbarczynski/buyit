namespace buyitWeb.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<CartModel> Cart { get; set; }
        //public double CartTotal { get; set; }
        public OrderHeaderModel OrderHeader { get; set; }
    }
}
