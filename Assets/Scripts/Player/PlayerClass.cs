using UnityEngine;

public class PlayerClass
{
    [Header("Métricas de Energía y Estrés")]
    private float estresActual = 0f;
    private float estresMaximo = 100f;

    private float energiaActual = 100f;
    private float energiaMaxima = 100f;

    [Header("Medidores de Ánimo")]
    [Tooltip("0 = Calma | 100 = Alerta")]
    [Range(0, 100)] private float tension = 50f;

    [Tooltip("0 = Pasividad | 100 = Hostilidad")]
    [Range(0, 100)] private float actitud = 50f;

    [Tooltip("0 = Liberado | 100 = Saturado")]
    [Range(0, 100)] private float bateriaSocial = 50f;

    [Header("Rasgos Base de Personalidad positivos")]
    [Range(0, 100)] private float empatia = 50f;
    [Range(0, 100)] private float sinceridad = 50f;
    [Range(0, 100)] private float seguridadEmocional = 50f;
    [Range(0, 100)] private float tolerancia = 50f;
    [Range(0, 100)] private float lealtad = 50f;

    [Header("Rasgos Base de Personalidad negativos")]
    [Range(0, 100)] private float egoismo = 50f;
    [Range(0, 100)] private float Manipulador = 50f;
    [Range(0, 100)] private float rencor = 50f;
    [Range(0, 100)] private float hostil = 50f;
    [Range(0, 100)] private float individualista = 50f;

    public float EstresActual { get => estresActual; set => estresActual = value; }
    public float EstresMaximo { get => estresMaximo; set => estresMaximo = value; }
    public float Tension { get => tension; set => tension = value; }
    public float Actitud { get => actitud; set => actitud = value; }
    public float BateriaSocial { get => bateriaSocial; set => bateriaSocial = value; }
    public float Empatia { get => empatia; set => empatia = value; }
    public float Sinceridad { get => sinceridad; set => sinceridad = value; }
    public float SeguridadEmocional { get => seguridadEmocional; set => seguridadEmocional = value; }
    public float Tolerancia { get => tolerancia; set => tolerancia = value; }
    public float Lealtad { get => lealtad; set => lealtad = value; }
    public float Egoismo { get => egoismo; set => egoismo = value; }
    public float Manipulador1 { get => Manipulador; set => Manipulador = value; }
    public float Rencor { get => rencor; set => rencor = value; }
    public float Hostil { get => hostil; set => hostil = value; }
    public float Individualista { get => individualista; set => individualista = value; }
}
