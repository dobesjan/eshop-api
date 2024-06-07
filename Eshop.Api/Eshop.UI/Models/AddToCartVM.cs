namespace Eshop.UI.Models
{
	public class AddToCartVM
	{
		public int ProductId { get; set; }
		public int Count { get; set; } = 1;
		public string FormId { get; set; }
	}
}
