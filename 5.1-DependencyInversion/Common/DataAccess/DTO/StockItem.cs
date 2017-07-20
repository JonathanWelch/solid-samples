namespace _5._1_DependencyInversion.Common.DataAccess.DTO
{
    public class StockItem
    {
        private string _sku;

        public StockItem() // TODO: Remove usages of the empty contrsuctor. You should have a _sku and an available quantity to create a stock item
        {
        }

        public StockItem(string sku, int available)
        {
            Sku = sku;
            Available = available;
        }

        public string Sku
        {
            get { return this._sku; }
            set { this._sku = value.Trim(); }
        }

        public int Available { get; set; }
    }
}
