using UnityEngine;

namespace TEID.Nucleo.Dominio
{
    // Esto añade la opción al menú de clic derecho en Unity para crear los datos
    [CreateAssetMenu(fileName = "NuevosDatosNodo", menuName = "TEID/Tablero/Datos de Nodo")]
    public class DatosNodoSO : ScriptableObject
    {
        [Header("Identidad")]
        public string nombreNodo;
        public TipoNodo tipo; // Referencia al Enum de arriba
        
        [Header("Visuales")]
        public Sprite icono; // Icono 2D para UI o depuración
        public Color colorDebug = Color.white; // Para verlas en la escena antes del arte final

        [Header("Reglas (Lógica)")]
        [Tooltip("Costo base de Puntos de Acción (PA) para entrar aquí")]
        public int costeBasePA = 1;
        
        [TextArea] 
        public string descripcion; // Explicación del efecto para el diseñador
    }
}