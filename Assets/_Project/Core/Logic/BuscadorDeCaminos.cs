using System.Collections.Generic;
using TEID.Scripts.Ambiente; // Necesitamos saber qué es un NodoTablero
using TEID.Nucleo.Dominio;   // Necesitamos saber los tipos de casillas

namespace TEID.Nucleo.Logica
{
    public static class BuscadorDeCaminos
    {
        // Esta función devuelve una lista de todos los nodos a los que puedes llegar
        // inicio: Dónde estás ahora
        // rango: Cuántos pasos (o PA) tienes disponibles
        public static List<NodoTablero> ObtenerNodosAlcanzables(NodoTablero inicio, int rango)
        {
            // 1. Preparamos las herramientas
            List<NodoTablero> alcanzables = new List<NodoTablero>();
            
            // La "Cola" es nuestra lista de tareas pendientes. 
            // Guardamos el nodo y cuánto "gasolina" nos queda al llegar a él.
            Queue<(NodoTablero nodo, int pasosRestantes)> frontera = new Queue<(NodoTablero, int)>();
            
            // El "Historial" para no volver a revisar una casilla que ya vimos
            HashSet<NodoTablero> visitados = new HashSet<NodoTablero>();

            // 2. Empezamos en el origen
            frontera.Enqueue((inicio, rango));
            visitados.Add(inicio);

            // 3. El bucle de la Mancha de Aceite
            while (frontera.Count > 0)
            {
                // Sacamos el siguiente nodo de la cola
                var (actual, pasos) = frontera.Dequeue();

                // Si no es el nodo de inicio, lo añadimos a la lista de "lugares válidos"
                if (actual != inicio)
                {
                    alcanzables.Add(actual);
                }

                // Si ya no nos quedan pasos, no seguimos expandiendo desde aquí
                if (pasos <= 0) continue;

                // 4. Miramos a los vecinos
                foreach (var vecino in actual.vecinos)
                {
                    // Si no lo hemos visitado todavía...
                    if (!visitados.Contains(vecino))
                    {
                        // AQUÍ IRÁN TUS REGLAS ESPECIALES (Ej. ¿Es camino exterior?)
                        // Por ahora, solo restamos 1 al movimiento (coste básico)
                        int coste = 1; 
                        
                        // Añadimos el vecino a la cola para revisarlo luego
                        frontera.Enqueue((vecino, pasos - coste));
                        visitados.Add(vecino);
                    }
                }
            }

            return alcanzables;
        }

        // --- NUEVO: EL GPS (Calcula la ruta paso a paso) ---
        public static List<NodoTablero> CalcularRuta(NodoTablero inicio, NodoTablero destino)
        {
            // Diccionario para recordar "de dónde vine" para llegar a cada casilla
            // Clave: Casilla Hija, Valor: Casilla Padre
            Dictionary<NodoTablero, NodoTablero> rastro = new Dictionary<NodoTablero, NodoTablero>();
            
            Queue<NodoTablero> frontera = new Queue<NodoTablero>();
            frontera.Enqueue(inicio);
            rastro[inicio] = null; // El inicio no tiene padre

            bool encontrado = false;

            // 1. Búsqueda BFS (Igual que antes, pero guardando el rastro)
            while (frontera.Count > 0)
            {
                NodoTablero actual = frontera.Dequeue();

                if (actual == destino)
                {
                    encontrado = true;
                    break; // ¡Llegamos!
                }

                foreach (var vecino in actual.vecinos)
                {
                    // Si no hemos registrado de dónde viene este vecino...
                    if (!rastro.ContainsKey(vecino))
                    {
                        rastro[vecino] = actual; // "Llegué al vecino DESDE actual"
                        frontera.Enqueue(vecino);
                    }
                }
            }

            // 2. Si no encontramos camino, devolvemos lista vacía
            if (!encontrado) return new List<NodoTablero>();

            // 3. Reconstruir el camino hacia atrás (Backtracking)
            List<NodoTablero> camino = new List<NodoTablero>();
            NodoTablero paso = destino;

            while (paso != inicio)
            {
                camino.Add(paso);
                paso = rastro[paso]; // Vamos al padre
            }

            // Como lo grabamos del final al principio, le damos la vuelta
            camino.Reverse();
            
            return camino;
        }
    }
}