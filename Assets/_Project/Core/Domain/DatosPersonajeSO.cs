using UnityEngine;

namespace TEID.Nucleo.Dominio
{
    // Añade la opción al menú de Unity para crear fichas de personajes
    [CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "TEID/Personajes/Datos de Personaje")]
    public class DatosPersonajeSO : ScriptableObject
    {
        [Header("Identidad")]
        public string nombre; // Ejemplo: "Mensajero Rojo" o "El Heraldo"
        public TipoRol rol;   // Aquí usas el Enum nuevo (Lista desplegable)
        
        [Header("Estadísticas Base")]
        [Range(0, 12)] // Esto pone una barrita deslizante en el inspector
        public int movimientoPorTurno = 1; // Heraldo = 5, Usurpador = 4
        
        public int influenciaInicial = 2; // Solo para Mensajeros
        
        [Header("Visuales")]
        public GameObject prefabModelo; // El modelo 3D (cuando lo tengas)
        public Color colorDebug = Color.blue; // Para diferenciar fichas en pruebas
    }
}