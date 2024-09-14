namespace BookStoreAPI.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public int Quantity { get; set; }
    }
}
