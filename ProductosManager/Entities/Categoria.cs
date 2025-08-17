namespace ProductosManager.Entities
{
    public class Categoria
    {
        public Categoria(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
