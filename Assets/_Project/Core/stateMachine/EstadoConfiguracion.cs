using UnityEngine;
using System.Collections;

namespace TEID.Nucleo.MaquinaEstados
{
    // Hereda de EstadoJuego -> Es un estado válido
    public class EstadoConfiguracion : EstadoJuego
    {
        public EstadoConfiguracion(Scripts.Managers.AdministradorJuego sistema) : base(sistema) { }

        public override IEnumerator Iniciar()
        {
            Debug.Log("⚙️ ESTADO: Configuración Inicial...");
            
            // Simular carga de datos, buscar fichas, barajar cartas...
            yield return new WaitForSeconds(1f); // Pequeña pausa dramática

            Debug.Log("✅ Configuración terminada. El juego puede comenzar.");

           Debug.Log("✅ Configuración terminada. Iniciando movimiento...");

            // Buscamos una ficha para probar (la primera que tenga el Manager)
            if (sistema.personajesEnJuego.Count > 0)
            {
                var fichaPrueba = sistema.personajesEnJuego[0];
                sistema.CambiarEstado(new EstadoMovimiento(sistema, fichaPrueba));
            }
            else
            {
                Debug.LogError("No hay fichas en la lista del GameManager para mover.");
            }
        }
    }
}