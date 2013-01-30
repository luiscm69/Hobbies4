using MyHobbie.Service.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyHobbie.Service
{
    public class TocarGuitarraService : ITocarGuitarraService
    {
       private GuitarraBL bl;

        public TocarGuitarraService()
        {
            bl = new GuitarraBL();
        }
        public void Registrar(string Marca, string Tipo)
        {
            try
            {
                bl.Registrar(Marca, Tipo);
            }
            catch (Exception)
            {
                throw new FaultException("Ya existe una Marca con el mismo nombre. Por favor, Ingrese otra.");
            }
        }

        public IList<GuitarraHobbie> Listar()
        {
            try
            {
                return bl.Listar();
            }
            catch (Exception)
            {
                throw new FaultException("Ocurrio un error al mostrar el listado.");
            }
        }
    }
}
