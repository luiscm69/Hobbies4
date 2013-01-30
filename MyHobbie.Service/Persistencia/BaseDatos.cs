using System;
using System.Data.Common;
using System.Configuration;
using System.Data;

namespace MyHobbie.Service.Persistencia
{
    /// <summary>
    /// Representa la base de datos en el sistema.
    /// Ofrece los métodos de acceso a misma.
    /// </summary>
    public class BaseDatos : IDisposable
    {

        private DbConnection conexion = null;
        private DbCommand comando = null;
        private DbTransaction transaccion = null;
        private string cadenaConexion;

        private static DbProviderFactory factory = null;

        public BaseDatos()
        {
            Configurar();
        }

        private void Configurar()
        {
            try
            {
                string proveedor = ConfigurationManager.ConnectionStrings["MyHobbieService"].ProviderName;
                this.cadenaConexion = ConfigurationManager.ConnectionStrings["MyHobbieService"].ConnectionString;
                BaseDatos.factory = DbProviderFactories.GetFactory(proveedor);
            }
            catch (ConfigurationException ex)
            {
                throw new BaseDatosException("Error al cargar la configuración del acceso a datos.", ex);
            }
        }

        public void Close()
        {
            if (this.conexion.State.Equals(ConnectionState.Open))
            {
                this.conexion.Close();
            }
        }

        public void Open()
        {
            if (this.conexion != null && !this.conexion.State.Equals(ConnectionState.Closed))
            {
                throw new BaseDatosException("La conexión ya se encuentra abierta.");
            }
            try
            {
                if (this.conexion == null)
                {
                    this.conexion = factory.CreateConnection();
                    this.conexion.ConnectionString = cadenaConexion;
                }
                this.conexion.Open();
            }
            catch (DataException ex)
            {
                throw new BaseDatosException("Error al conectarse a la base de datos.", ex);
            }
        }

        public void CreateCommand(string sentenciaSQL, CommandType TipoComandoSQL)
        {
            this.comando = factory.CreateCommand();
            this.comando.Connection = this.conexion;
            this.comando.CommandType = TipoComandoSQL;
            this.comando.CommandText = sentenciaSQL;
            if (this.transaccion != null)
            {
                this.comando.Transaction = this.transaccion;
            }
        }

        public void CreateParameter(string parameterName, object parameterValue)
        {
            DbParameter dbParam = comando.CreateParameter();
            dbParam.ParameterName = parameterName;
            dbParam.Value = parameterValue ?? System.DBNull.Value;
            comando.Parameters.Add(dbParam);
        }

        public void CreateParameter(string parameterName, object parameterValue, DbType dbType)
        {
            DbParameter dbParam = comando.CreateParameter();
            dbParam.ParameterName = parameterName;
            dbParam.Value = parameterValue ?? System.DBNull.Value;
            dbParam.DbType = dbType;
            comando.Parameters.Add(dbParam);
        }

        public void CreateParameter(string parameterName, object parameterValue, DbType dbType, int size)
        {
            DbParameter dbParam = comando.CreateParameter();
            dbParam.ParameterName = parameterName;
            dbParam.Value = parameterValue ?? System.DBNull.Value;
            dbParam.DbType = dbType;
            dbParam.Size = size;
            comando.Parameters.Add(dbParam);
        }

        public DbDataReader ExecuteReader()
        {
            return this.comando.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public object ExecuteScalar()
        {
            object escalar = null;
            try
            {
                escalar = this.comando.ExecuteScalar();
            }
            catch (InvalidCastException ex)
            {
                throw new BaseDatosException("Error al ejecutar un escalar.", ex);
            }
            return escalar;
        }

        public int ExecuteNonQuery()
        {
            return this.comando.ExecuteNonQuery();
        }

        public void BeginTransaction()
        {
            if (this.transaccion == null)
            {
                this.transaccion = this.conexion.BeginTransaction(IsolationLevel.ReadCommitted);
            }
        }

        public void CancelTransaction()
        {
            if (this.transaccion != null)
            {
                this.transaccion.Rollback();
            }
        }

        public void CommitTransaction()
        {
            if (this.transaccion != null)
            {
                this.transaccion.Commit();
            }
        }

        public void Dispose()
        {
            Close();
            conexion.Dispose();
        }
    }
}