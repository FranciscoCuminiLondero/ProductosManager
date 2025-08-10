namespace ProductosManager.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required decimal Precio { get; set; }
    }
}
