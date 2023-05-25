using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioSincronizador.DAO
{
    public class Conexion
    {
        public SqlConnection conectar()
        {
            return cn();
        }
        public SqlConnection cn()
        {
            string nombreBaseDatos = ConfigurationManager.AppSettings["BDAddonRQ"];


            //SqlConnection cn = new SqlConnection("Server=209.45.52.78,61449;Database=" + nombreBaseDatos + ";User ID=sa;Password=$martcod3**85;Trusted_Connection=False");
            SqlConnection cn = new SqlConnection("Server=10.10.1.115;Database=" + nombreBaseDatos + ";User ID=sa;Password=@M1n3r4T1t4n2022@;Trusted_Connection=False");
            //SqlConnection cn = new SqlConnection("Server=DESKTOP-G3GP840;Database=" + nombreBaseDatos + ";User ID=sa;Password=123;Trusted_Connection=False");

            return cn;
        }
    }
}
