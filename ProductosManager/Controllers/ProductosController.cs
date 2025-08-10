using Microsoft.AspNetCore.Mvc;
using ProductosManager.Entities;

namespace ProductosManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        //GET api/productos
        [HttpGet]
        public IActionResult Get()
        {
            //Lista fija de productos de ejemplo
            var productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Chomba", Precio = 20000.00m},
                new Producto { Id = 2, Nombre = "Pantalon", Precio = 25000.00m}
            };

            return Ok(productos);
        }

        //GET api/productos/:id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Chomba", Precio = 20000.00m},
                new Producto { Id = 2, Nombre = "Pantalon", Precio = 25000.00m}
            };

            var producto = productos.FirstOrDefault(p => p.Id == id);

            if (producto == null) {
                return NotFound($"No se encontró un producto con ID {id}");
            }

            return Ok(producto);
        }

        //POST api/productos
        [HttpPost]
        public IActionResult Post([FromBody] Producto nuevoProducto)
        {
            var productos = new List<Producto>
            {
                 new Producto { Id = 1, Nombre = "Chomba", Precio = 20000.00m},
                 new Producto { Id = 2, Nombre = "Pantalon", Precio = 25000.00m}
            };

            int nuevoId = productos.Count > 0 ? productos.Max(p => p.Id) + 1 : 1;
            nuevoProducto.Id = nuevoId;

            if (string.IsNullOrEmpty(nuevoProducto.Nombre))
            {
                return BadRequest("El nombre es requerido.");
            }

            if (nuevoProducto.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor a 0.");
            }

            return CreatedAtAction(nameof(Get), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        //PUT api/productos/:id
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Producto productoActualizar)
        {
            var productos = new List<Producto>
            {
                 new Producto { Id = 1, Nombre = "Chomba", Precio = 20000.00m},
                 new Producto { Id = 2, Nombre = "Pantalon", Precio = 25000.00m}
            };

            var producto = productos.FirstOrDefault(p => p.Id == id);

            if(producto == null)
            {
                return NotFound($"No se encontró producto con ID {id} para actualizar");
            }

            if (string.IsNullOrEmpty(productoActualizar.Nombre))
            {
                return BadRequest("El nombre es requerido");
            }

            if(productoActualizar.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor a 0");
            }

            producto.Nombre = productoActualizar.Nombre;
            producto.Precio = productoActualizar.Precio;

            return NoContent();
        }

        //DELETE api/productos/:id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var productos = new List<Producto>
            {
                 new Producto { Id = 1, Nombre = "Chomba", Precio = 20000.00m},
                 new Producto { Id = 2, Nombre = "Pantalon", Precio = 25000.00m}
            };

            var producto = productos.FirstOrDefault(p => p.Id == id);

            if(producto == null)
            {
                return NotFound($"No se encontró producto con ID {id} para eliminar");

            }

            productos.Remove(producto);

            return NoContent();
        }
    }
}
