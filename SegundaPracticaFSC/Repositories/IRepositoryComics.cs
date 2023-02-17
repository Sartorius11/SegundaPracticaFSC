using SegundaPracticaFSC.Models;

namespace SegundaPracticaFSC.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();
        void InsertarComic(string Nombre,
            string Imagen, string Descripcion);
  
    }
}

