using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;

namespace Autokereskedes
{
    public class Lekerdezes_gyartok
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter dataadapter;
        bool kapcsolodva;
        public Lekerdezes_gyartok()
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
                MessageBox.Show("Nem hajthato vegre a parancs!");
            }
        }
        public void updatehoz(string sql)
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
                MessageBox.Show("Mar ez a ceg jelen van a kivalasztott orszagban!");
            }
        }
        void lekapcsolodas()
        {
            if (kapcsolodva) connection.Close();
            kapcsolodva = false;
        }
        public DataSet lekerdez_cegnevek()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT Cegnev from Gyartok", "Cegnevek");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_gyartok()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT GyID,Cegnev from Gyartok", "Gyartok");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_nev_szerint(string nev)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT GyID,Cegnev from Gyartok where Cegnev='"+nev+"'", "Gyartok");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_orszag(string nev)
        {
            kapcsolodas();
           
            DataSet ds = futtat("SELECT Orszag.OID,Onev as Orszagnev from Gyartok,Orszag,Gyartoszarmazas where Cegnev='" + nev + "' and GyartoSzarmazas.GYID = Gyartok.GyID and GyartoSzarmazas.Oid = Orszag.OID", "Gyartok");
            lekapcsolodas();
            return ds;
        }
        //////////////////////////Modositasok
        public void hozzarendel_gyarto_orszag(string gyartoID,string orszagID)
        {
            kapcsolodas();
            updatehoz("INSERT INTO GyartoSzarmazas (GyID, OID) VALUES ("+gyartoID+","+orszagID+")");
            lekapcsolodas();
        }
        public void uj_gyarto(string gyarto, int orszagid)
        {
            kapcsolodas();
            update("INSERT INTO Gyartok (Cegnev) VALUES ('" + gyarto + "')");
            DataSet ds = futtat("SELECT GYID from Gyartok WHERE Cegnev='" + gyarto + "'", "gyartoid");
            update("INSERT INTO GyartoSzarmazas (GyID, OID) VALUES (" + ds.Tables[0].Rows[0][0].ToString() + "," + orszagid+")");
            lekapcsolodas();

        }
        public void gyarto_torles(string ID)
        {
            kapcsolodas();
            update("DELETE from Gyartok WHERE GyID=" + ID);
            lekapcsolodas();
        }
    }
}
