using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;



namespace Test.Data
{
    public static class DBConexion
    {
        public static IDbConnection Factory(string conexion)
        {
            string strCnn = Conexiones[conexion];
            return new SqlConnection(strCnn);
        }

        private static IDictionary<string, string> Conexiones = new Dictionary<string, string>();

        public static void AgregarConexiones(string nombreConn, string cs)
        {
            Conexiones[nombreConn] = cs;
        }
    }
}
