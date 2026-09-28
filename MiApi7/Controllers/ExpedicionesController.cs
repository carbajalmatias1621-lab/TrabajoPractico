using Microsoft.AspNetCore.Mvc;
using TP.Api.Models;

namespace TP.Api.Controllers;

[ApiController, Route("api/expediciones")]
public class ExpedicionesController : ControllerBase
{
    private static int IdExpedicionNuevo() => Datos.Expediciones.Select(e => e.Id).DefaultIfEmpty().Max() + 1;
    private static int IdExploradorNuevo() => Datos.Exploradores.Select(e => e.Id).DefaultIfEmpty().Max() + 1;
    private static int IdParticipacionNueva() => Datos.Participaciones.Select(p => p.Id).DefaultIfEmpty().Max() + 1;

    [HttpGet()]
    public IActionResult Todas()
    {
        var resultado = Datos.Expediciones.OrderBy(x => x.FechaInicio);

        return Ok(resultado);
    }

    [HttpGet("({id:int})")]
    public IActionResult PorId(int id)
    {
        var x = Datos.Expediciones.FirstOrDefault(x => x.Id == id);

        return x is null ? NotFound("Expedicion no encontrada.") : Ok(x);
    }

    [HttpPost()]
    public IActionResult Crear(Expedicion x)
    {
        if (x.FechaFin < x.FechaInicio)
            return BadRequest("FechaFin no puede ser anterior a FechaInicio.");

        if (x.Capacidad <= 0)
            return BadRequest("La capacidad debe ser positiva.");

        x.Id = IdExpedicionNuevo();

        Datos.Expediciones.Add(x);

        return CreatedAtAction(nameof(PorId), new { id = x.Id }, x);
    }

    [HttpPut("({id:int})")]
    public IActionResult Modificar(int id, Expedicion e)
    {
        var x = Datos.Expediciones.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Expedicion no encontrada.");

        if (e.FechaFin < e.FechaInicio || e.Capacidad <= 0)
            return BadRequest("Fechas o capacidad invalidas.");

        if (Datos.Participaciones.Count(p => p.ExpedicionId == id) > e.Capacidad)
            return Conflict("La capacidad no puede ser menor que los exploradores ya asignados.");

        x.Nombre = e.Nombre;

        x.Destino = e.Destino;

        x.FechaInicio = e.FechaInicio;

        x.FechaFin = e.FechaFin;

        x.Capacidad = e.Capacidad;

        x.Estado = e.Estado;

        return Ok(x);
    }

    [HttpDelete("({id:int})")]
    public IActionResult Eliminar(int id)
    {
        var x = Datos.Expediciones.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Expedicion no encontrada.");

        Datos.Expediciones.Remove(x);

        Datos.Participaciones.RemoveAll(p => p.ExpedicionId == id);

        return NoContent();
    }

    [HttpPost("exploradores")]
    public IActionResult CrearExplorador(Explorador x)
    {
        if (string.IsNullOrWhiteSpace(x.Nombre) || x.ExperienciaAnios < 0)
            return BadRequest("Datos invalidos.");

        x.Id = IdExploradorNuevo();

        Datos.Exploradores.Add(x);

        return Created("", x);
    }

    [HttpPost("participaciones")]
    public IActionResult Asignar(Participacion x)
    {
        var e = Datos.Expediciones.FirstOrDefault(e => e.Id == x.ExpedicionId);

        var p = Datos.Exploradores.FirstOrDefault(p => p.Id == x.ExploradorId);

        if (e is null || p is null)
            return NotFound("Expedicion o explorador inexistente.");

        if (!p.Disponible)
            return BadRequest("El explorador no esta disponible.");

        if (Datos.Participaciones.Any(a => a.ExpedicionId == e.Id && a.ExploradorId == p.Id))
            return Conflict("El explorador ya esta asignado.");

        if (Datos.Participaciones.Count(a => a.ExpedicionId == e.Id) >= e.Capacidad)
            return Conflict("La expedicion esta completa.");

        bool superpone = Datos
            .Participaciones.Where(a => a.ExploradorId == p.Id)
            .Join(Datos.Expediciones, a => a.ExpedicionId, otra => otra.Id, (a, otra) => otra)
            .Any(otra => e.FechaInicio <= otra.FechaFin && e.FechaFin >= otra.FechaInicio);

        if (superpone)
            return Conflict("Las fechas se superponen con otra expedicion.");

        x.Id = IdParticipacionNueva();

        Datos.Participaciones.Add(x);

        return Created("", x);
    }

    [HttpGet("({id:int})/integrantes")]
    public IActionResult Integrantes(int id)
    {
        var resultado = Datos
            .Participaciones.Where(x => x.ExpedicionId == id)
            .Join(
                Datos.Exploradores,
                x => x.ExploradorId,
                e => e.Id,
                (x, e) => new { Explorador = e, x.Rol }
            );

        return Ok(resultado);
    }

    [HttpGet("exploradores/({id:int})/expediciones")]
    public IActionResult ExpedicionesDe(int id)
    {
        var resultado = Datos
            .Participaciones.Where(x => x.ExploradorId == id)
            .Join(Datos.Expediciones, x => x.ExpedicionId, e => e.Id, (x, e) => e);

        return Ok(resultado);
    }

    [HttpGet("exploradores/especialidad/({valor})")]
    public IActionResult Especialidad(string valor)
    {
        var resultado = Datos.Exploradores.Where(x =>
            x.Especialidad.Contains(valor, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("exploradores/disponibles")]
    public IActionResult Disponibles()
    {
        var resultado = Datos.Exploradores.Where(x => x.Disponible);

        return Ok(resultado);
    }

    [HttpGet("activas")]
    public IActionResult Activas()
    {
        var resultado = Datos.Expediciones.Where(x =>
            x.Estado.Equals("Activa", StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("futuras")]
    public IActionResult Futuras()
    {
        var resultado = Datos.Expediciones.Where(x => x.FechaInicio > DateTime.Now);

        return Ok(resultado);
    }

    [HttpGet("completas")]
    public IActionResult Completas()
    {
        var resultado = Datos.Expediciones.Where(e =>
            Datos.Participaciones.Count(x => x.ExpedicionId == e.Id) >= e.Capacidad
        );

        return Ok(resultado);
    }

    [HttpGet("ocupacion")]
    public IActionResult Ocupacion()
    {
        var resultado = Datos.Expediciones.Select(e => new
        {
            Expedicion = e,
            Cantidad = Datos.Participaciones.Count(x => x.ExpedicionId == e.Id),
            Porcentaje = e.Capacidad == 0
                ? 0
                : Datos.Participaciones.Count(x => x.ExpedicionId == e.Id) * 100.0 / e.Capacidad,
        });

        return Ok(resultado);
    }

    [HttpGet("exploradores/ranking")]
    public IActionResult Ranking()
    {
        var resultado = Datos
            .Exploradores.Select(e => new
            {
                Explorador = e,
                Cantidad = Datos.Participaciones.Count(x => x.ExploradorId == e.Id),
            })
            .OrderByDescending(x => x.Cantidad);

        return Ok(resultado);
    }

    [HttpGet("especialidades/ranking")]
    public IActionResult Especialidades()
    {
        var resultado = Datos
            .Participaciones.Join(
                Datos.Exploradores,
                p => p.ExploradorId,
                e => e.Id,
                (p, e) => e.Especialidad
            )
            .GroupBy(x => x)
            .Select(g => new { Especialidad = g.Key, Cantidad = g.Count() })
            .OrderByDescending(x => x.Cantidad);

        return Ok(resultado);
    }
}
