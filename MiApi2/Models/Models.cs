namespace TP.Api.Models;



public class Estacion
{
    private int id;
    private string nombre = string.Empty;
    private string localidad = string.Empty;
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
    public string Localidad
    {
        get { return this.localidad; }
        set { this.localidad = value; }
    }
    public bool Activa
    {
        get { return this.activa; }
        set { this.activa = value; }
    }
}

public class Medicion
{
    private int id;
    private int estacionId;
    private double temperatura;
    private double humedad;
    private double velocidadViento;
    private DateTime fechaHora;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public int EstacionId
    {
        get { return this.estacionId; }
        set { this.estacionId = value; }
    }
    public double Temperatura
    {
        get { return this.temperatura; }
        set { this.temperatura = value; }
    }
    public double Humedad
    {
        get { return this.humedad; }
        set { this.humedad = value; }
    }
    public double VelocidadViento
    {
        get { return this.velocidadViento; }
        set { this.velocidadViento = value; }
    }
    public DateTime FechaHora
    {
        get { return this.fechaHora; }
        set { this.fechaHora = value; }
    }
}



public static class Datos

{

    public static List<Estacion> Estaciones { get; } = new();

    public static List<Medicion> Mediciones { get; } = new();

}

