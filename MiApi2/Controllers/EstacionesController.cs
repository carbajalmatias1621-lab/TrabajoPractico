using Microsoft.AspNetCore.Mvc;
using TP.Api.Models;

namespace TP.Api.Controllers;

[ApiController, Route("api/estaciones")]
public class EstacionesController : ControllerBase
{
    private static int IdEstacionNuevo() => Datos.Estaciones.Select(e => e.Id).DefaultIfEmpty().Max() + 1;
    private static int IdMedicionNueva() => Datos.Mediciones.Select(m => m.Id).DefaultIfEmpty().Max() + 1;

    [HttpPost()]
    public IActionResult Crear(Estacion x)
    {
        if (string.IsNullOrWhiteSpace(x.Nombre))
            return BadRequest("El nombre es obligatorio.");

        x.Id = IdEstacionNuevo();

        Datos.Estaciones.Add(x);

        return Created("", x);
    }

    [HttpGet()]
    public IActionResult Todas()
    {
        var resultado = Datos.Estaciones;

        return Ok(resultado);
    }

    [HttpGet("localidad/({localidad})")]
    public IActionResult PorLocalidad(string localidad)
    {
        var resultado = Datos.Estaciones.Where(x =>
            x.Localidad.Contains(localidad, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpPost("mediciones")]
    public IActionResult Medir(Medicion x)
    {
        var e = Datos.Estaciones.FirstOrDefault(e => e.Id == x.EstacionId);

        if (e is null)
            return NotFound("La estacion no existe.");

        if (!e.Activa)
            return BadRequest("La estacion esta inactiva.");

        if (!double.IsFinite(x.Temperatura) || !double.IsFinite(x.Humedad) || !double.IsFinite(x.VelocidadViento))
            return BadRequest("Los valores de la medicion deben ser numericos validos.");

        if (x.Humedad < 0 || x.Humedad > 100)
            return BadRequest("La humedad debe estar entre 0 y 100.");

        if (x.VelocidadViento < 0)
            return BadRequest("La velocidad no puede ser negativa.");

        if (x.FechaHora > DateTime.Now)
            return BadRequest("La fecha no puede ser futura.");

        x.Id = IdMedicionNueva();

        Datos.Mediciones.Add(x);

        return Created("", x);
    }

    [HttpGet("mediciones")]
    public IActionResult Mediciones()
    {
        var resultado = Datos.Mediciones;

        return Ok(resultado);
    }

    [HttpGet("({id:int})/mediciones")]
    public IActionResult MedicionesDe(int id)
    {
        var resultado = Datos.Mediciones.Where(x => x.EstacionId == id);

        return Ok(resultado);
    }

    [HttpGet("temperaturas-superiores/({valor:double})")]
    public IActionResult Superiores(double valor)
    {
        var resultado = Datos.Mediciones.Where(x => x.Temperatura > valor);

        return Ok(resultado);
    }

    [HttpGet("temperaturas-ordenadas")]
    public IActionResult Ordenadas()
    {
        var resultado = Datos.Mediciones.OrderByDescending(x => x.Temperatura);

        return Ok(resultado);
    }

    [HttpGet("estadisticas")]
    public IActionResult Estadisticas()
    {
        return !Datos.Mediciones.Any()
            ? Ok(
                new
                {
                    promedio = 0,
                    maxima = 0,
                    minima = 0,
                }
            )
            : Ok(
                new
                {
                    promedio = Datos.Mediciones.Average(x => x.Temperatura),
                    maxima = Datos.Mediciones.Max(x => x.Temperatura),
                    minima = Datos.Mediciones.Min(x => x.Temperatura),
                }
            );
    }

    [HttpGet("cantidad-por-estacion")]
    public IActionResult Cantidad()
    {
        var resultado = Datos.Estaciones.Select(e => new
        {
            e.Id,
            e.Nombre,
            Cantidad = Datos.Mediciones.Count(m => m.EstacionId == e.Id),
        });

        return Ok(resultado);
    }
}
