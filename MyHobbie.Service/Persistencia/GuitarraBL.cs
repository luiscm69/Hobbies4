using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MyHobbie.Service.Persistencia
{
    public class GuitarraBL
    {
        public IList<GuitarraHobbie> Listar()
        {
            List<GuitarraHobbie> lista = new List<GuitarraHobbie>();
            GuitarraDA da = new GuitarraDA();

            GuitarraHobbie oBe = null;
            using (IDataReader reader = da.ObtenerEmpleados())
            {
                while (reader.Read())
                {
                    oBe = new GuitarraHobbie();
                    oBe.Id = Convert.ToInt32(reader["Id"]);
                    oBe.Marca = Convert.ToString(reader["Marca"]);
                    oBe.Tipo = Convert.ToString(reader["Tipo"]);
                    lista.Add(oBe);
                }
            }
            return lista;
        }

        public void Registrar(string Marca, string Tipo)
        {
            GuitarraDA da = new GuitarraDA();
            da.Registrar(Marca, Tipo);
        }
    }
}