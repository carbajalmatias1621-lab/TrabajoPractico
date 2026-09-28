using Microsoft.AspNetCore.Mvc;
using TP.Api.Models;

namespace TP.Api.Controllers;

[ApiController, Route("api/drones")]
public class DronesController : ControllerBase
{
    private static int IdDroneNuevo() => Datos.Drones.Select(d => d.Id).DefaultIfEmpty().Max() + 1;
    private static int IdMisionNueva() => Datos.Misiones.Select(m => m.Id).DefaultIfEmpty().Max() + 1;

    [HttpGet()]
    public IActionResult Todos()
    {
        var resultado = Datos.Drones;

        return Ok(resultado);
    }

    [HttpGet("({id:int})")]
    public IActionResult PorId(int id)
    {
        var x = Datos.Drones.FirstOrDefault(x => x.Id == id);

        return x is null ? NotFound("Drone no encontrado.") : Ok(x);
    }

    [HttpPost()]
    public IActionResult Crear(Drone x)
    {
        if (string.IsNullOrWhiteSpace(x.Codigo) || string.IsNullOrWhiteSpace(x.Modelo))
            return BadRequest("El codigo y el modelo son obligatorios.");

        if (Datos.Drones.Any(d => d.Codigo.Equals(x.Codigo, StringComparison.OrdinalIgnoreCase)))
            return Conflict("Ya existe un drone con ese codigo.");

        if (x.Bateria < 0 || x.Bateria > 100)
            return BadRequest("La bateria debe estar entre 0 y 100.");

        x.Id = IdDroneNuevo();

        Datos.Drones.Add(x);

        return CreatedAtAction(nameof(PorId), new { id = x.Id }, x);
    }

    [HttpPut("({id:int})")]
    public IActionResult Modificar(int id, Drone e)
    {
        var x = Datos.Drones.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Drone no encontrado.");

        if (e.Bateria < 0 || e.Bateria > 100)
            return BadRequest("Bateria invalida.");

        x.Codigo = e.Codigo;

        x.Modelo = e.Modelo;

        x.Bateria = e.Bateria;

        x.Estado = e.Estado;

        return Ok(x);
    }

    [HttpDelete("({id:int})")]
    public IActionResult Eliminar(int id)
    {
        var x = Datos.Drones.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Drone no encontrado.");

        Datos.Drones.Remove(x);

        return NoContent();
    }

    [HttpGet("disponibles")]
    public IActionResult Disponibles()
    {
        var resultado = Datos.Drones.Where(x =>
            x.Estado.Equals("Disponible", StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("bateria-inferior/({porcentaje:int})")]
    public IActionResult Bateria(int porcentaje)
    {
        var resultado = Datos.Drones.Where(x => x.Bateria < porcentaje);

        return Ok(resultado);
    }

    [HttpGet("modelo/({modelo})")]
    public IActionResult Modelo(string modelo)
    {
        var resultado = Datos.Drones.Where(x =>
            x.Modelo.Contains(modelo, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpPost("misiones")]
    public IActionResult Asignar(Mision x)
    {
        var d = Datos.Drones.FirstOrDefault(d => d.Id == x.DroneId);

        if (d is null)
            return NotFound("El drone no existe.");

        if (!d.Estado.Equals("Disponible", StringComparison.OrdinalIgnoreCase))
            return BadRequest("El drone no esta disponible.");

        if (d.Bateria < 30)
            return BadRequest("El drone necesita al menos 30 de bateria.");

        if (Datos.Misiones.Any(m => m.DroneId == d.Id && !m.Completada))
            return Conflict("El drone ya tiene una mision activa.");

        x.Id = IdMisionNueva();

        x.Fecha = x.Fecha == default ? DateTime.Now : x.Fecha;

        Datos.Misiones.Add(x);

        d.Estado = "EnMision";

        return Created("", x);
    }

    [HttpPatch("misiones/({id:int})/finalizar")]
    public IActionResult Finalizar(int id)
    {
        var m = Datos.Misiones.FirstOrDefault(x => x.Id == id);

        if (m is null)
            return NotFound("Mision no encontrada.");

        if (m.Completada)
            return Conflict("La mision ya finalizo.");

        m.Completada = true;

        var d = Datos.Drones.FirstOrDefault(x => x.Id == m.DroneId);

        if (d is not null)
            d.Estado = "Disponible";

        return Ok(m);
    }

    [HttpGet("({id:int})/misiones")]
    public IActionResult Misiones(int id)
    {
        var resultado = Datos.Misiones.Where(x => x.DroneId == id);

        return Ok(resultado);
    }

    [HttpGet("distancias")]
    public IActionResult Distancias()
    {
        var resultado = Datos.Drones.Select(d => new
        {
            Drone = d,
            TotalKm = Datos
                .Misiones.Where(m => m.DroneId == d.Id && m.Completada)
                .Sum(m => m.DistanciaKm),
        });

        return Ok(resultado);
    }

    [HttpGet("mayor-distancia")]
    public IActionResult Mayor()
    {
        var resultado = Datos
            .Drones.Select(d => new
            {
                Drone = d,
                TotalKm = Datos
                    .Misiones.Where(m => m.DroneId == d.Id && m.Completada)
                    .Sum(m => m.DistanciaKm),
            })
            .OrderByDescending(x => x.TotalKm)
            .FirstOrDefault();

        return Ok(resultado);
    }

    [HttpGet("promedio-bateria")]
    public IActionResult Promedio()
    {
        var d = Datos.Drones.Where(x =>
            x.Estado.Equals("Disponible", StringComparison.OrdinalIgnoreCase)
        );

        var resultado = d.Any() ? d.Average(x => x.Bateria) : 0;

        return Ok(resultado);
    }

    [HttpGet("misiones-pendientes")]
    public IActionResult Pendientes()
    {
        var resultado = Datos.Misiones.Where(x => !x.Completada).OrderBy(x => x.Fecha);

        return Ok(resultado);
    }
}
