using Alquinet_Datos;
using Alquinet_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquinet_Negocio
{
    public class FotoService
    {
        private FotoRepository _fotoRepository = new FotoRepository();

        public List<Fotos> GetFotosByCod(int cod_propiedad)
        {
            return _fotoRepository.GetFotosByCod(cod_propiedad);
        }

        public int Registrar(Fotos foto, out string mensaje)
        {
            return _fotoRepository.Registrar(foto, out mensaje);
        }
    }
}
