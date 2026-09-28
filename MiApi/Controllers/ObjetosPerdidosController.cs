using Microsoft.AspNetCore.Mvc;
using TP.Api.Models;

namespace TP.Api.Controllers;

[ApiController, Route("api/objetosperdidos")]
public class ObjetosPerdidosController : ControllerBase
{
    private static int SiguienteId() => Datos.Objetos.Select(o => o.Id).DefaultIfEmpty().Max() + 1;

    [HttpGet()]
    public IActionResult Todos()
    {
        var resultado = Datos.Objetos.OrderByDescending(x => x.FechaEncontrado);

        return Ok(resultado);
    }

    [HttpGet("({id:int})")]
    public IActionResult PorId(int id)
    {
        var x = Datos.Objetos.FirstOrDefault(x => x.Id == id);

        return x is null ? NotFound("Objeto no encontrado.") : Ok(x);
    }

    [HttpPost()]
    public IActionResult Crear(ObjetoPerdido x)
    {
        if (string.IsNullOrWhiteSpace(x.Descripcion))
            return BadRequest("La descripcion es obligatoria.");

        if (x.FechaEncontrado > DateTime.Now)
            return BadRequest("La fecha no puede ser futura.");

        x.Id = SiguienteId();

        Datos.Objetos.Add(x);

        return CreatedAtAction(nameof(PorId), new { id = x.Id }, x);
    }

    [HttpPut("({id:int})")]
    public IActionResult Modificar(int id, ObjetoPerdido entrada)
    {
        var x = Datos.Objetos.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Objeto no encontrado.");

        if (
            string.IsNullOrWhiteSpace(entrada.Descripcion)
            || entrada.FechaEncontrado > DateTime.Now
        )
            return BadRequest("Datos invalidos.");

        x.Descripcion = entrada.Descripcion;

        x.Categoria = entrada.Categoria;

        x.LugarEncontrado = entrada.LugarEncontrado;

        x.FechaEncontrado = entrada.FechaEncontrado;

        return Ok(x);
    }

    [HttpDelete("({id:int})")]
    public IActionResult Eliminar(int id)
    {
        var x = Datos.Objetos.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Objeto no encontrado.");

        Datos.Objetos.Remove(x);

        return NoContent();
    }

    [HttpGet("buscar")]
    public IActionResult Buscar(string descripcion)
    {
        var resultado = Datos.Objetos.Where(x =>
            x.Descripcion.Contains(descripcion, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("categoria/({categoria})")]
    public IActionResult Categoria(string categoria)
    {
        var resultado = Datos.Objetos.Where(x =>
            x.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("no-reclamados")]
    public IActionResult NoReclamados()
    {
        var resultado = Datos.Objetos.Where(x => !x.Reclamado);

        return Ok(resultado);
    }

    [HttpGet("posteriores")]
    public IActionResult Posteriores(DateTime fecha)
    {
        var resultado = Datos
            .Objetos.Where(x => x.FechaEncontrado > fecha)
            .OrderByDescending(x => x.FechaEncontrado);

        return Ok(resultado);
    }

    [HttpGet("cantidad")]
    public IActionResult Cantidad()
    {
        var resultado = Datos.Objetos.Count();

        return Ok(resultado);
    }

    [HttpPatch("({id:int})/reclamar")]
    public IActionResult Reclamar(int id, [FromBody] string nombre)
    {
        var x = Datos.Objetos.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Objeto no encontrado.");

        if (x.Reclamado)
            return Conflict("El objeto ya fue reclamado.");

        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest("Debe indicar quien retiro el objeto.");

        x.Reclamado = true;

        x.NombrePersonaQueRetiro = nombre;

        return Ok(x);
    }
}
