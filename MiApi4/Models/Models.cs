namespace TP.Api.Models;



public class Drone
{
    private int id;
    private string codigo = string.Empty;
    private string modelo = string.Empty;
    private int bateria;
    private string estado = "Disponible";
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public string Codigo
    {
        get { return this.codigo; }
        set { this.codigo = value; }
    }
    public string Modelo
    {
        get { return this.modelo; }
        set { this.modelo = value; }
    }
    public int Bateria
    {
        get { return this.bateria; }
        set { this.bateria = value; }
    }
    public string Estado
    {
        get { return this.estado; }
        set { this.estado = value; }
    }
}

public class Mision
{
    private int id;
    private int droneId;
    private string descripcion = string.Empty;
    private double distanciaKm;
    private DateTime fecha;
    private bool completada;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public int DroneId
    {
        get { return this.droneId; }
        set { this.droneId = value; }
    }
    public string Descripcion
    {
        get { return this.descripcion; }
        set { this.descripcion = value; }
    }
    public double DistanciaKm
    {
        get { return this.distanciaKm; }
        set { this.distanciaKm = value; }
    }
    public DateTime Fecha
    {
        get { return this.fecha; }
        set { this.fecha = value; }
    }
    public bool Completada
    {
        get { return this.completada; }
        set { this.completada = value; }
    }
}



public static class Datos

{

    public static List<Drone> Drones { get; } = new();

    public static List<Mision> Misiones { get; } = new();

}

