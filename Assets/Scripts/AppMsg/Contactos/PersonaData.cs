using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaPersona", menuName = "Dialogos/Ficha de Persona")]
public class PersonaData : ScriptableObject
{
    public struct LineaDialogo
    {
        public int id;
        public string hablante;
        public string texto;
        public int idSiguiente;
        
        // Opción 1
        public string opt1Text; public int dest1; public string mod1;
        // Opción 2
        public string opt2Text; public int dest2; public string mod2;
        // Opción 3
        public string opt3Text; public int dest3; public string mod3;
        // Opción 4
        public string opt4Text; public int dest4; public string mod4;
    }

    [Header("Datos Básicos")]
    public string nombre;
    public string relacion; 

    [Header("Atributos Dinámicos")]
    public int empatia = 50;
    public int ansiedad = 50;
    public int seguridad = 50;

    [Header("Control del Guion")]
    public int idActual = 0; 
    
    public TextAsset archivoGuionCSV;

    private Dictionary<int, LineaDialogo> diccionarioDialogos = new Dictionary<int, LineaDialogo>();

    public List<int> historialConversacion = new List<int>();

    public void ResetearPersonaje()
    {
        empatia = 50;
        ansiedad = 50;
        seguridad = 50;
        idActual = 0;
        historialConversacion.Clear();
    }

    /// <summary>
    /// Guarda un ID en el historial si no estaba ya registrado.
    /// </summary>
    public void GuardarEnHistorial(int id)
    {
        if (!historialConversacion.Contains(id))
        {
            historialConversacion.Add(id);
        }
    }

    public void InicializarGuion()
    {
        if (archivoGuionCSV == null)
        {
            Debug.LogError($"¡Falta el archivo CSV en la ficha de {nombre}!");
            return;
        }

        diccionarioDialogos.Clear();

        string[] lineas = archivoGuionCSV.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lineas.Length; i++)
        {
            string[] celdas = lineas[i].Split(';');

            if (celdas.Length < 16) continue;

            LineaDialogo linea = new LineaDialogo
            {
                id = int.Parse(celdas[0]),
                hablante = celdas[1],
                texto = celdas[2],
                idSiguiente = int.Parse(celdas[3]),
                
                // Opción 1 (Celdas 4, 5, 6)
                opt1Text = celdas[4], 
                dest1 = string.IsNullOrEmpty(celdas[5]) ? -1 : int.Parse(celdas[5]), 
                mod1 = celdas[6],
                
                // Opción 2 (Celdas 7, 8, 9)
                opt2Text = celdas[7], 
                dest2 = string.IsNullOrEmpty(celdas[8]) ? -1 : int.Parse(celdas[8]), 
                mod2 = celdas[9],
                
                // Opción 3 (Celdas 10, 11, 12)
                opt3Text = celdas[10], 
                dest3 = string.IsNullOrEmpty(celdas[11]) ? -1 : int.Parse(celdas[11]), 
                mod3 = celdas[12],
                
                // Opción 4 (Celdas 13, 14, 15)
                opt4Text = celdas[13], 
                dest4 = string.IsNullOrEmpty(celdas[14]) ? -1 : int.Parse(celdas[14]), 
                mod4 = celdas[15]
            };

            if (!diccionarioDialogos.ContainsKey(linea.id))
            {
                diccionarioDialogos.Add(linea.id, linea);
            }
        }
    }

    public bool ObtenerLineaPorID(int idBuscar, out LineaDialogo lineaEncontrada)
    {
        return diccionarioDialogos.TryGetValue(idBuscar, out lineaEncontrada);
    }

    public void ReiniciarConversacion()
    {
        idActual = 0;
        historialConversacion.Clear();
    }//reiniciar conversacion, chequear si lo sacamos luego
}