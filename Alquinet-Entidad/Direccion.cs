namespace Alquinet_Entidad
{
    //    CREATE TABLE[Direccion] (
    //  [cod PK] integer,
    //  [calle] varchar,
    //  [cod_pos] varchar,
    //  [barrio] varchar,
    //  [longitud] float,
    //  [latitud] float,
    //  [cod_propiedad FK] integer
    //);
    public class Direccion
    {
        public int Cod { get; set; }
        public string Cod_pos { get; set; }
        public string Calle { get; set; }
        public string Barrio { get; set; }
        public string Zona {  get; set; }
        public decimal Longitud { get; set; }
        public decimal Latitud { get; set; }
    }
}