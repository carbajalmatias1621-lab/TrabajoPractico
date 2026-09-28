namespace TP.Api.Models;



public class ObjetoEspacial
{
    private int id;
    private string nombre = string.Empty;
    private string tipo = string.Empty;
    private double distancia;
    private int nivelRiesgo;
    private DateTime fechaDescubrimiento;
    private bool activo = true;
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
    public double Distancia
    {
        get { return this.distancia; }
        set { this.distancia = value; }
    }
    public int NivelRiesgo
    {
        get { return this.nivelRiesgo; }
        set { this.nivelRiesgo = value; }
    }
    public DateTime FechaDescubrimiento
    {
        get { return this.fechaDescubrimiento; }
        set { this.fechaDescubrimiento = value; }
    }
    public bool Activo
    {
        get { return this.activo; }
        set { this.activo = value; }
    }
}

public class Observacion
{
    private int id;
    private int objetoEspacialId;
    private DateTime fecha;
    private double distanciaMedida;
    private double velocidad;
    private string comentario = string.Empty;
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public int ObjetoEspacialId
    {
        get { return this.objetoEspacialId; }
        set { this.objetoEspacialId = value; }
    }
    public DateTime Fecha
    {
        get { return this.fecha; }
        set { this.fecha = value; }
    }
    public double DistanciaMedida
    {
        get { return this.distanciaMedida; }
        set { this.distanciaMedida = value; }
    }
    public double Velocidad
    {
        get { return this.velocidad; }
        set { this.velocidad = value; }
    }
    public string Comentario
    {
        get { return this.comentario; }
        set { this.comentario = value; }
    }
}



public static class Datos

{

    public static List<ObjetoEspacial> ObjetosEspaciales { get; } = new();

    public static List<Observacion> Observaciones { get; } = new();

}

