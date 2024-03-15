using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcCoreZapatillasPractica.Models;
using MvcCoreZapatillasPractica.Repositories;

namespace MvcCoreZapatillasPractica.Controllers
{
    public class ZapasController : Controller
    {
        private RepositoryZapas repo;

        public ZapasController(RepositoryZapas repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Zapas> zapas = await this.repo.GetAllZapasAsync();
            return View(zapas);
        }

        public async Task<IActionResult> Details(int idzapa)
        {
            Zapas zapa = await this.repo.FindZapaAsync(idzapa);
            return View(zapa);
        }

        public async Task<IActionResult> AddImagenZapa()
        {
            List<Zapas> zapas = await this.repo.GetAllZapasAsync();
            return View(zapas);
        }

        [HttpPost]
        public async Task<IActionResult> AddImagenZapa(int idzapa, List<string> imagenes)
        {
            foreach (var imagen in imagenes)
            {
                int ultimoIdImagen = await this.repo.GetUltimoIdImagenZapa(idzapa);

                int nuevoIdImagen = ultimoIdImagen + 1;

                ImagenesZapas imagenZapa = new ImagenesZapas
                {
                    IdImagen = nuevoIdImagen,
                    IdProducto = idzapa,
                    Imagen = imagen
                };

                await this.repo.AddImagenZapa(imagenZapa);
            }

            return RedirectToAction("Details",new{ idzapa = idzapa});
        }

        public async Task<IActionResult> _DetallesZapaPartial(int idzapa)
        {
            Zapas zapa = await this.repo.FindZapaAsync(idzapa);
            return PartialView("_DetallesZapaPartial", zapa);
        }

        public async Task<IActionResult> _ZapasPartial(int? posicion, int idZapa)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            var model = await this.repo.GetFotosZapasAsync(posicion.Value, idZapa);
            int numeroRegistros = model.NumeroRegistros;

            int siguiente = posicion.Value + 1;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }

            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }

            ViewData["ULTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;

            return PartialView("_ZapasPartial", model.Imagenes);
        }
    }
}
