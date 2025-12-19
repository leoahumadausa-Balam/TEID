namespace TEID.Nucleo.Dominio
{
    // Define la identidad de la casilla: ¿Qué es?
    public enum TipoNodo
    {
        Normal,         // Casilla de tránsito común
        Inicio,         // Donde inician los Mensajeros
        Exp,            // Otorga experiencia / Cartas
        Escudo,         // Protección
        Taberna,        // Recuperación
        Caverna,        // Teletransporte (interconectadas)
        CabanaGnomo,    // Cabaña del Gnomo
        Santuario,      // Final del juego
        Vacio           // Para huecos o lógica técnica
    }

    // Define quién puede pisar esta casilla
    // [System.Flags] permite combinar opciones (ej. Interior | Exterior)
    [System.Flags]
    public enum CapaCamino
    {
        Ninguno = 0,
        Interior = 1,      // Solo Mensajeros (Jugadores)
        Exterior = 2,      // Heraldo y Usurpador (NPCs)
        Todo = Interior | Exterior // Cualquiera puede pisar
    }

    // Define QUÉ es la ficha. Vital para aplicar reglas específicas.
    // Ejemplo: Si (rol == Heraldo) -> Mover 5 casillas.
    public enum TipoRol
    {
        Mensajero,      // Los jugadores (Consejeros)
        Heraldo,        // NPC Imperial
        Usurpador,      // NPC Enemigo
        Gnomo,          // NPC Neutral/Tienda
        Asesino         // NPC Hostil
    }
}