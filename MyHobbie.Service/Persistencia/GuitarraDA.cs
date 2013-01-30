using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MyHobbie.Service.Persistencia
{
    public class GuitarraDA
    {
        public IDataReader ObtenerEmpleados()
        {
            BaseDatos bd = new BaseDatos();
            bd.Open();
            bd.CreateCommand("dbo.usp_Listar", CommandType.StoredProcedure);

            return bd.ExecuteReader();
        }

        public void Registrar(string Marca, string Tipo)
        {
            using (BaseDatos bd = new BaseDatos())
            {
                bd.Open();
                bd.CreateCommand("dbo.usp_Registrar", CommandType.StoredProcedure);
                bd.CreateParameter("Marca", Marca,DbType.AnsiString);
                bd.CreateParameter("Tipo", Tipo, DbType.AnsiString);
                bd.ExecuteNonQuery();
            }
        }
    }
}