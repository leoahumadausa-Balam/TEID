using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TEID.Scripts.Personajes; // Para mover la ficha
using TEID.Scripts.Ambiente;   // Para leer los nodos
using TEID.Nucleo.Logica;      // Para el algoritmo de caminos

namespace TEID.Nucleo.MaquinaEstados
{
    public class EstadoMovimiento : EstadoJuego
    {
        private FichaTablero fichaActiva;
        private List<NodoTablero> nodosValidos;
        private bool esperandoInput = false;

        // Constructor: Recibimos quién se va a mover
        public EstadoMovimiento(Scripts.Managers.AdministradorJuego sistema, FichaTablero ficha) : base(sistema) 
        {
            this.fichaActiva = ficha;
        }

        public override IEnumerator Iniciar()
        {
            Debug.Log($"🏃 ESTADO: Turno de movimiento para {fichaActiva.name}");

            // 1. Obtenemos el rango REAL desde los datos del personaje
            // Accedemos a la Ficha -> Datos -> MovimientoPorTurno
            int rangoReal = fichaActiva.datos.movimientoPorTurno;

            // 2. Usamos esa variable en el buscador
            nodosValidos = BuscadorDeCaminos.ObtenerNodosAlcanzables(fichaActiva.nodoActual, rangoReal);
    
            Debug.Log($"Info: Rango {rangoReal}. Puede moverse a {nodosValidos.Count} casillas.");

            esperandoInput = true;
            yield break;
        }

        public override void Actualizar()
        {
            if (!esperandoInput) return;

            // Detectar Clic Izquierdo del Mouse
            if (Input.GetMouseButtonDown(0))
            {
                ProcesarClic();
            }
        }

        private void ProcesarClic()
        {
            // --- MAGIA 3D: RAYCASTING ---
            
            // 1. Crear el rayo desde la cámara hacia donde apunta el mouse
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit golpe;

            // 2. Disparar el rayo. Si choca con algo (física)...
            if (Physics.Raycast(rayo, out golpe))
            {
                // 3. Preguntar: ¿Golpeé un NodoTablero?
                NodoTablero nodoClicado = golpe.collider.GetComponent<NodoTablero>();

                if (nodoClicado != null)
                {
                    IntentarMover(nodoClicado);
                }
            }
        }

        private void IntentarMover(NodoTablero destino)
        {
            // 1. Validar: ¿Está el destino dentro de las casillas azules permitidas?
            if (nodosValidos.Contains(destino))
            {
                Debug.Log($"✅ Destino válido: {destino.name}. Calculando ruta...");
                esperandoInput = false; // Bloqueamos inputs

                // 2. USAMOS EL GPS NUEVO
                // Pedimos la ruta detallada desde donde estoy hasta donde hice clic
                List<NodoTablero> rutaCompleta = BuscadorDeCaminos.CalcularRuta(fichaActiva.nodoActual, destino);

                // 3. Le damos la lista completa a la ficha para que camine paso a paso
                fichaActiva.MoverPorCamino(rutaCompleta);

                // Esperamos y terminamos
                sistema.StartCoroutine(EsperarYFinalizar(rutaCompleta.Count));
            }
            else
            {
                Debug.LogWarning("❌ Ese destino está muy lejos o no es válido.");
            }
        }

       private IEnumerator EsperarYFinalizar(int pasos)
        {
            // Calculamos tiempo aproximado: 0.5s por paso (ajusta según la velocidad de tu ficha)
            float tiempoEspera = pasos * 0.5f + 0.5f; 
            yield return new WaitForSeconds(tiempoEspera);
            
            Debug.Log("🏁 Fin del Turno de Movimiento.");
            // Aquí en el futuro cambiarás al Estado de Acción o atacar
        }
    }
}