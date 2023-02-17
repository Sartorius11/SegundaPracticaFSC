using SegundaPracticaFSC.Models;
using System.Data;
using System.Data.SqlClient;

#region PROCEDURE INSERTAR SQL
/*CREATE OR ALTER PROCEDURE SP_INSERT_COMIC(@NOMBRE NVARCHAR(150),@IMAGEN NVARCHAR(600),
@DESCRIPCION NVARCHAR(500))
AS
DECLARE @ID INT
SELECT @ID = MAX(IDCOMIC)+1 FROM COMICS
INSERT INTO COMICS VALUES(@ID,@NOMBRE,@IMAGEN,@DESCRIPCION)
GO*/

#endregion

namespace SegundaPracticaFSC.Repositories
{
    
    public class RepositoryComicSql:IRepositoryComics
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataAdapter adapter;
        private DataTable tablaComics;

        public RepositoryComicSql()
        {
            string connectionString =
            @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;TrustServerCertificate=True;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            this.adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaComics = new DataTable();
            this.adapter.Fill(this.tablaComics);
        }


        public List<Comic> GetComics(){
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach(var row in consulta)
            {
                Comic comi = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comi);
            }
            return comics;
        }



      
        public void InsertarComic(string Nombre, string Imagen, string Descripcion)
        {
            string sql = "insert into comic values (@nombre "
                + ", @imagen, @descripcion)";

            SqlParameter pamnombre = new SqlParameter("@nombre", Nombre);
            this.com.Parameters.Add(pamnombre);
            SqlParameter pamimagen = new SqlParameter("@imagen", Imagen);
            this.com.Parameters.Add(pamimagen);

            SqlParameter pamdescripcion = new SqlParameter("@descripcion", Descripcion);
            this.com.Parameters.Add(pamdescripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }

}

