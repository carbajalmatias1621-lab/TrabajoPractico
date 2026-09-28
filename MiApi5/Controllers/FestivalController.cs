using Microsoft.AspNetCore.Mvc;
using TP.Api.Models;

namespace TP.Api.Controllers;

[ApiController, Route("api/festival")]
public class FestivalController : ControllerBase
{
    private static int IdActividadNuevo() => Datos.Actividades.Select(a => a.Id).DefaultIfEmpty().Max() + 1;
    private static int IdParticipanteNuevo() => Datos.ParticipantesFestival.Select(p => p.Id).DefaultIfEmpty().Max() + 1;
    private static int IdReservaNueva() => Datos.Reservas.Select(r => r.Id).DefaultIfEmpty().Max() + 1;

    [HttpPost("actividades")]
    public IActionResult CrearActividad(Actividad x)
    {
        if (string.IsNullOrWhiteSpace(x.Nombre) || x.DuracionMinutos <= 0 || x.Capacidad <= 0)
            return BadRequest("Datos de actividad invalidos.");

        x.Id = IdActividadNuevo();

        Datos.Actividades.Add(x);

        return Created("", x);
    }

    [HttpPost("participantes")]
    public IActionResult CrearParticipante(ParticipanteFestival x)
    {
        if (string.IsNullOrWhiteSpace(x.Nombre) || string.IsNullOrWhiteSpace(x.Email) || !x.Email.Contains('@'))
            return BadRequest("Nombre y email son obligatorios.");

        if (
            Datos.ParticipantesFestival.Any(p =>
                p.Email.Equals(x.Email, StringComparison.OrdinalIgnoreCase)
            )
        )
            return Conflict("El email ya existe.");

        x.Id = IdParticipanteNuevo();

        Datos.ParticipantesFestival.Add(x);

        return Created("", x);
    }

    [HttpPost("reservas")]
    public IActionResult Reservar(ReservaActividad x)
    {
        var a = Datos.Actividades.FirstOrDefault(a => a.Id == x.ActividadId);

        var p = Datos.ParticipantesFestival.FirstOrDefault(p => p.Id == x.ParticipanteId);

        if (a is null || p is null)
            return NotFound("Actividad o participante inexistente.");

        if (!a.Activa)
            return BadRequest("La actividad esta inactiva.");

        if (Datos.Reservas.Any(r => r.ActividadId == a.Id && r.ParticipanteId == p.Id))
            return Conflict("El participante ya esta inscripto.");

        if (Datos.Reservas.Count(r => r.ActividadId == a.Id) >= a.Capacidad)
            return Conflict("La actividad esta completa.");

        var fin = a.Horario.AddMinutes(a.DuracionMinutos);

        bool superpone = Datos
            .Reservas.Where(r => r.ParticipanteId == p.Id)
            .Join(Datos.Actividades, r => r.ActividadId, otra => otra.Id, (r, otra) => otra)
            .Any(otra =>
                a.Horario < otra.Horario.AddMinutes(otra.DuracionMinutos) && fin > otra.Horario
            );

        if (superpone)
            return Conflict("El horario se superpone con otra actividad.");

        x.Id = IdReservaNueva();

        x.FechaReserva = DateTime.Now;

        Datos.Reservas.Add(x);

        return Created("", x);
    }

    [HttpDelete("reservas/({id:int})")]
    public IActionResult Cancelar(int id)
    {
        var x = Datos.Reservas.FirstOrDefault(x => x.Id == id);

        if (x is null)
            return NotFound("Reserva no encontrada.");

        Datos.Reservas.Remove(x);

        return NoContent();
    }

    [HttpGet("actividades")]
    public IActionResult Disponibles()
    {
        var resultado = Datos.Actividades.Where(a =>
            a.Activa && Datos.Reservas.Count(r => r.ActividadId == a.Id) < a.Capacidad
        );

        return Ok(resultado);
    }

    [HttpGet("actividades/({id:int})/participantes")]
    public IActionResult Participantes(int id)
    {
        var resultado = Datos
            .Reservas.Where(r => r.ActividadId == id)
            .Join(Datos.ParticipantesFestival, r => r.ParticipanteId, p => p.Id, (r, p) => p);

        return Ok(resultado);
    }

    [HttpGet("participantes/({id:int})/actividades")]
    public IActionResult Actividades(int id)
    {
        var resultado = Datos
            .Reservas.Where(r => r.ParticipanteId == id)
            .Join(Datos.Actividades, r => r.ActividadId, a => a.Id, (r, a) => a);

        return Ok(resultado);
    }

    [HttpGet("actividades/buscar")]
    public IActionResult Buscar(string nombre)
    {
        var resultado = Datos.Actividades.Where(a =>
            a.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("actividades/tipo/({tipo})")]
    public IActionResult Tipo(string tipo)
    {
        var resultado = Datos.Actividades.Where(a =>
            a.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase)
        );

        return Ok(resultado);
    }

    [HttpGet("actividades/completas")]
    public IActionResult Completas()
    {
        var resultado = Datos.Actividades.Where(a =>
            Datos.Reservas.Count(r => r.ActividadId == a.Id) >= a.Capacidad
        );

        return Ok(resultado);
    }

    [HttpGet("actividades/ranking")]
    public IActionResult Ranking()
    {
        var resultado = Datos
            .Actividades.Select(a => new
            {
                Actividad = a,
                Inscriptos = Datos.Reservas.Count(r => r.ActividadId == a.Id),
                Ocupacion = a.Capacidad == 0
                    ? 0
                    : Datos.Reservas.Count(r => r.ActividadId == a.Id) * 100.0 / a.Capacidad,
            })
            .OrderByDescending(x => x.Inscriptos);

        return Ok(resultado);
    }

    [HttpGet("actividades/mas-popular")]
    public IActionResult Popular()
    {
        var resultado = Datos
            .Actividades.Select(a => new
            {
                Actividad = a,
                Inscriptos = Datos.Reservas.Count(r => r.ActividadId == a.Id),
            })
            .OrderByDescending(x => x.Inscriptos)
            .FirstOrDefault();

        return Ok(resultado);
    }
}
