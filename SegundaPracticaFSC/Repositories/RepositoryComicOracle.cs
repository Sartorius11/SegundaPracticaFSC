using Oracle.ManagedDataAccess.Client;
using SegundaPracticaFSC.Models;
using System.Data;
using System.Data.SqlClient;

#region Procedure Insertar ORACLE
/*create or replace procedure SP_INSERT_COMIC
(nombre nvarchar2 imagen nvarchar2, descripcion nvarchar2)
as
begin
  insert into COMICS VALUES((SELECT MAX(IDCOMICS+1)FROM COMICS),P_NOMBRE,P_IMAGEN,P_DESCRIPCION);
COMMIT;
END;*/
#endregion



namespace SegundaPracticaFSC.Repositories
{
    public class RepositoryComicOracle : IRepositoryComics 
    {
        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable tablaComics;

        public  RepositoryComicOracle()
        {
            string connectionString =
                @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            this.adapter = new OracleDataAdapter(sql, connectionString);
            this.tablaComics = new DataTable();
            this.adapter.Fill(this.tablaComics);
        }
        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
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
