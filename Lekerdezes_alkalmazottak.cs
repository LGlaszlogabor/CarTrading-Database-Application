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
    public class Lekerdezes_alkalmazottak
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter dataadapter;
        bool kapcsolodva;
        public Lekerdezes_alkalmazottak()
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
        public void updatedel(string sql)
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
                MessageBox.Show("Nem hajthato vegre a parancs! Managert nem torolhetunk!");
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
                MessageBox.Show("Mar tud ezek a nyelvek kozul parat!");
            }
        }
        public void update1(string sql)
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
               
            }
        }
        void lekapcsolodas()
        {
            if (kapcsolodva) connection.Close();
            kapcsolodva = false;
        }
        public DataSet lekerdez_alkalmazottak()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT Alkalmazottak.AlkID, Alknev as Nev, Lakcim, Tel as Telefonszam, Fizetes, Rnev as Reszleg from Alkalmazottak,Dolgozik, Reszlegek WHERE Dolgozik.AlkID = Alkalmazottak.AlkID and Dolgozik.Rid = Reszlegek.Rid", "Alkalmazottak");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_tudott_nyelvek(String AlkID)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT Nyelvek.NyID, NyNev as Nyelv from Nyelvek,NyelvTudas,Alkalmazottak WHERE Nyelvek.NyID = NyelvTudas.NyID and Nyelvtudas.AlkID = Alkalmazottak.AlkID and Alkalmazottak.AlkID =" + AlkID, "Alkalmazottak");
            lekapcsolodas();
            return ds;
        }
        public DataSet lekerdez_nyelvet_tudok(String Nyid)
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT Alkalmazottak.AlkID, Alknev as Nev, Lakcim, Tel as Telefonszam, Fizetes, Rnev as Reszleg from Nyelvek,NyelvTudas,Alkalmazottak,Dolgozik, Reszlegek WHERE Dolgozik.AlkID = Alkalmazottak.AlkID and Dolgozik.Rid = Reszlegek.Rid and Nyelvek.NyID = NyelvTudas.NyID and Nyelvtudas.AlkID = Alkalmazottak.AlkID and Nyelvek.NyID =" + Nyid, "Alkalmazottak");
            lekapcsolodas();
            return ds;
        }
        public void hozzarendel_alk_nyelv(String alkID, String nyID)
        {
            kapcsolodas();
            update("INSERT INTO NyelvTudas (AlkID, NyID) VALUES (" + alkID + "," + nyID + ")");
            lekapcsolodas();
        }
        public void torol_alk_nyelv(String alkID, String nyID)
        {
            kapcsolodas();
            update1("DELETE FROM NyelvTudas WHERE AlkID =" + alkID + " and NyID = " + nyID );
            lekapcsolodas();
        }
        public DataSet lekerdez_nyelvek()
        {
            kapcsolodas();
            DataSet ds = futtat("SELECT NyID, NyNev as Nyelv from Nyelvek", "Alkalmazottak");
            lekapcsolodas();
            return ds;
        }
        public void delete_alkalmazottak(string id)
        {
            kapcsolodas();
            updatedel("DELETE from Alkalmazottak WHERE AlkID=" + id);
            lekapcsolodas();
        }
        public void uj_alkalmazott(string nev, string cim, string telefon, string reszleg, string fizetes)
        {
            kapcsolodas();
            DataSet rid = futtat("SELECT Rid from Reszlegek WHERE Rnev='" + reszleg + "'", "reszlegid");
            update("INSERT INTO Alkalmazottak (Alknev,Lakcim,Tel,Fizetes) VALUES ('" + nev + "','" + cim + "'," + telefon + "," + fizetes + ")");
            DataSet alkid = futtat("SELECT AlkID from Alkalmazottak WHERE Alknev='" + nev + "'", "alkid");
            update("INSERT INTO Dolgozik (AlkID,Rid) VALUES (" + alkid.Tables[0].Rows[0].Field<int>("AlkID") + "," + rid.Tables[0].Rows[0].Field<int>("Rid") + ")");
            lekapcsolodas();

        }
        public void update_alkalmazott(string nev, string cim, string telefon, string reszleg, string fizetes)
        {
            kapcsolodas();
            DataSet rid = futtat("SELECT Rid from Reszlegek WHERE Rnev='" + reszleg + "'", "reszlegid");
            DataSet alkid = futtat("SELECT AlkID from Alkalmazottak WHERE Alknev='" + nev + "'", "alkid");
            update("UPDATE Alkalmazottak SET Alknev='" + nev + "',Lakcim='" + cim + "',Tel=" + telefon + ",Fizetes=" + fizetes + " WHERE AlkID=" + alkid.Tables[0].Rows[0].Field<int>("AlkID"));
            DataSet regirid = futtat("SELECT Rid from Alkalmazottak,Dolgozik WHERE Alknev='" + nev + "' and Dolgozik.Alkid = Alkalmazottak.AlkId", "reszlegid");
            update("UPDATE Dolgozik SET AlkID=" + alkid.Tables[0].Rows[0].Field<int>("AlkID") + ",Rid=" + rid.Tables[0].Rows[0].Field<int>("Rid") + "WHERE AlkID=" + alkid.Tables[0].Rows[0].Field<int>("AlkID") + "and Rid=" + regirid.Tables[0].Rows[0].Field<int>("Rid") );
            lekapcsolodas();

        }
    }
}
