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
            new Producto(1,"Chomba Blanca", 20000.00m, 4, null),
            new Producto(2,"Pantalón Azul", 25000.00m, 2, null),
            new Producto(3,"Bermuda Lila", 30000.00m, 1, null)
        };

        private static List<Categoria> categoryList = new List<Categoria>()
        {
            new Categoria(1,"Chombas"),
            new Categoria(2, "Pantalones"),
            new Categoria(3, "Bermudas"),
            new Categoria(4, "Calzados")

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

            if (producto == null)
            {
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

            if (producto.Stock < 0)
            {
                return BadRequest("El stock no puede ser menor a 0.");
            }

            if (productList.Any())
            {
                producto.Id = productList.Max(p => p.Id) + 1;
            }
            else
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

            if (productoExistente == null)
            {
                return NotFound($"No se encontró producto con ID {id} para actualizar");
            }

            if (string.IsNullOrEmpty(producto.Nombre))
            {
                return BadRequest("El nombre es requerido");
            }

            if (producto.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor a 0");
            }

            if (producto.Stock < 0)
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

            if (productoExistente == null)
            {
                return NotFound($"No se encontró producto con ID {id} para eliminar");

            }

            productList.Remove(productoExistente);

            return NoContent();
        }

        //GET api/productos/buscar?nombre=texto
        [HttpGet("buscar")]
        public ActionResult<List<Producto>> BuscarPorNombre([FromQuery] string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return Ok(new List<Producto>());
            }

            var resultados = productList
                .Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(resultados);
        }


        // GET api/productos/precio-minimo/:valor
        [HttpGet("precio-minimo/{valor}")]
        public ActionResult<List<Producto>> ObtenerPorPrecioMinimo([FromRoute] decimal valor)
        {
            var productosFiltrados = productList
                .Where(p => p.Precio >= valor)
                .ToList();

            return Ok(productosFiltrados);
        }

        // GET api/productos/total
        [HttpGet("total")]
        public ActionResult<List<Producto>> ObtenerCantidadTotal()
        {
            int totalProductos = productList.Count();
            return Ok(totalProductos);
        }

        // PATCH api/productos/:id
        [HttpPatch("{id}")]
        public ActionResult<List<Producto>> ActualizarParcial([FromRoute] int id, [FromBody] ProductoPatchRequest productoPatch)
        {
            var productoExistente = productList.FirstOrDefault(p => p.Id == id);

            if (productoExistente == null )
            {
                return NotFound($"Producto para actualizar con id {id} no encontrado");
            }

            if (productoPatch.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor a 0");
            }

            if (productoPatch.Stock < 0)
            {
                return BadRequest("El stock debe ser mayor a 0");
            }

            productoExistente.Precio = productoPatch.Precio;
            productoExistente.Stock = productoPatch.Stock;

            return NoContent();
        }

        //PUT api/productos/:id/categoria/:categoriaId
        [HttpPut("{id}/categoria/{categoriaId}")]
        public ActionResult AsociarCategoria([FromRoute] int id, [FromRoute] int categoriaId )
        {
            var producto = productList.FirstOrDefault(p => p.Id == id);
            if(producto == null)
            {
                return NotFound($"Producto con Id {id} no encontrado");
            }

            var categoria = categoryList.FirstOrDefault(c => c.Id == categoriaId);
            if(categoria == null)
            {
                return NotFound($"Categoria con ID {id} no encontrada");
            }

            producto.Categoria = categoria;
            return Ok($"Categoría '{categoria.Nombre}' asignada al producto '{producto.Nombre}'");
        }

        //DELETE api/productos/:id/categoria/:categoriaId
        [HttpDelete("{id}/categoria")]
        public ActionResult DesasociarCategoria([FromRoute] int id)
        {
            var producto = productList.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound($"Producto con ID {id} no encontrado");
            }

            if (producto.Categoria == null)
            {
                return BadRequest("El producto no tiene ninguna categoría asociada");
            }

            producto.Categoria = null;

            return Ok($"Categoría desasociada del producto '{producto.Nombre}'");
        }
    }
}
