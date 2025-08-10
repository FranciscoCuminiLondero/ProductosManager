namespace ProductosManager.Entities
{
    public class Producto(int id, string nombre, decimal precio, int stock)
    {
        public int Id { get; set; } = id;
        public string Nombre { get; set; } = nombre;
        public decimal Precio { get; set; } = precio;
        public int Stock { get; set; } = stock;
    }
}
