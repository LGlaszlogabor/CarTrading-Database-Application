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
    class Lekerdezes_vasarlasok
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter dataadapter;
        bool kapcsolodva;
        public Lekerdezes_vasarlasok()
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
        public DataSet lekerdez_kliens_szerint(string nev)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT AlkNev as Elado,Anev as Auto, Datum from Vasarlas, Kliensek, Autok, Alkalmazottak where Vasarlas.KID = Kliensek.KID and Vasarlas.AID = Autok.AID and Alkalmazottak.AlkID = Vasarlas.AlkID and Kliensek.Knev= '"+nev+"'", "Vasarlasok");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_vasarlasok()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT Knev as Vevo,AlkNev as Elado,Anev as Auto, Datum from Vasarlas, Kliensek, Autok, Alkalmazottak where Vasarlas.KID = Kliensek.KID and Vasarlas.AID = Autok.AID and Alkalmazottak.AlkID = Vasarlas.AlkID ", "Vasarlasok");
            lekapcsolodas();
            return ds;
        }
        public void uj_vasarlas(string autonev, string kliensnev, string alknev,string datum)
        {
            kapcsolodas();
            DataSet aid = futtat("SELECT AID from Autok WHERE Anev = '"+autonev+"'","aid");
            DataSet kid = futtat("SELECT KID from Kliensek WHERE Knev = '" + kliensnev + "'", "kid");
            DataSet alkid = futtat("SELECT AlkID from Alkalmazottak WHERE Alknev = '" + alknev + "'", "alkid");
            update("INSERT INTO Vasarlas (AID,KID,AlkID,Datum) VALUES(" + aid.Tables[0].Rows[0].Field<int>("AID") + "," + kid.Tables[0].Rows[0].Field<int>("KID") + "," + alkid.Tables[0].Rows[0].Field<int>("AlkID") +",'"+datum+ "')");
            lekapcsolodas();
        }
    }
}
