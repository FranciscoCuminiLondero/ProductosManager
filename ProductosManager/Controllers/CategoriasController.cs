using Microsoft.AspNetCore.Mvc;
using ProductosManager.Entities;

namespace ProductosManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private static List<Producto> productList = new List<Producto>()
        {
            new Producto(1,"Chomba Blanca", 20000.00m, 4, new Categoria(1,"Chombas")),
            new Producto(2,"Pantalón Azul", 25000.00m, 2, new Categoria(2,"Pantalones")),
            new Producto(3,"Bermuda Lila", 30000.00m, 1, new Categoria(3,"Bermudas"))
        };

        private static List<Categoria> categoryList = new List<Categoria>()
        {
            new Categoria(1,"Chombas"),
            new Categoria(2, "Pantalones"),
            new Categoria(3, "Bermudas"),
            new Categoria(4, "Calzados")

        };
        //GET api/categorias
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetCategoria()
        {
            return Ok(categoryList);
        }
        //GET api/categorias/:id
        [HttpGet("{id}")]
        public ActionResult<List<Categoria>> GetCategoryById([FromRoute] int id)
        {
            var categorias = categoryList.FirstOrDefault(c => c.Id == id);

            if (categorias == null)
            {
                return NotFound($"Categoria con {id} no encontrada");
            }

            return Ok(categorias);
        }

        //POST api/categorias
        [HttpPost]
        public ActionResult<List<Categoria>> CreateCategory([FromBody] Categoria categoria)
        {
            if (string.IsNullOrEmpty(categoria.Nombre))
            {
                return BadRequest("El nombre no puede estar vacío");
            }

            if (categoryList.Any())
            {
                categoria.Id = categoryList.Max(c => c.Id) + 1;
            }
            else
            {
                categoria.Id = 1;
            }

            var nuevaCategoria = new Categoria(categoria.Id, categoria.Nombre);
            categoryList.Add(nuevaCategoria);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = nuevaCategoria.Id, nombre = nuevaCategoria.Nombre },
                nuevaCategoria
            );
        }
        //PUT api/categorias/:id  (si tiene productos Conflict)
        [HttpPut("{id}")]
        public ActionResult UpdateCategory([FromRoute] int id, [FromBody] Categoria categoria)
        {
            var categoriaExistente = categoryList.FirstOrDefault(c => c.Id == id);

            if(categoriaExistente == null)
            {
                return NotFound($"Categoría con ID {id} no encontrada");
            }

            bool tieneProductos = productList.Any(p => p.Categoria != null && p.Categoria.Id == id);
            if (tieneProductos)
            {
                return Conflict("La categoría tiene productos asociados. Debe quitar la asignación antes de modificarla.");
            }

            if(string.IsNullOrEmpty(categoria.Nombre))
            {
                return BadRequest("El nombre no puede estar vacío.");
            }

            categoriaExistente.Nombre = categoria.Nombre;
            return NoContent();
        }
        //DELETE api/categorias/:id (si tiene productos Conflict)
        [HttpDelete("{id}")]
        public ActionResult DeleteCategory([FromRoute] int id) 
        {
            var categoriaExistente = categoryList.FirstOrDefault(c=>c.Id == id);

            if(categoriaExistente == null )
            {
                return NotFound($"Categoria con ID {id} no encontrada");
            }


            bool tieneProductos = productList.Any(p => p.Categoria != null && p.Categoria.Id == id);
            if (tieneProductos)
            {
                return Conflict("La categoría tiene productos asociados. Debe quitar la asignación antes de eliminarla.");
            }

            categoryList.Remove(categoriaExistente);
            return NoContent();
        }
    }
};
