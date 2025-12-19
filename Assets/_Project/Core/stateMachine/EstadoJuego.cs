using UnityEngine;
using System.Collections; // Para Corrutinas
using TEID.Scripts.Managers; // (Crearemos este namespace luego)

namespace TEID.Nucleo.MaquinaEstados
{
    // "abstract" significa que no puedes usar esta clase directamente,
    // tienes que crear hijos de ella (Herencia).
    public abstract class EstadoJuego
    {
        // Referencia al "Dueño" de la máquina para poder pedirle cosas
        // (como cambiar de estado o acceder al tablero)
        protected AdministradorJuego sistema;

        public EstadoJuego(AdministradorJuego sistema)
        {
            this.sistema = sistema;
        }

        // Se ejecuta una vez cuando entramos a este estado
        // Ej: Repartir cartas al iniciar turno
        public virtual IEnumerator Iniciar() 
        { 
            yield break; 
        }

        // Se ejecuta en cada frame (Update)
        // Ej: Esperar input del jugador
        public virtual void Actualizar() { }

        // Se ejecuta al salir
        // Ej: Limpiar la UI
        public virtual void Salir() { }
    }
}