using UnityEngine;
using System.Collections.Generic;
using TEID.Nucleo.Dominio; // Necesario para entender TipoNodo y DatosNodoSO

namespace TEID.Scripts.Ambiente
{
    public class NodoTablero : MonoBehaviour
    {
        [Header("Configuración")]
        public DatosNodoSO datos; // Arrastra aquí el archivo (ej. Datos_Taberna)
        public CapaCamino caminoPermitido = CapaCamino.Interior; // ¿Quién camina por aquí?

        [Header("Conexiones (El Grafo)")]
        // Aquí arrastramos manualmente los otros Nodos vecinos en el Inspector
        public List<NodoTablero> vecinos = new List<NodoTablero>();

        // Variable para pruebas en el inspector
        [Header("Debug - Pruebas")]
        public int rangoPrueba = 2;
        public bool mostrarAlcance = false;

        // IMPORTANTE: Este nombre debe estar en inglés porque es un evento nativo de Unity.
        // Se ejecuta en el editor para dibujar ayudas visuales.
        private void OnDrawGizmos()
        {
            // (Lo que ya tenías para dibujar líneas amarillas...)
            if (datos != null) { Gizmos.color = datos.colorDebug; Gizmos.DrawSphere(transform.position, 0.3f); }
            Gizmos.color = Color.yellow;
            foreach (var vecino in vecinos) { if(vecino) Gizmos.DrawLine(transform.position, vecino.transform.position); }

            // --- NUEVO CÓDIGO DE PRUEBA ---
            if (mostrarAlcance)
            {
                // Llamamos a nuestra nueva calculadora lógica
                // Nota: Como estamos en el Editor y no en Play, usamos un truco (fully qualified name) 
                // o asegúrate de que el script compile bien.
                var alcanzables = TEID.Nucleo.Logica.BuscadorDeCaminos.ObtenerNodosAlcanzables(this, rangoPrueba);

                Gizmos.color = Color.cyan; // Color Cian para el movimiento válido
                foreach (var nodo in alcanzables)
                {
                    // Dibujamos un "fantasma" en los nodos a los que llegamos
                    Gizmos.DrawWireSphere(nodo.transform.position, 0.5f);
                }
            }
        }

        // --- HERRAMIENTA DE EDITOR ---
        // Esto añade una opción al menú de Unity para arreglar errores humanos
        [ContextMenu("Autoconectar Vecinos (Bidireccional)")]
        public void ValidarConexiones()
        {
            // Bandera para saber si hicimos cambios
            bool huboCambios = false;

            // Recorremos todos los vecinos que yo (A) digo que tengo
            foreach (var vecino in vecinos)
            {
                if (vecino != null)
                {
                    // Pregunta Clave: Si yo apunto a él... ¿él apunta a mí?
                    if (!vecino.vecinos.Contains(this))
                    {
                        // Si no me tiene, me agrego a su lista a la fuerza
                        vecino.vecinos.Add(this);
                        Debug.Log($"Conexión Corregida: {vecino.name} ahora conoce a {this.name}");
                        huboCambios = true;
                    }
                }
            }

            if (!huboCambios)
            {
                Debug.Log("Todo perfecto. Todas las conexiones ya eran dobles.");
            }
        }

        // --- HERRAMIENTA MAESTRA (GLOBAL) ---

#if UNITY_EDITOR // Solo compilar este código dentro del Editor de Unity
        
        // Esto crea un botón en la barra superior de Unity: "TEID -> Herramientas -> Conectar Todo"
        [UnityEditor.MenuItem("TEID/Herramientas/Conectar Todo el Tablero")]
        public static void ConectarTodoElMundo()
        {
            // 1. Buscamos TODOS los nodos que existen en la escena actual
            NodoTablero[] todosLosNodos = FindObjectsOfType<NodoTablero>();

            Debug.Log($"🔧 Iniciando conexión masiva de {todosLosNodos.Length} nodos...");

            int correcciones = 0;

            // 2. Ejecutamos la validación en CADA uno de ellos
            foreach (var nodo in todosLosNodos)
            {
                // Usamos la misma lógica que ya escribiste
                foreach (var vecino in nodo.vecinos)
                {
                    if (vecino != null)
                    {
                        if (!vecino.vecinos.Contains(nodo))
                        {
                            vecino.vecinos.Add(nodo);
                            // Marcamos que hubo un cambio para que Unity guarde la escena
                            UnityEditor.EditorUtility.SetDirty(vecino); 
                            correcciones++;
                        }
                    }
                }
            }

            Debug.Log($"✅ ¡Listo! Se realizaron {correcciones} conexiones nuevas en todo el tablero.");
        }
#endif

    }

    
}