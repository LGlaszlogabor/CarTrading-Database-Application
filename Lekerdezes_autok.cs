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
    public class Lekerdezes_autok
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter dataadapter;
        bool kapcsolodva;
        public Lekerdezes_autok()
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
        public DataSet lekerdez_autok()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT AID,ANev as Autonev, Keszlet, Szeriaszam, Ar, Ferohely, Cegnev as Gyarto FROM Autok as a,Gyartok as gy where a.GyID=gy.GYID", "Autonevek");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_szurt_nevek(string szuro)
        {
            kapcsolodas();
            if (szuro.Contains("SELECT") || szuro.Contains("select")) szuro = "";
            szuro += "%'";
            DataSet ds = futtat("SELECT AID,ANev as Autonev, Keszlet, Szeriaszam, Ar, Ferohely, Cegnev as Gyarto FROM Autok as a,Gyartok as gy where a.GyID=gy.GYID and ANev like '" + szuro, "Autonevek");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_cegnevek()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT Cegnev from Gyartok", "Cegnevek");
            lekapcsolodas();
            return ds;
        }

        public DataSet lekerdez_cegek_szerint(string s)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT AID,ANev as Autonev, Keszlet, Szeriaszam, Ar, Ferohely, Cegnev as Gyarto FROM Autok as a,Gyartok as gy where a.GyID=gy.GYID and gy.Cegnev='" + s + "'", "Cegnevek");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_ferohely()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT DISTINCT Ferohely from Autok", "Ferohely");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_ferohely_szerint(string s)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT AID,ANev as Autonev, Keszlet, Szeriaszam, Ar, Ferohely, Cegnev as Gyarto FROM Autok as a,Gyartok as gy where a.GyID=gy.GYID and Ferohely=" + s + "", "Ferohely");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_ar_szerint(string min, string max)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT AID,ANev as Autonev, Keszlet, Szeriaszam, Ar, Ferohely, Cegnev as Gyarto FROM Autok as a,Gyartok as gy where a.GyID=gy.GYID and Ar>=" + min + " and Ar<="+max, "Ar");
            lekapcsolodas();
            return ds;
        }
        //////////////////////////Modositasok
        public void uj_auto(string nev, string keszlet, string szeriaszam, string ferohely, string ar, string gyarto){
            kapcsolodas();
            DataSet ds = futtat("SELECT GYID from Gyartok WHERE Cegnev='"+gyarto+"'","gyartoid");
            update("INSERT INTO Autok (Anev,Ferohely,Szeriaszam,Keszlet,GYID,Ar) VALUES ('" + nev + "'," + ferohely + "," + szeriaszam + "," + keszlet + "," + ds.Tables[0].Rows[0].Field<int>("GYID") + "," + ar + ")");
            lekapcsolodas();
           
        }
        public void update_auto(string nev, string keszlet, string szeriaszam, string ferohely, string ar, string gyarto,string AID)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT GYID from Gyartok WHERE Cegnev='" + gyarto + "'", "gyartoid");
            update("UPDATE Autok SET Anev='" + nev + "',Ferohely=" + ferohely + ",Szeriaszam=" + szeriaszam + ",Keszlet=" + keszlet + ",GyID=" + ds.Tables[0].Rows[0].Field<int>("GYID") + ",Ar=" + ar + " WHERE AID=" + AID);
            lekapcsolodas();
        }
        public void auto_torles(string ID)
        {
            kapcsolodas();
            update("DELETE from Autok WHERE AID="+ID);
            lekapcsolodas();
        }
        public void auto_nev_torles(string nev)
        {
            kapcsolodas();
            update("DELETE from Autok WHERE Anev='" + nev+"'");
            lekapcsolodas();
        }
    }
}
