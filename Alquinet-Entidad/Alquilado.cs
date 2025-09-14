using System;

namespace Alquinet_Entidad
{
    public class Alquilado
    {
        public int Cod { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaText { get; set; }
        public int Cod_usuario { get; set; }
        public int Cod_agente { get; set; }
        public int Cod_propiedad { get; set; }
        public double Comision { get; set; }
        public string ComisionText { get; set; }
        public Usuario Usuario { get; set; }
        public Propiedad Propiedad { get; set; }
        public Agente Agente { get; set; }


    }
}
