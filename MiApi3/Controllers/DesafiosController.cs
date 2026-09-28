using Microsoft.AspNetCore.Mvc;
using TP.Api.Models;

namespace TP.Api.Controllers;

[ApiController, Route("api/desafios")]
public class DesafiosController : ControllerBase
{
    private static int IdParticipanteNuevo() => Datos.ParticipantesDesafio.Select(p => p.Id).DefaultIfEmpty().Max() + 1;
    private static int IdDesafioNuevo() => Datos.Desafios.Select(d => d.Id).DefaultIfEmpty().Max() + 1;
    private static int IdResolucionNueva() => Datos.Resoluciones.Select(r => r.Id).DefaultIfEmpty().Max() + 1;

    [HttpPost("participantes")]
    public IActionResult Participante(ParticipanteDesafio x)
    {
        if (string.IsNullOrWhiteSpace(x.Nombre) || string.IsNullOrWhiteSpace(x.Email) || !x.Email.Contains('@'))
            return BadRequest("Nombre y email son obligatorios.");

        if (
            Datos.ParticipantesDesafio.Any(p =>
                p.Email.Equals(x.Email, StringComparison.OrdinalIgnoreCase)
            )
        )
            return Conflict("El email ya existe.");

        x.Id = IdParticipanteNuevo();

        Datos.ParticipantesDesafio.Add(x);

        return Created("", x);
    }

    [HttpPost()]
    public IActionResult Crear(Desafio x)
    {
        if (x.PuntajeMaximo <= 0)
            return BadRequest("El puntaje maximo debe ser positivo.");

        x.Id = IdDesafioNuevo();

        Datos.Desafios.Add(x);

        return Created("", x);
    }

    [HttpPost("resoluciones")]
    public IActionResult Resolver(Resolucion x)
    {
        var p = Datos.ParticipantesDesafio.FirstOrDefault(p => p.Id == x.ParticipanteId);
        
        var d = Datos.Desafios.FirstOrDefault(d => d.Id == x.DesafioId);

        if (p is null || d is null)
            return NotFound("Participante o desafio inexistente.");

        if (!d.Activo)
            return BadRequest("El desafio esta inactivo.");

        if (x.PuntajeObtenido < 0 || x.PuntajeObtenido > d.PuntajeMaximo)
            return BadRequest("Puntaje invalido.");

        if (
            Datos.Resoluciones.Any(r =>
                r.ParticipanteId == x.ParticipanteId && r.DesafioId == x.DesafioId
            )
        )
            return Conflict("El desafio ya fue resuelto por el participante.");

        x.Id = IdResolucionNueva();

        x.FechaEntrega = x.FechaEntrega == default ? DateTime.Now : x.FechaEntrega;

        Datos.Resoluciones.Add(x);

        return Created("", x);
    }

    [HttpGet("activos")]
    public IActionResult Activos()
    {
        var resultado = Datos.Desafios.Where(x => x.Activo);

        return Ok(resultado);
    }

    [HttpGet("dificultad/({valor})")]
    public IActionResult Dificultad(string valor)
    {
        var resultado = Datos.Desafios.Where(x =>
            x.Dificultad.Equals(valor, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("participantes/({id:int})/resoluciones")]
    public IActionResult Resoluciones(int id)
    {
        var resultado = Datos.Resoluciones.Where(x => x.ParticipanteId == id);

        return Ok(resultado);
    }

    [HttpGet("participantes/superaron/({puntaje:int})")]
    public IActionResult Superaron(int puntaje)
    {
        var resultado = Datos.ParticipantesDesafio.Where(p =>
            Datos
                .Resoluciones.Where(r => r.ParticipanteId == p.Id)
                .Any(r => r.PuntajeObtenido > puntaje)
        );

        return Ok(resultado);
    }

    [HttpGet("({id:int})/promedio")]
    public IActionResult Promedio(int id)
    {
        var r = Datos.Resoluciones.Where(x => x.DesafioId == id);

        var resultado = r.Any() ? r.Average(x => x.PuntajeObtenido) : 0;

        return Ok(resultado);
    }

    [HttpGet("ranking")]
    public IActionResult Ranking()
    {
        var resultado = Datos
            .ParticipantesDesafio.Select(p => new
            {
                Participante = p,
                Total = Datos
                    .Resoluciones.Where(r => r.ParticipanteId == p.Id)
                    .Sum(r => r.PuntajeObtenido),
            })
            .OrderByDescending(x => x.Total);

        return Ok(resultado);
    }

    [HttpGet("mejor-participante")]
    public IActionResult Mejor()
    {
        var resultado = Datos
            .ParticipantesDesafio.Select(p => new
            {
                Participante = p,
                Total = Datos
                    .Resoluciones.Where(r => r.ParticipanteId == p.Id)
                    .Sum(r => r.PuntajeObtenido),
            })
            .OrderByDescending(x => x.Total)
            .FirstOrDefault();

        return Ok(resultado);
    }

    [HttpGet("participantes/({id:int})/pendientes")]
    public IActionResult Pendientes(int id)
    {
        var resultado = Datos.Desafios.Where(d =>
            d.Activo && !Datos.Resoluciones.Any(r => r.ParticipanteId == id && r.DesafioId == d.Id)
        );

        return Ok(resultado);
    }
}
