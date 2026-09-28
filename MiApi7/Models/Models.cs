namespace TP.Api.Models;



public class Expedicion
{
    private int id;
    private string nombre = string.Empty;
    private string destino = string.Empty;
    private DateTime fechaInicio;
    private DateTime fechaFin;
    private int capacidad;
    private string estado = "Futura";
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
    public string Destino
    {
        get { return this.destino; }
        set { this.destino = value; }
    }
    public DateTime FechaInicio
    {
        get { return this.fechaInicio; }
        set { this.fechaInicio = value; }
    }
    public DateTime FechaFin
    {
        get { return this.fechaFin; }
        set { this.fechaFin = value; }
    }
    public int Capacidad
    {
        get { return this.capacidad; }
        set { this.capacidad = value; }
    }
    public string Estado
    {
        get { return this.estado; }
        set { this.estado = value; }
    }
}

public class Explorador
{
    private int id;
    private string nombre = string.Empty;
    private string especialidad = string.Empty;
    private int experienciaAnios;
    private bool disponible = true;
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
    public string Especialidad
    {
        get { return this.especialidad; }
        set { this.especialidad = value; }
    }
    public int ExperienciaAnios
    {
        get { return this.experienciaAnios; }
        set { this.experienciaAnios = value; }
    }
    public bool Disponible
    {
        get { return this.disponible; }
        set { this.disponible = value; }
    }
}

public class Participacion
{
    private int id;
    private int expedicionId;
    private int exploradorId;
    private string rol = string.Empty;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public int ExpedicionId
    {
        get { return this.expedicionId; }
        set { this.expedicionId = value; }
    }
    public int ExploradorId
    {
        get { return this.exploradorId; }
        set { this.exploradorId = value; }
    }
    public string Rol
    {
        get { return this.rol; }
        set { this.rol = value; }
    }
}



public static class Datos

{

    public static List<Expedicion> Expediciones { get; } = new();

    public static List<Explorador> Exploradores { get; } = new();

    public static List<Participacion> Participaciones { get; } = new();

}

