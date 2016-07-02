using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace Autokereskedes
{
    class Lekerdezes_kliensek
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter dataadapter;
        bool kapcsolodva;
        public Lekerdezes_kliensek()
        {
            connectionString = "Data Source=lg\\SQLEXPRESS;Initial Catalog=Auto_kereskedes;Integrated Security=True";
            kapcsolodva = false;
        }
        void kapcsolodas()
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                kapcsolodva = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Nem sikerult csatlakozni az adatbazishoz.");
                kapcsolodva = false;
            }
        }
        public DataSet futtat(string sql, string tablanev)
        {
            if (!kapcsolodva) kapcsolodas();
            try
            {
                dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "Autonevek");
                return ds;
            }
            catch (Exception e)
            {
                MessageBox.Show("Nem sikerult vegrehajtani a parancsot.");
                DataSet ds = new DataSet();
                ds.Tables.Add("Autonevek");
                return ds;
            }
        }
        public void update(string sql)
        {
            if (!kapcsolodva) kapcsolodas();
            try
            {
                dataadapter = new SqlDataAdapter(sql, connection);
                dataadapter.InsertCommand = new SqlCommand(sql, connection);
                dataadapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Nem sikerult vegrehajtani a parancsot.");
            }
        }
        void lekapcsolodas()
        {
            if (kapcsolodva) connection.Close();
            kapcsolodva = false;
        }
        public DataSet lekerdez_nev_szerint(string nev)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT * from Kliensek where KNev='"+nev+"'", "Kliensek");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_kliens_nevek()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT KNev as Nev from Kliensek", "Kliensek");
            lekapcsolodas();
            return ds;
        }
         public DataSet lekerdez_kliensek()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT KNev as Nev,Lakcim,Tel as Telefonszam from Kliensek", "Kliensek");
            lekapcsolodas();
            return ds;
        }
        public void uj_kliens(string nev, string cim, string tel){
             kapcsolodas();
            
            update("INSERT INTO Kliensek (Knev,Lakcim,Tel) VALUES('"+nev+"','"+cim+"',"+tel+")");
            lekapcsolodas();
        }
    }
}
