using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreZapatillasPractica.Models
{
    public class ModelImagenesZapas
    {
        public int NumeroRegistros { get; set; }
        public List<ImagenesZapas> Imagenes { get; set; }
    }
}
