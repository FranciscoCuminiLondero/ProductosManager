namespace ProductosManager.Entities
{
    public class Producto
    {
        public Producto(int id, string nombre, decimal precio, int stock = 10, Categoria? categoria = null)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            Categoria = categoria;
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; } = 10;
        public Categoria? Categoria { get; set; }
    }
}
