using Microsoft.AspNetCore.Mvc;
using TP.Api.Models;

namespace TP.Api.Controllers;

[ApiController, Route("api/objetosespaciales")]
public class ObjetosEspacialesController : ControllerBase
{
    private static int IdObjetoNuevo() => Datos.ObjetosEspaciales.Select(o => o.Id).DefaultIfEmpty().Max() + 1;
    private static int IdObservacionNueva() => Datos.Observaciones.Select(o => o.Id).DefaultIfEmpty().Max() + 1;

    [HttpPost()]
    public IActionResult Crear(ObjetoEspacial x)
    {
        if (string.IsNullOrWhiteSpace(x.Nombre) || !double.IsFinite(x.Distancia) || x.Distancia < 0 || x.NivelRiesgo < 0)
            return BadRequest("Datos invalidos.");

        if (x.FechaDescubrimiento > DateTime.Now)
            return BadRequest("La fecha no puede ser futura.");

        x.Id = IdObjetoNuevo();

        Datos.ObjetosEspaciales.Add(x);

        return Created("", x);
    }

    [HttpPost("observaciones")]
    public IActionResult Observar(Observacion x)
    {
        if (!Datos.ObjetosEspaciales.Any(o => o.Id == x.ObjetoEspacialId))
            return NotFound("El objeto espacial no existe.");

        if (x.Fecha > DateTime.Now || !double.IsFinite(x.DistanciaMedida) || x.DistanciaMedida < 0 || !double.IsFinite(x.Velocidad) || x.Velocidad < 0)
            return BadRequest("Datos de observacion invalidos.");

        x.Id = IdObservacionNueva();

        Datos.Observaciones.Add(x);

        return Created("", x);
    }

    [HttpGet("({id:int})/observaciones")]
    public IActionResult Observaciones(int id)
    {
        var resultado = Datos.Observaciones.Where(x => x.ObjetoEspacialId == id);

        return Ok(resultado);
    }

    [HttpGet("buscar")]
    public IActionResult Buscar(string nombre)
    {
        var resultado = Datos.ObjetosEspaciales.Where(x =>
            x.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("tipo/({tipo})")]
    public IActionResult Tipo(string tipo)
    {
        var resultado = Datos.ObjetosEspaciales.Where(x =>
            x.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("riesgo/({nivel:int})")]
    public IActionResult Riesgo(int nivel)
    {
        var resultado = Datos
            .ObjetosEspaciales.Where(x => x.NivelRiesgo == nivel)
            .OrderByDescending(x => x.NivelRiesgo);

        return Ok(resultado);
    }

    [HttpGet("cercanos/({distancia:double})")]
    public IActionResult Cercanos(double distancia)
    {
        var resultado = Datos.ObjetosEspaciales.Where(x => x.Distancia < distancia);

        return Ok(resultado);
    }

    [HttpGet("promedio-velocidad")]
    public IActionResult Promedio()
    {
        var resultado = Datos.Observaciones.Any()
            ? Datos.Observaciones.Average(x => x.Velocidad)
            : 0;

        return Ok(resultado);
    }

    [HttpGet("mas-observado")]
    public IActionResult MasObservado()
    {
        var resultado = Datos
            .ObjetosEspaciales.Select(o => new
            {
                Objeto = o,
                Cantidad = Datos.Observaciones.Count(x => x.ObjetoEspacialId == o.Id),
            })
            .OrderByDescending(x => x.Cantidad)
            .FirstOrDefault();

        return Ok(resultado);
    }

    [HttpGet("sin-observaciones")]
    public IActionResult SinObservaciones()
    {
        var resultado = Datos.ObjetosEspaciales.Where(o =>
            !Datos.Observaciones.Any(x => x.ObjetoEspacialId == o.Id)
        );

        return Ok(resultado);
    }

    [HttpGet("estadisticas")]
    public IActionResult Estadisticas()
    {
        var resultado = new
        {
            CantidadObjetos = Datos.ObjetosEspaciales.Count(),
            Activos = Datos.ObjetosEspaciales.Count(x => x.Activo),
            CantidadObservaciones = Datos.Observaciones.Count(),
            PromedioVelocidad = Datos.Observaciones.Any()
                ? Datos.Observaciones.Average(x => x.Velocidad)
                : 0,
        };

        return Ok(resultado);
    }

    [HttpGet("alertas")]
    public IActionResult Alertas(
        [FromQuery] int riesgoMinimo = 7,
        [FromQuery] double distanciaMaxima = 1000
    )
    {
        var resultado = Datos.ObjetosEspaciales.Where(x =>
            x.NivelRiesgo >= riesgoMinimo && x.Distancia < distanciaMaxima && x.Activo
        );

        return Ok(resultado);
    }
}
