using UnityEngine;
using System.Collections; // Necesario para Corrutinas
using System.Collections.Generic;
using TEID.Nucleo.Dominio;
using TEID.Scripts.Ambiente;

namespace TEID.Scripts.Personajes
{
    public class FichaTablero : MonoBehaviour
    {
        [Header("Configuración")]
        public DatosPersonajeSO datos;
        public float velocidadMovimiento = 5f; // Metros por segundo

        [Header("Estado Actual")]
        public NodoTablero nodoActual;
        public bool seEstaMoviendo = false; // Para bloquear inputs mientras camina

        private void Start()
        {
            SnapA_Nodo();
        }

        // --- MOVIMIENTO TELETRANSPORTADO (El que ya tenías) ---
        public void SnapA_Nodo()
        {
            if (nodoActual != null)
            {
                transform.position = nodoActual.transform.position + Vector3.up * 0.5f;
                transform.rotation = nodoActual.transform.rotation;
            }
        }

        // --- NUEVO: MOVIMIENTO FLUIDO (Corrutina) ---
        // Recibe una lista de pasos: [NodoA, NodoB, NodoC...]
        public void MoverPorCamino(List<NodoTablero> camino)
        {
            if (seEstaMoviendo) return; // Seguridad: No interrumpir si ya camina
            
            // Iniciamos la "película" del movimiento
            StartCoroutine(RutinaMovimiento(camino));
        }

        private IEnumerator RutinaMovimiento(List<NodoTablero> camino)
        {
            seEstaMoviendo = true;

            // Recorremos cada nodo de la lista uno por uno
            foreach (var siguienteNodo in camino)
            {
                // Paso 1: Definir el objetivo visual (encima del cubo)
                Vector3 destino = siguienteNodo.transform.position + Vector3.up * 0.5f;

                // Paso 2: Moverse suavemente hasta llegar
                // Mientras la distancia sea mayor a 0.05 (muy cerquita)...
                while (Vector3.Distance(transform.position, destino) > 0.05f)
                {
                    // MoveTowards calcula el punto intermedio para este frame
                    transform.position = Vector3.MoveTowards(
                        transform.position, 
                        destino, 
                        velocidadMovimiento * Time.deltaTime
                    );

                    // IMPORTANTE: Aquí pausamos hasta el siguiente frame de video
                    yield return null; 
                }

                // Paso 3: Asegurar posición final exacta y actualizar lógica
                transform.position = destino;
                nodoActual = siguienteNodo; // Actualizamos el cerebro: "Ahora estoy aquí"
            }

            seEstaMoviendo = false;
            Debug.Log("🏁 Movimiento terminado.");
        }

        // --- HERRAMIENTA DE PRUEBA (Para no usar mouse todavía) ---
        [ContextMenu("Prueba: Mover al Primer Vecino")]
        public void TestMoverAlVecino()
        {
            if (nodoActual != null && nodoActual.vecinos.Count > 0)
            {
                // Creamos un "camino" falso de un solo paso
                List<NodoTablero> caminoPrueba = new List<NodoTablero>();
                caminoPrueba.Add(nodoActual.vecinos[0]); // Agrega al primer vecino que encuentre

                MoverPorCamino(caminoPrueba);
            }
            else
            {
                Debug.LogWarning("No tengo vecinos a donde ir.");
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (nodoActual != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, nodoActual.transform.position);
                Gizmos.DrawSphere(nodoActual.transform.position, 0.2f);
            }
        }
    }
}