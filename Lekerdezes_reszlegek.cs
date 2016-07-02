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
    class Lekerdezes_reszlegek
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter dataadapter;
        bool kapcsolodva;
        public Lekerdezes_reszlegek()
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
                dataadapter.Fill(ds, tablanev);
                return ds;
            }
            catch (Exception e)
            {
                MessageBox.Show("Nem sikerult vegrehajtani a parancsot.");
                DataSet ds = new DataSet();
                ds.Tables.Add(tablanev);
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
        public DataSet lekerdez_reszlegek()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT RID,Rnev as Reszlegnev, AlkNev as Manager from Reszlegek, Alkalmazottak WHERE ManID = AlkID", "Reszlegek");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_alkalmazottak(string reszleg)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT Alkalmazottak.AlkID,Alknev as Alkalmazottnev, Lakcim, Tel as Telefonszam, Fizetes from Reszlegek, Alkalmazottak,Dolgozik WHERE Dolgozik.Rid = Reszlegek.Rid and Alkalmazottak.AlkID = Dolgozik.AlkID and Rnev = '"+reszleg + "'", "Reszlegek");
            lekapcsolodas();
            return ds;
        }
    }
}
