namespace TP.Api.Models;



public class ParticipanteDesafio
{
    private int id;
    private string nombre = string.Empty;
    private string email = string.Empty;
    private string nivel = string.Empty;
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
    public string Nivel
    {
        get { return this.nivel; }
        set { this.nivel = value; }
    }
}

public class Desafio
{
    private int id;
    private string titulo = string.Empty;
    private string dificultad = string.Empty;
    private int puntajeMaximo;
    private bool activo = true;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public string Titulo
    {
        get { return this.titulo; }
        set { this.titulo = value; }
    }
    public string Dificultad
    {
        get { return this.dificultad; }
        set { this.dificultad = value; }
    }
    public int PuntajeMaximo
    {
        get { return this.puntajeMaximo; }
        set { this.puntajeMaximo = value; }
    }
    public bool Activo
    {
        get { return this.activo; }
        set { this.activo = value; }
    }
}

public class Resolucion
{
    private int id;
    private int participanteId;
    private int desafioId;
    private int puntajeObtenido;
    private DateTime fechaEntrega;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public int ParticipanteId
    {
        get { return this.participanteId; }
        set { this.participanteId = value; }
    }
    public int DesafioId
    {
        get { return this.desafioId; }
        set { this.desafioId = value; }
    }
    public int PuntajeObtenido
    {
        get { return this.puntajeObtenido; }
        set { this.puntajeObtenido = value; }
    }
    public DateTime FechaEntrega
    {
        get { return this.fechaEntrega; }
        set { this.fechaEntrega = value; }
    }
}



public static class Datos

{

    public static List<ParticipanteDesafio> ParticipantesDesafio { get; } = new();

    public static List<Desafio> Desafios { get; } = new();

    public static List<Resolucion> Resoluciones { get; } = new();

}

