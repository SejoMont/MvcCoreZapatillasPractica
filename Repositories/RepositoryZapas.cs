using MvcCoreZapatillasPractica.Data;
using MvcCoreZapatillasPractica.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

#region PROCEDURES
//CREATE PROCEDURE SP_FOTOS_ZAPAS_OUT
//(
//    @posicion INT,
//    @idzapa INT,
//    @registros INT OUT
//)
//AS
//BEGIN
//    SELECT @registros = COUNT(IMAGEN) 
//    FROM IMAGENESZAPASPRACTICA
//    WHERE IDPRODUCTO = @idzapa;

//SELECT IDIMAGEN, IDPRODUCTO, IMAGEN 
//    FROM (
//        SELECT 
//            CAST(ROW_NUMBER() OVER (ORDER BY IMAGEN) AS INT) AS POSICION,
//        IDIMAGEN,
//        IDPRODUCTO,
//        IMAGEN
//        FROM IMAGENESZAPASPRACTICA
//        WHERE IDPRODUCTO = @idzapa
//    ) AS QUERY
//    WHERE QUERY.POSICION = @posicion;
//END
//GO
#endregion

namespace MvcCoreZapatillasPractica.Repositories
{
    public class RepositoryZapas
    {
        private ZapatillasContext context;

        public RepositoryZapas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapas>> GetAllZapasAsync()
        {
            var zapas = await this.context.Zapas.ToListAsync();
            return zapas;
        }

        public async Task AddImagenZapa(ImagenesZapas imagenZapa)
        {
            context.ImagenesZapas.Add(imagenZapa);
            await context.SaveChangesAsync();
        }

        public async Task<Zapas> FindZapaAsync(int idzapa)
        {
            return await this.context.Zapas.FirstOrDefaultAsync(z => z.IdProducto == idzapa);
        }

        public async Task<List<ImagenesZapas>> GetImagenesZapa(int idzapa)
        {
            var ImagenesZapa = await this.context.ImagenesZapas
                .Where(z => z.IdProducto == idzapa)
                .ToListAsync();

            return ImagenesZapa;
        }

        public async Task<int> GetUltimoIdImagenZapa(int idzapa)
        {
            var ultimoIdImagen = await this.context.ImagenesZapas
                                            .MaxAsync(imagen => (int?)imagen.IdImagen);

            return ultimoIdImagen ?? 0;
        }


        public async Task<ModelImagenesZapas> GetFotosZapasAsync(int posicion, int idzapa)
        {
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamIdZapa = new SqlParameter("@idzapa", idzapa);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = System.Data.ParameterDirection.Output;

            var sql = "EXEC SP_FOTOS_ZAPAS_OUT @posicion, @idzapa, @registros OUT";
            var query = await this.context.ImagenesZapas
                .FromSqlRaw(sql, pamPosicion, pamIdZapa, pamRegistros)
                .ToListAsync();

            int registros = (int)pamRegistros.Value;

            return new ModelImagenesZapas
            {
                NumeroRegistros = registros,
                Imagenes = query
            };
        }

    }
}
