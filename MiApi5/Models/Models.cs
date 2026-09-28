namespace TP.Api.Models;



public class Actividad
{
    private int id;
    private string nombre = string.Empty;
    private string tipo = string.Empty;
    private DateTime horario;
    private int duracionMinutos;
    private int capacidad;
    private bool activa = true;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public string Nombre
    {
        get { return this.nombre; }
        set { this.nombre = value; }
    }
    public string Tipo
    {
        get { return this.tipo; }
        set { this.tipo = value; }
    }
    public DateTime Horario
    {
        get { return this.horario; }
        set { this.horario = value; }
    }
    public int DuracionMinutos
    {
        get { return this.duracionMinutos; }
        set { this.duracionMinutos = value; }
    }
    public int Capacidad
    {
        get { return this.capacidad; }
        set { this.capacidad = value; }
    }
    public bool Activa
    {
        get { return this.activa; }
        set { this.activa = value; }
    }
}

public class ParticipanteFestival
{
    private int id;
    private string nombre = string.Empty;
    private string email = string.Empty;
    private int edad;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public string Nombre
    {
        get { return this.nombre; }
        set { this.nombre = value; }
    }
    public string Email
    {
        get { return this.email; }
        set { this.email = value; }
    }
    public int Edad
    {
        get { return this.edad; }
        set { this.edad = value; }
    }
}

public class ReservaActividad
{
    private int id;
    private int actividadId;
    private int participanteId;
    private DateTime fechaReserva;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public int ActividadId
    {
        get { return this.actividadId; }
        set { this.actividadId = value; }
    }
    public int ParticipanteId
    {
        get { return this.participanteId; }
        set { this.participanteId = value; }
    }
    public DateTime FechaReserva
    {
        get { return this.fechaReserva; }
        set { this.fechaReserva = value; }
    }
}



public static class Datos

{

    public static List<Actividad> Actividades { get; } = new();

    public static List<ParticipanteFestival> ParticipantesFestival { get; } = new();

    public static List<ReservaActividad> Reservas { get; } = new();

}

