using Microsoft.AspNetCore.Mvc;
using ProductosManager.Contracts;
using ProductosManager.Entities;

namespace ProductosManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private static List<Producto> productList = new List<Producto>()
        {
            new Producto(1,"Chomba", 20000.00m, 4),
            new Producto(2,"Pantalón", 25000.00m, 2),
            new Producto(3,"Bermuda", 30000.00m, 1)
        };

        //GET api/productos
        [HttpGet]
        public ActionResult<List<Producto>> GetAll()
        {
            var listaProductos = productList.ToList();

            return Ok(listaProductos);
        }

        //GET api/productos/:id
        [HttpGet("{id}")]
        public ActionResult<List<Producto>> GetById([FromRoute] int id)
        {
            var producto = productList.FirstOrDefault(p => p.Id == id);

            if (producto == null) {
                return NotFound($"No se encontró un producto con ID {id}");
            }

            return Ok(producto);
        }

        //POST api/productos
        [HttpPost]
        public ActionResult<List<ProductoResponse>> Create([FromBody] ProductoRequest producto)
        {
            if (string.IsNullOrEmpty(producto.Nombre))
            {
                return BadRequest("El nombre es requerido.");
            }

            if (producto.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor a 0.");
            }

            if(productList.Any())
            {
                producto.Id = productList.Max(p => p.Id) + 1;
            } else
            {
                producto.Id = 1;
            }

            int stock = producto.Stock == null ? 10 : producto.Stock.Value;

            var nuevoProducto = new Producto(producto.Id, producto.Nombre, producto.Precio, stock);

            productList.Add(nuevoProducto);

            var devolverProducto = new ProductoResponse()
            {
                Id = nuevoProducto.Id,
                Nombre = nuevoProducto.Nombre,
                Precio = nuevoProducto.Precio
            };

            return CreatedAtAction(nameof(GetById), new { id = devolverProducto.Id }, devolverProducto);
        }

        //PUT api/productos/:id
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] ProductoRequest producto)
        {
            var productoExistente = productList.FirstOrDefault(p => p.Id == id);

            if(productoExistente == null)
            {
                return NotFound($"No se encontró producto con ID {id} para actualizar");
            }

            if (string.IsNullOrEmpty(producto.Nombre))
            {
                return BadRequest("El nombre es requerido");
            }

            if(producto.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor a 0");
            }

            if(producto.Stock < 0)
            {
                return BadRequest("El stock no puede ser menor a 0");
            }

            productoExistente.Nombre = producto.Nombre;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock ?? 0;

            return NoContent();
        }

        //DELETE api/productos/:id
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var productoExistente = productList.FirstOrDefault(p => p.Id == id);

            if(productoExistente == null)
            {
                return NotFound($"No se encontró producto con ID {id} para eliminar");

            }

            productList.Remove(productoExistente);

            return NoContent();
        }
    }
}
