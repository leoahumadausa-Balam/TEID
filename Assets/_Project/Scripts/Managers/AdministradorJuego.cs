using UnityEngine;
using TEID.Nucleo.MaquinaEstados; // Para ver los estados
using TEID.Scripts.Personajes;    // Para conocer a las fichas
using System.Collections.Generic;

namespace TEID.Scripts.Managers
{
    public class AdministradorJuego : MonoBehaviour
    {
        [Header("Estado Global")]
        // Aquí vemos en qué estado estamos (solo lectura para debug)
        [SerializeField] private string estadoActualNombre;
        
        // La variable "Polimórfica": Puede ser CUALQUIER hijo de EstadoJuego
        private EstadoJuego estadoActual;

        [Header("Referencias Globales")]
        // El Manager conoce a todos los actores
        public List<FichaTablero> personajesEnJuego;
        // Aquí podrías poner referencia a la UI, la Cámara, etc.

        private void Start()
        {
            // AL INICIAR: Configuramos el primer estado.
            // Por ahora, crearemos un estado de prueba "Vacio".
            CambiarEstado(new EstadoConfiguracion(this));
        }

        private void Update()
        {
            // Si hay un estado activo, dejamos que él maneje la lógica
            if (estadoActual != null)
            {
                estadoActual.Actualizar();
            }
        }

        // La función mágica para cambiar de fase
        public void CambiarEstado(EstadoJuego nuevoEstado)
        {
            // 1. Limpiamos el anterior
            if (estadoActual != null)
            {
                estadoActual.Salir();
            }

            // 2. Cambiamos
            estadoActual = nuevoEstado;
            estadoActualNombre = nuevoEstado.GetType().Name; // Para verlo en Inspector

            // 3. Iniciamos el nuevo (usamos Corrutina por si tiene animaciones)
            StartCoroutine(estadoActual.Iniciar());
        }
    }
}