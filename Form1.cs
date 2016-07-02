using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Autokereskedes
{
    public partial class Form1 : Form
    {
        Lekerdezes_autok sqlAutok;
        Lekerdezes_kliensek sqlKliensek;
        Lekerdezes_orszagok sqlOrszagok;
        Lekerdezes_gyartok sqlGyartok;
        Lekerdezes_vasarlasok sqlVasarlasok;
        Lekerdezes_reszlegek sqlReszlegek;
        Lekerdezes_alkalmazottak sqlAlkalmazottak;
        TreeNode kivalasztott;
        TreeNode jobbclick;
        List<Control> autok;
        List<Control> kliensek;
        bool auto_uj,auto_update;
        bool gyarto_uj, gyarto_update;
        bool alkalmazott_uj, alkalmazott_update;
        ContextMenuStrip docMenu;
        public Form1()
        {
            sqlAutok = new Lekerdezes_autok();
            sqlKliensek = new Lekerdezes_kliensek();
            sqlOrszagok = new Lekerdezes_orszagok();
            sqlGyartok = new Lekerdezes_gyartok();
            sqlVasarlasok = new Lekerdezes_vasarlasok();
            sqlReszlegek = new Lekerdezes_reszlegek();
            sqlAlkalmazottak = new Lekerdezes_alkalmazottak();
            InitializeComponent();
        }
        private void init_Autok()
        {
            ///////////////////////////////Ujgyarto feltoltes
            DataSet orsz = sqlOrszagok.lekerdez_orszagok();
            for (int i = 0; i < orsz.Tables[0].Rows.Count; i++)
                auto_ujgyartoorszag_beszuras_combo.Items.Add(orsz.Tables[0].Rows[i][1].ToString());
            auto_ujgyartoorszag_beszuras_combo.SelectedIndex = 0;
            /////////////////////////////////DatagridView feltoltes
            DataSet ds;
            ds = sqlAutok.lekerdez_autok();
            auto_tabla.DataSource = ds;
            auto_tabla.DataMember = "Autonevek";
            auto_tabla.Columns[0].Visible = false;
            ////////////////////////////////////Autok::::::Filter_feltoltes
            DataSet cegek = sqlAutok.lekerdez_cegnevek();
            auto_gyarto_combo.Items.Add("--------------------------------");
            for (int i = 0; i < cegek.Tables[0].Rows.Count; i++)
                auto_gyarto_combo.Items.Add(cegek.Tables[0].Rows[i][0].ToString());
            auto_gyarto_combo.SelectedIndex = 0;
            DataSet ferohely = sqlAutok.lekerdez_ferohely();
            auto_ferohely_combo.Items.Add("-----------------------------");
            for (int i = 0; i < ferohely.Tables[0].Rows.Count; i++)
                auto_ferohely_combo.Items.Add(ferohely.Tables[0].Rows[i][0].ToString());
            auto_ferohely_combo.SelectedIndex = 0;
            //////////////////////////////////Autok:::::::::Modositas_kezelok
            for (int i = 0; i < cegek.Tables[0].Rows.Count; i++)
                auto_ujgyarto_combo.Items.Add(cegek.Tables[0].Rows[i][0].ToString());   
        }
        private void init_Kliensek()
        {
            //////////////////////////////////////////datagridview_feltoltes
            DataSet ds = sqlKliensek.lekerdez_kliensek();
            kliensek_tabla.DataSource = ds;
            kliensek_tabla.DataMember = "Autonevek";
        }
        private void init_Gyartok()
        {
            //////////////////////////////Gyarto_panel
            DataSet gy = sqlGyartok.lekerdez_gyartok();
            gyartok_dataGridView.DataSource = gy;
            gyartok_dataGridView.DataMember = "Gyartok";
            gyartok_dataGridView.Columns[0].Visible = false;
            ////////////////////////////////Orszagok
            gy = sqlOrszagok.lekerdez_orszagok();
            gyartok_orszag_datagridView.DataSource = gy;
            gyartok_orszag_datagridView.DataMember = "Autonevek";
            gyartok_orszag_datagridView.Columns[0].Visible = false;
            ///////////////////////////////Ujgyarto orszag
            gyartok_uj_gyartoorszag_combo.Items.Clear();
            DataSet orsz = sqlOrszagok.lekerdez_orszagok();
            for (int i = 0; i < orsz.Tables[0].Rows.Count; i++)
                gyartok_uj_gyartoorszag_combo.Items.Add(orsz.Tables[0].Rows[i][1].ToString());
            gyartok_uj_gyartoorszag_combo.SelectedIndex = 0;
        }
        private void init_Reszlegek()
        {
            DataSet r = sqlReszlegek.lekerdez_reszlegek();
            reszlegek_dataGridView.DataSource = r;
            reszlegek_dataGridView.DataMember = "Reszlegek";
            reszlegek_dataGridView.Columns[0].Visible = false;
        }
        private void init_TreeView()
        {
            //////////////////////////Tree_view felepites
            treeView1.Nodes.Add("Autokereskedes");
            kivalasztott = treeView1.Nodes[0].Nodes.Add("Autok");
            treeView1.Nodes[0].Nodes.Add("Kliensek");
            treeView1.Nodes[0].Nodes.Add("Alakalmazottak");
            treeView1.Nodes[0].Nodes.Add("Gyartok");
            treeView1.Nodes[0].Nodes.Add("Reszlegek");
            treeView1.Nodes[0].Nodes.Add("Vasarlasok");
            treeView1.SelectedNode = kivalasztott;
            DataSet tmp = sqlAutok.lekerdez_cegnevek();
            for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
            {
                treeView1.Nodes[0].Nodes[0].Nodes.Add(tmp.Tables[0].Rows[i][0].ToString());
               // treeView1.Nodes[0].Nodes[3].Nodes.Add(tmp.Tables[0].Rows[i][0].ToString());
                DataSet tmp1 = sqlAutok.lekerdez_cegek_szerint(tmp.Tables[0].Rows[i][0].ToString());
                for (int j = 0; j < tmp1.Tables[0].Rows.Count; j++)
                    treeView1.Nodes[0].Nodes[0].Nodes[i].Nodes.Add(tmp1.Tables[0].Rows[j][1].ToString());
            }
            tmp = sqlKliensek.lekerdez_kliens_nevek();
            for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
            {
                treeView1.Nodes[0].Nodes[1].Nodes.Add(tmp.Tables[0].Rows[i][0].ToString());
            }
            tmp = sqlReszlegek.lekerdez_reszlegek();
            for (int i = 0; i < tmp.Tables[0].Rows.Count; i++)
            {
                treeView1.Nodes[0].Nodes[4].Nodes.Add(tmp.Tables[0].Rows[i][1].ToString());
            }
        }
        private void init_Alkalmazottak()
        {
            //////////////////////////////Alkalmazottak
            DataSet a = sqlAlkalmazottak.lekerdez_alkalmazottak();
            alkalmazottak_dataGridView.DataSource = a;
            alkalmazottak_dataGridView.DataMember = "Alkalmazottak";
            alkalmazottak_dataGridView.Columns[0].Visible = false;
            ////////////////////////////////Nyelvek
            a = sqlAlkalmazottak.lekerdez_nyelvek();
            alkalmazottak_nyelv_dataGridView.DataSource= a;
            alkalmazottak_nyelv_dataGridView.DataMember = "Alkalmazottak";
            alkalmazottak_nyelv_dataGridView.Columns[0].Visible = false;
            ///////////////////////////////Ujalkalmazott reszleg
            alkalmazottak_uj_reszleg_combo.Items.Clear();
            DataSet resz = sqlReszlegek.lekerdez_reszlegek();
            for (int i = 0; i < resz.Tables[0].Rows.Count; i++)
                alkalmazottak_uj_reszleg_combo.Items.Add(resz.Tables[0].Rows[i][1].ToString());
            alkalmazottak_uj_reszleg_combo.SelectedIndex = 0;
        }
        private void init_Vasarlasok()
        {
            DataSet r = sqlVasarlasok.lekerdez_vasarlasok();
            vasarlasok_dataGridView.DataSource = r;
            vasarlasok_dataGridView.DataMember = "Vasarlasok";
            hide_uj_vasarlasok();


            vasarlasok_uj_auto_combo.Items.Clear();
            DataSet autok = sqlAutok.lekerdez_autok();
            for (int i = 0; i < autok.Tables[0].Rows.Count; i++)
                vasarlasok_uj_auto_combo.Items.Add(autok.Tables[0].Rows[i][1].ToString());

            vasarlasok_uj_vevo_combo.Items.Clear();
            autok = sqlKliensek.lekerdez_kliensek();
            for (int i = 0; i < autok.Tables[0].Rows.Count; i++)
                vasarlasok_uj_vevo_combo.Items.Add(autok.Tables[0].Rows[i][0].ToString());

            vasarlasok_uj_elado_combo.Items.Clear();
            autok = sqlReszlegek.lekerdez_alkalmazottak("Eladas");
            for (int i = 0; i < autok.Tables[0].Rows.Count; i++)
                vasarlasok_uj_elado_combo.Items.Add(autok.Tables[0].Rows[i][1].ToString());   
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            /////////////////////////////Form inicializalas
            init_TreeView();
            init_Autok();
            init_Gyartok();
            init_Kliensek();
            init_Reszlegek();
            init_Alkalmazottak();
            init_Vasarlasok();
            /////////////////////////////////////// auto_form osszekapcsolas
            autok = new List<Control>();
            autok.Add(auto_tabla);
            autok.Add(auto_gyarto_combo);
            autok.Add(auto_nev_text);
            autok.Add(auto_nev_radio);
            autok.Add(auto_gyarto_radio);
            autok.Add(auto_nev_label);
            autok.Add(auto_aktiv_label);
            autok.Add(auto_gyarto_label);
            autok.Add(auto_ferohely_label);
            autok.Add(auto_ferohely_combo);
            autok.Add(auto_ferohely_radio);
            autok.Add(auto_ar_radio);
            autok.Add(auto_minar_numeric);
            autok.Add(auto_maxar_numeric);
            autok.Add(auto_ar_label);
            autok.Add(auto_darabszam_label);
            autok.Add(auto_darabszam_numeric);
            autok.Add(auto_uj_button);
            autok.Add(auto_modositas_button);
            autok.Add(auto_torles_button);
            //////////////////////////////////Kliens form osszekapcsolas
            kliensek = new List<Control>();
            kliensek.Add(kliensek_tabla);
            kliensek.Add(kliensek_uj_button);
            //////////////////////////////////////////Elsodleges lathatosag
            hide_uj();
            hide_uj_gyarto();
            hide_kliensek();
            hide_gyartok();
            hide_belso();
            gyarto_hide_uj();
            hide_reszlegek();
            hide_alkalmazottak();
            hide_vasarlasok();
            vasarlasok_uj_datum.Format = DateTimePickerFormat.Short;
            //////////////////////Jobb click menu
            docMenu = new ContextMenuStrip();
            ToolStripMenuItem openLabel = new ToolStripMenuItem();
            openLabel.Text = "KIVALASZT";
            ToolStripMenuItem deleteLabel = new ToolStripMenuItem();
            deleteLabel.Text = "UJ";
            ToolStripMenuItem renameLabel = new ToolStripMenuItem();
            renameLabel.Text = "TORLES";
            docMenu.Items.Add(openLabel);
            docMenu.Items.Add(deleteLabel);
            docMenu.Items.Add(renameLabel);
            treeView1.MouseUp += new MouseEventHandler(treeView1MouseUp);
            docMenu.Items[0].MouseDown += new MouseEventHandler(kivalaszt);
            docMenu.Items[1].MouseDown += new MouseEventHandler(uj_gomb);
            docMenu.Items[2].MouseUp += new MouseEventHandler(torol_gomb);
        }
        void hide_uj_menu(){
            docMenu.Items[1].Visible = false;
        }
        void hide_delete_menu(){
            docMenu.Items[2].Visible = false;
        }
        void show_uj_menu()
        {
            docMenu.Items[1].Visible = true;
        }
        void show_delete_menu()
        {
            docMenu.Items[2].Visible = true;
        }
        void uj_gomb(object sender, MouseEventArgs e)
        {
            kivalaszt(sender, e);
             if (jobbclick.Parent != null && jobbclick.Parent.Parent != null && jobbclick.Parent.Parent.Text == "Autok")
                        {
                        }
                    else if (jobbclick.Parent != null) switch (jobbclick.Parent.Text)
                            {
                                case "Autokereskedes":
                                    switch (jobbclick.Index)
                                    {
                                        case 0: //autok

                                            auto_uj_button_Click(sender, e);
                                            break;
                                        case 1://kliensek
                                            kliensek_uj_button_Click_1(sender, e);
                                            break;
                                        case 2://alkamazottak
                                            alkalmazotak_uj_button_Click(sender, e);
                                            break;
                                        case 4: //Reszlegek
                                            
                                            break;
                                        case 5: //Vasarlasok
                                            vasarlasok_uj_button_Click(sender, e);
                                            break;
                                        case 3: // gyartok
                                            gyartok_uj_gyartouj_button_Click(sender,e);
                                            break;

                                    }
                                    break;
                                case "Autok":
                                   
                                    break;
                                case "Kliensek":
                                   

                                    break;
                                case "Reszlegek":
                                  
                                    break;

                            }
                
        }
        void torol_gomb(object sender, MouseEventArgs e)
        {
            kivalaszt(sender, e);
            if (jobbclick.Parent != null && jobbclick.Parent.Parent != null && jobbclick.Parent.Parent.Text == "Autok")
            {
                sqlAutok.auto_nev_torles(jobbclick.Text);
                auto_tabla_frissit();
            }
            else if (jobbclick.Parent != null) switch (jobbclick.Parent.Text)
                {
                    case "Autokereskedes":
                        switch (jobbclick.Index)
                        {
                            case 0: //autok
                                MessageBox.Show("A torleshez valasz ki a tablabol egy elemet es nyomd meg a torles gombot!");
                                break;
                            case 1://kliensek
                               
                                break;
                            case 2://alkamazottak
                                MessageBox.Show("A torleshez valasz ki a tablabol egy elemet es nyomd meg a torles gombot!");
                                break;
                            case 4: //Reszlegek

                                break;
                            case 5: //Vasarlasok
                               
                                break;
                            case 3: // gyartok
                                MessageBox.Show("A torleshez valasz ki a tablabol egy elemet es nyomd meg a torles gombot!");
                                break;

                        }
                        break;
                    case "Autok":
                        
                        break;
                    case "Kliensek":


                        break;
                    case "Reszlegek":

                        break;

                }
        }
        void kivalaszt(object sender, MouseEventArgs e)
        {
            treeView1.SelectedNode = jobbclick;
        }
        void treeView1MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                jobbclick = treeView1.GetNodeAt(e.X, e.Y);
                if (treeView1.SelectedNode != null)
                {
                    docMenu.Show(treeView1, e.Location);
                    if (jobbclick != null && jobbclick.Parent != null && jobbclick.Parent.Parent != null && jobbclick.Parent.Parent.Text == "Autok")
                        {
                            hide_uj_menu();
                           show_delete_menu();
                        }
                    else if (jobbclick != null && jobbclick.Parent != null) switch (jobbclick.Parent.Text)
                            {
                                case "Autokereskedes":
                                    switch (jobbclick.Index)
                                    {
                                        case 0: //autok
                                            show_uj_menu();
                                            show_delete_menu();
                                            break;
                                        case 1://kliensek
                                            show_uj_menu();
                                            hide_delete_menu();
                                            break;
                                        case 2://alkamazottak
                                             show_uj_menu();
                                            show_delete_menu();
                                            break;
                                        case 4: //Reszlegek
                                            hide_uj_menu();
                                            hide_delete_menu();
                                            break;
                                        case 5: //Vasarlasok
                                            show_uj_menu();
                                            hide_delete_menu();
                                            break;
                                        case 3: // gyartok
                                            show_uj_menu();
                                            show_delete_menu();
                                            break;

                                    }
                                    break;
                                case "Autok":
                                    hide_uj_menu();
                                    hide_delete_menu();
                                    break;
                                case "Kliensek":
                                    hide_uj_menu();
                                    hide_delete_menu();

                                    break;
                                case "Reszlegek":
                                    hide_uj_menu();
                                    hide_delete_menu();
                                    break;

                            }
                }
            }
        }
        ///////////////////////////////////////////////////////////////Tree_view       
        public void show_autok()
        {
            for (int i = 0; i < autok.Count; i++)
            {
                autok[i].Visible = true;
            }
        }
        public void hide_autok()
        {
            for (int i = 0; i < autok.Count; i++)
            {
                autok[i].Visible = false;
            }
            hide_uj();
            hide_uj_gyarto();
        }
        public void show_kliensek()
        {
            for (int i = 0; i < kliensek.Count; i++)
            {
                kliensek[i].Visible = true;
            }
            hide_uj_kliensek();
        }
        public void hide_kliensek()
        {
            for (int i = 0; i < kliensek.Count; i++)
            {
                kliensek[i].Visible = false;
            }
            hide_uj_kliensek();
        }
        public void hide_gyartok()
        {
            gyartok_panel.Visible = false;
            hide_uj_gyarto();
        }
        public void show_gyartok() {
            gyartok_panel.Visible = true;
            hide_uj_gyarto();
        }
        public void show_reszlegek()
        {
            reszlegek_panel.Visible = true;
        }
        public void hide_reszlegek()
        {
            reszlegek_panel.Visible = false;
        }
        public void hide_belso()
        {
            belso_dataGridView.Visible = false;
        }
        public void show_belso()
        {
            belso_dataGridView.Visible = true;
        }
        public void show_alkalmazottak()
        {
            alkalmazottak_panel.Visible = true;
            alkalmazottak_hide_uj();
        }
        public void hide_alkalmazottak()
        {
            alkalmazottak_panel.Visible = false;
            alkalmazottak_hide_uj();
        }
        public void show_vasarlasok()
        {
            vasarlasok_panel.Visible = true;
            hide_uj_vasarlasok();
        }
        public void hide_vasarlasok()
        {
            vasarlasok_panel.Visible = false;
            hide_uj_vasarlasok();
        }
        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != kivalasztott)
            {
                if (treeView1.SelectedNode.Parent!=null &&  treeView1.SelectedNode.Parent.Parent != null && treeView1.SelectedNode.Parent.Parent.Text == "Autok")
                {
                    hide_autok();
                    hide_gyartok();
                    hide_kliensek();
                    hide_reszlegek();
                    hide_alkalmazottak();
                    hide_vasarlasok();
                    show_belso();
                    DataSet d = sqlAutok.lekerdez_szurt_nevek(treeView1.SelectedNode.Text.ToString());
                    belso_dataGridView.DataSource = d;
                    belso_dataGridView.DataMember = "Autonevek";
                    belso_dataGridView.Columns[0].Visible = false;
                }else if(treeView1.SelectedNode.Parent != null) switch (treeView1.SelectedNode.Parent.Text)
                {
                    case "Autokereskedes":
                         switch (treeView1.SelectedNode.Index)
                        {
                            case 0: //autok
                                    hide_belso();
                                    show_autok();
                                    hide_reszlegek();
                                    hide_kliensek();
                                    hide_gyartok();
                                    hide_alkalmazottak();
                                    hide_vasarlasok();
                                break;
                            case 1://kliensek
                                    hide_belso();
                                    hide_autok();
                                    hide_reszlegek();
                                    show_kliensek();
                                    hide_gyartok();
                                    hide_alkalmazottak();
                                    hide_vasarlasok();
                                break;
                            case 2://alkamazottak
                                hide_belso();
                                hide_autok();
                                hide_reszlegek();
                                hide_kliensek();
                                hide_gyartok();
                                hide_alkalmazottak();
                                show_alkalmazottak();
                                hide_vasarlasok();
                                break;
                            case 4: //Reszlegek
                                hide_autok();
                                hide_gyartok();
                                hide_kliensek();
                                hide_belso();
                                show_reszlegek();
                                hide_alkalmazottak();
                                hide_vasarlasok();
                                break;
                            case 5: //Vasarlasok
                                hide_autok();
                                hide_gyartok();
                                hide_kliensek();
                                hide_belso();
                                hide_reszlegek();
                                hide_alkalmazottak();
                                show_vasarlasok();
                                break;
                            case 3: // gyartok
                                    hide_belso();
                                    hide_reszlegek();
                                    hide_autok();
                                    hide_kliensek();
                                    show_gyartok();
                                    hide_alkalmazottak();
                                    hide_vasarlasok();
                                break;
                            
                        }
                        break;
                    case "Autok":
                        hide_autok();
                        hide_gyartok();
                        hide_kliensek();
                        hide_reszlegek();
                        hide_alkalmazottak();
                        hide_vasarlasok();
                        show_belso();
                        DataSet d = sqlAutok.lekerdez_cegek_szerint(treeView1.SelectedNode.Text.ToString());
                        belso_dataGridView.DataSource = d;
                        belso_dataGridView.DataMember = "Autonevek";
                        belso_dataGridView.Columns[0].Visible = false;
                    break;
                    case "Kliensek":
                        hide_autok();
                        hide_gyartok();
                        hide_kliensek();
                        hide_reszlegek();
                        hide_alkalmazottak();
                        hide_vasarlasok();
                        show_belso();
                        d = sqlVasarlasok.lekerdez_kliens_szerint(treeView1.SelectedNode.Text.ToString());
                        belso_dataGridView.DataSource = d;
                        belso_dataGridView.DataMember = "Vasarlasok";
                       
                    break;
                    case "Reszlegek":
                        hide_reszlegek();
                        hide_autok();
                        hide_gyartok();
                        hide_kliensek();
                        show_belso();
                        hide_alkalmazottak();
                        hide_vasarlasok();
                        d = sqlReszlegek.lekerdez_alkalmazottak(treeView1.SelectedNode.Text.ToString());
                        belso_dataGridView.DataSource = d;
                        belso_dataGridView.DataMember = "Reszlegek";
                        belso_dataGridView.Columns[0].Visible = false;
                    break;                   
                   
                }
                kivalasztott = treeView1.SelectedNode;
            }
        }
        ////////////////////////////////////////////Autok::::Filtervaltozas
        private void nev_TextChanged(object sender, EventArgs e)
        {
            if (auto_nev_radio.Checked)
            {
                DataSet ds = sqlAutok.lekerdez_szurt_nevek(auto_nev_text.Text);
                auto_tabla.DataSource = ds;
                auto_tabla.DataMember = "Autonevek";
            }
        }
        private void elso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (auto_gyarto_radio.Checked)
            {
                DataSet ds;
                if (auto_gyarto_combo.SelectedIndex != 0)
                {
                    ds = sqlAutok.lekerdez_cegek_szerint(auto_gyarto_combo.SelectedItem.ToString());
                }
                else ds = sqlAutok.lekerdez_autok();
                auto_tabla.DataSource = ds;
                auto_tabla.DataMember = "Autonevek";
            }
        }
        private void auto_ferohely_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (auto_ferohely_radio.Checked)
            {
                DataSet ds;
                if (auto_ferohely_combo.SelectedIndex != 0)
                {
                    ds = sqlAutok.lekerdez_ferohely_szerint(auto_ferohely_combo.SelectedItem.ToString());
                }
                else ds = sqlAutok.lekerdez_autok();
                auto_tabla.DataSource = ds;
                auto_tabla.DataMember = "Autonevek";
            }
        }
        private void auto_minar_numeric_ValueChanged(object sender, EventArgs e)
        {
            if (auto_ar_radio.Checked)
            {
                DataSet ds;
                ds = sqlAutok.lekerdez_ar_szerint(auto_minar_numeric.Value.ToString(), auto_maxar_numeric.Value.ToString());
                auto_tabla.DataSource = ds;
                auto_tabla.DataMember = "Autonevek";
            }
        }
        private void auto_maxar_numeric_ValueChanged(object sender, EventArgs e)
        {
            auto_minar_numeric_ValueChanged(sender, e);
        }
        private void auto_darabszam_numeric_ValueChanged(object sender, EventArgs e)
        {
            if ((int) auto_darabszam_numeric.Value != 0)
            {
               int lathato=auto_tabla.DisplayedRowCount(false);
                if (lathato > (int)auto_darabszam_numeric.Value)
                {     
                    for (int i = (int)auto_darabszam_numeric.Value; i < lathato-1; i++)
                    {
                        auto_tabla.Rows[i].Visible = false;
                    }
                }
                else
                {
                    if (lathato != auto_tabla.RowCount)
                    {
                        for (int i = lathato-1; i < auto_darabszam_numeric.Value; i++)
                        {
                            auto_tabla.Rows[i].Visible = true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < auto_tabla.RowCount; i++)
                {
                    auto_tabla.Rows[i].Visible = true;
                }
            }
        }
        ///////////////////////////////////////////////////Autok:::::Szurokivalasztas   
        private void neve_CheckedChanged(object sender, EventArgs e)
        {
            nev_TextChanged(sender,e);
        }
        private void gyartoja_CheckedChanged(object sender, EventArgs e)
        {
            elso_SelectedIndexChanged(sender, e);
        }
        private void auto_ferohely_radio_CheckedChanged(object sender, EventArgs e)
        {
            auto_ferohely_combo_SelectedIndexChanged(sender, e);
        }
        private void auto_ar_radio_CheckedChanged(object sender, EventArgs e)
        {
            auto_minar_numeric_ValueChanged(sender, e);
        }
       
        /////////////////////////////////////Autok:::::::::::modositasok
        private void hide_uj_gyarto()
        {
            auto_ujgyarto_beszuras_label.Visible = false;
            auto_ujgyartonev_beszuras_label.Visible = false;
            auto_ujgyartonev_beszuras_text.Visible = false;
            auto_ujgyartoorszag_beszuras_label.Visible = false;
            auto_ujgyartoorszag_beszuras_combo.Visible = false;
            autok_ujgyartohozzaad_button.Visible = false;
            auto_ujgyartomegse_button.Visible = false;
        }
        private void show_uj_gyarto()
        {
            auto_ujgyarto_beszuras_label.Visible = true;
            auto_ujgyartonev_beszuras_label.Visible = true;
            auto_ujgyartonev_beszuras_text.Visible = true;
            auto_ujgyartoorszag_beszuras_label.Visible = true;
            auto_ujgyartoorszag_beszuras_combo.Visible = true;
            autok_ujgyartohozzaad_button.Visible = true;
            auto_ujgyartomegse_button.Visible = true;
        }
        private void hide_uj()
        {
            auto_mentes_button.Visible = false;
            auto_megse_button.Visible = false;
            auto_ujnev_text.Visible = false;
            auto_ujnev_label.Visible = false;
            auto_ujkeszlet_text.Visible = false;
            auto_ujkeszlet_label.Visible = false;
            auto_ujszeriaszam_text.Visible = false;
            auto_ujszeriaszam_label.Visible = false;
            auto_ujferohely_text.Visible = false;
            auto_ujferohely_label.Visible = false;
            auto_ujgyarto_combo.Visible = false;
            auto_ujgyarto_label.Visible = false;
            auto_ujar_text.Visible = false;
            auto_ujar_label.Visible = false;
            auto_ujgyarto_checkbox.Visible = false;
        }
        private void show_uj()
        {
            auto_mentes_button.Visible = true;
            auto_megse_button.Visible = true;
            auto_ujnev_text.Visible = true;
            auto_ujnev_label.Visible = true;
            auto_ujkeszlet_text.Visible = true;
            auto_ujkeszlet_label.Visible = true;
            auto_ujszeriaszam_text.Visible = true;
            auto_ujszeriaszam_label.Visible = true;
            auto_ujferohely_text.Visible = true;
            auto_ujferohely_label.Visible = true;
            auto_ujgyarto_combo.Visible = true;
            auto_ujgyarto_label.Visible = true;
            auto_ujar_text.Visible = true;
            auto_ujar_label.Visible = true;
            auto_ujgyarto_checkbox.Visible = true;
        }
        private void auto_kilepes_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void auto_uj_button_Click(object sender, EventArgs e)
        {
            auto_megse_button_Click(sender, e);
            auto_uj = true;
            show_uj();
        }
        private void auto_megse_button_Click(object sender, EventArgs e)
        {
            auto_uj = false;
            auto_update = false;
            hide_uj();
            auto_ujar_text.Text = "";
            auto_ujnev_text.Text = "";
            auto_ujkeszlet_text.Text = "";
            auto_ujszeriaszam_text.Text = "";
            auto_ujferohely_text.Text = "";
            auto_ujgyarto_combo.SelectedIndex = -1;
            auto_ujgyarto_checkbox.Checked = false;
        }
        private void auto_modositas_button_Click(object sender, EventArgs e)
        {
            try
            {
                auto_megse_button_Click(sender,e);
                auto_update = true;
                auto_ujnev_text.Text = auto_tabla.SelectedRows[0].Cells[1].Value.ToString();
                auto_ujkeszlet_text.Text = auto_tabla.SelectedRows[0].Cells[2].Value.ToString();
                auto_ujszeriaszam_text.Text = auto_tabla.SelectedRows[0].Cells[3].Value.ToString();
                auto_ujferohely_text.Text = auto_tabla.SelectedRows[0].Cells[5].Value.ToString();
                auto_ujar_text.Text = auto_tabla.SelectedRows[0].Cells[4].Value.ToString();
                auto_ujgyarto_combo.SelectedItem = auto_tabla.SelectedRows[0].Cells[6].Value;
                show_uj();
            }
            catch(Exception ex){
                MessageBox.Show("Valassz ki egy sort!");
            }
          
        }
        private void auto_torles_button_Click(object sender, EventArgs e)
        {
            try
            {
                sqlAutok.auto_torles(auto_tabla.SelectedRows[0].Cells[0].Value.ToString());
                auto_tabla_frissit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Valassz ki egy sort!");
            }
        }
        public void auto_tabla_frissit()
        {
            //////////////////////////tree view
            treeView1.Nodes.Clear();
            init_TreeView();
            /////////////////////////filterek
            auto_ujgyarto_combo.Items.Clear();
            auto_gyarto_combo.Items.Clear();
            auto_ferohely_combo.Items.Clear();
            init_Autok();
            init_Gyartok();
            init_Vasarlasok();
        }
        private void auto_mentes_button_Click(object sender, EventArgs e)
        {
            if ((auto_ujnev_text.Text.Equals("")) || (auto_ujkeszlet_text.Text.Equals("")) || (auto_ujszeriaszam_text.Text.Equals("")) || (auto_ujferohely_text.Text.Equals("")) || (auto_ujar_text.Text.Equals("")) || (auto_ujgyarto_combo.SelectedItem==null))
            {
                MessageBox.Show("Toltsd ki az osszes mezot!");
            }
            else 
            {
                Double tmp;
                if (Double.TryParse(auto_ujkeszlet_text.Text.ToString(), out tmp) && Double.TryParse(auto_ujszeriaszam_text.Text.ToString(), out tmp) && Double.TryParse(auto_ujferohely_text.Text.ToString(), out tmp) && Double.TryParse(auto_ujar_text.Text.ToString(), out tmp))
                {
                    if (auto_uj)
                    {
                        sqlAutok.uj_auto(auto_ujnev_text.Text, auto_ujkeszlet_text.Text, auto_ujszeriaszam_text.Text, auto_ujferohely_text.Text, auto_ujar_text.Text, auto_ujgyarto_combo.SelectedItem.ToString());
                        auto_uj = false;
                    }
                    else if (auto_update)
                    {
                        sqlAutok.update_auto(auto_ujnev_text.Text, auto_ujkeszlet_text.Text, auto_ujszeriaszam_text.Text, auto_ujferohely_text.Text, auto_ujar_text.Text, auto_ujgyarto_combo.SelectedItem.ToString(), auto_tabla.SelectedRows[0].Cells[0].Value.ToString());
                        auto_update = false;
                    }
                    auto_tabla_frissit();
                    auto_megse_button_Click(sender, e);
                }
                else MessageBox.Show("Az ar, a keszlet, a ferohely es a szeriaszam numerikusak!");
            }
        }
        ///////////////////////////////Autok:::::ujgyarto_beszuras
        private void auto_ujgyarto_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (auto_ujgyarto_checkbox.Checked) show_uj_gyarto();
            else hide_uj_gyarto();
        }
        private void autok_ujgyartohozzaad_button_Click(object sender, EventArgs e)
        {
            Double r;
            if (Double.TryParse(auto_ujgyartonev_beszuras_text.Text.ToString(), out r))
            {
                MessageBox.Show("A gyartonev az nem numerikus!");
            }
            else
            {
                sqlGyartok.uj_gyarto(auto_ujgyartonev_beszuras_text.Text.ToString(), auto_ujgyartoorszag_beszuras_combo.SelectedIndex + 1);
                auto_tabla_frissit();
                hide_uj_gyarto();
            }
        }
        private void auto_ujgyartomegse_button_Click(object sender, EventArgs e)
        {
            hide_uj_gyarto();
            auto_ujgyarto_checkbox.Checked = false;
        }
        ///////////////////////////////Gyartok::::::modositasok
        private void gyarto_hide_uj()
        {
            gyartok_uj_gyarto_label.Visible = false;
            gyartok_uj_gyarto_textbox.Visible = false;
            gyartok_uj_gyartomegse_button.Visible = false;
            gyartok_uj_gyartomentes_button.Visible = false;
            gyartok_uj_gyartoorszag_combo.Visible = false;
            gyartok_uj_gyartoorszag_label.Visible = false;
        }
        private void gyarto_show_uj()
        {
            gyartok_uj_gyarto_label.Visible = true;
            gyartok_uj_gyarto_textbox.Visible = true;
            gyartok_uj_gyartomegse_button.Visible = true;
            gyartok_uj_gyartomentes_button.Visible = true;
            gyartok_uj_gyartoorszag_combo.Visible = true;
            gyartok_uj_gyartoorszag_label.Visible = true;
        }
        private void gyartok_uj_gyartouj_button_Click(object sender, EventArgs e)
        {
            gyartok_uj_gyartomegse_button_Click(sender, e);
            gyarto_uj = true;
            gyarto_show_uj();
        }
        private void gyartok_uj_gyartomegse_button_Click(object sender, EventArgs e)
        {
            gyarto_uj = false;
            gyarto_update = false;
            gyarto_hide_uj();
            auto_ujar_text.Text = "";
            gyartok_uj_gyarto_textbox.Text = "";
            gyartok_uj_gyartoorszag_combo.SelectedIndex = -1;
        }
        private void gyarto_tabla_frissit()
        {
            //////////////////////////tree view
            treeView1.Nodes.Clear();
            init_TreeView();
            /////////////////////////filterek
            auto_ujgyarto_combo.Items.Clear();
            auto_gyarto_combo.Items.Clear();
            auto_ferohely_combo.Items.Clear();
            init_Autok();
            init_Gyartok();
        }
        private void gyartok_uj_gyartomentes_button_Click(object sender, EventArgs e)
        {
            if ((gyartok_uj_gyarto_textbox.Text.Equals("")) || (gyartok_uj_gyartoorszag_combo.SelectedItem == null))
            {
                MessageBox.Show("Toltsd ki az osszes mezot!");
            }
            else
            {
                Double r;
                if (Double.TryParse(gyartok_uj_gyarto_textbox.Text.ToString(), out r))
                {
                    MessageBox.Show("A gyartonev az nem numerikus!");
                }
                else
                {
                    sqlGyartok.uj_gyarto(gyartok_uj_gyarto_textbox.Text.ToString(), auto_ujgyartoorszag_beszuras_combo.SelectedIndex + 1);
                    gyarto_tabla_frissit();
                    gyarto_hide_uj();
                }
            }
        }
        private void gyartok_torles_button_Click(object sender, EventArgs e)
        {
            try
            {
                sqlGyartok.gyarto_torles(gyartok_dataGridView.SelectedRows[0].Cells[0].Value.ToString());
                gyarto_tabla_frissit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Valassz ki egy sort!");
            }
        }
        private void gyartok_hozzarendel_button_Click(object sender, EventArgs e)
        {
            if(gyartok_dataGridView.SelectedRows.Count<1){
                MessageBox.Show("Valaszd ki hogy melyik gyartokat rendeled hozza melyik orszagokhoz!");
            }
            else if(gyartok_orszag_datagridView.SelectedRows.Count <1 ){
                MessageBox.Show("Nem valasztottal ki orszagot!");
            }
            else  for(int i=0;i<gyartok_dataGridView.SelectedRows.Count;i++){
                for (int j = 0; j < gyartok_orszag_datagridView.SelectedRows.Count; j++)
                {
                    sqlGyartok.hozzarendel_gyarto_orszag(gyartok_dataGridView.SelectedRows[i].Cells[0].Value.ToString(),gyartok_orszag_datagridView.SelectedRows[j].Cells[0].Value.ToString());
                }
            }
        }
        private void gyartok_mutat_button_Click(object sender, EventArgs e)
        {
            if (gyartok_dataGridView.SelectedRows.Count == 1)
            {
                DataSet o = sqlGyartok.lekerdez_orszag(gyartok_dataGridView.SelectedRows[0].Cells[1].Value.ToString());
                gyartok_orszag_datagridView.DataSource = o;
                gyartok_orszag_datagridView.DataMember = "Gyartok";
                gyartok_orszag_datagridView.Columns[0].Visible = false;
            }
            else MessageBox.Show("Valassz ki egy gyartot amelynek a szarmazasi orszagait latni akarod!");
        }
        private void gyartok_visszaallit_button_Click(object sender, EventArgs e)
        {
            init_Gyartok();
        }
        private void gyartok_orszaggyarto_button_Click(object sender, EventArgs e)
        {
            if (gyartok_orszag_datagridView.SelectedRows.Count == 1)
            {
                DataSet o = sqlOrszagok.lekerdez_gyartok(gyartok_orszag_datagridView.SelectedRows[0].Cells[1].Value.ToString());
                gyartok_dataGridView.DataSource = o;
                gyartok_dataGridView.DataMember = "Autonevek";
                gyartok_dataGridView.Columns[0].Visible = false;
            }
            else MessageBox.Show("Valassz ki egy gyartot amelynek a szarmazasi orszagait latni akarod!");
        }
        //////////////////////////////////////////Alkalmazottak::::modositasok
        private void alkalmazottak_hide_uj()
        {
            alkalmazottak_uj_cim_label.Visible = false;
            alkalmazottak_uj_cim_textbox.Visible = false;
            alkalmazottak_uj_megse_button.Visible = false;
            alkalmazottak_uj_mentes_button.Visible = false;
            alkalmazottak_uj_mentes_button.Visible = false;
            alkalmazottak_uj_nev_label.Visible = false;
            alkalmazottak_uj_nev_textbox.Visible = false;
            alkalmazottak_uj_reszleg_combo.Visible = false;
            alkalmazottak_uj_reszleg_label.Visible = false;
            alkalmazottak_uj_telefon_label.Visible = false;
            alkalmazottak_uj_telefon_textbox.Visible = false;
            alkalmazottak_uj_fizetes_textbox.Visible = false;
            alkalmazottak_uj_fizetes_label.Visible = false;
        }
        private void alkalmazottak_show_uj()
        {
            alkalmazottak_uj_cim_label.Visible = true;
            alkalmazottak_uj_cim_textbox.Visible = true;
            alkalmazottak_uj_megse_button.Visible = true;
            alkalmazottak_uj_mentes_button.Visible = true;
            alkalmazottak_uj_mentes_button.Visible = true;
            alkalmazottak_uj_nev_label.Visible = true;
            alkalmazottak_uj_nev_textbox.Visible = true;
            alkalmazottak_uj_reszleg_combo.Visible = true;
            alkalmazottak_uj_reszleg_label.Visible = true;
            alkalmazottak_uj_telefon_label.Visible = true;
            alkalmazottak_uj_telefon_textbox.Visible = true;
            alkalmazottak_uj_fizetes_textbox.Visible = true;
            alkalmazottak_uj_fizetes_label.Visible = true;
        }
        private void alkalmazotak_uj_button_Click(object sender, EventArgs e)
        {
            alkalmazottak_uj_megse_button_Click(sender, e);
            alkalmazott_uj = true;
            alkalmazottak_show_uj();
        }
        private void alkalmazottak_uj_megse_button_Click(object sender, EventArgs e)
        {
            alkalmazott_uj = false;
            alkalmazott_update = false;
            alkalmazottak_hide_uj();
            alkalmazottak_uj_nev_textbox.Text = "";
            alkalmazottak_uj_reszleg_combo.SelectedIndex = -1;
            alkalmazottak_uj_telefon_textbox.Text = "";
            alkalmazottak_uj_cim_textbox.Text = "";
            alkalmazottak_uj_fizetes_textbox.Text = "";
        }
        private void alkalmazottak_tabla_frissit()
        {
            //////////////////////////tree view
            treeView1.Nodes.Clear();
            init_TreeView();
            /////////////////////////filterek
            init_Alkalmazottak();
            init_Vasarlasok();
            init_Reszlegek();
        }
        private void alkalmazottak_uj_mentes_button_Click(object sender, EventArgs e)
        {
            if ((alkalmazottak_uj_nev_textbox.Text.Equals("")) || (alkalmazottak_uj_cim_textbox.Text.Equals("")) || (alkalmazottak_uj_telefon_textbox.Text.Equals("")) || (alkalmazottak_uj_fizetes_textbox.Text.Equals("")) ||  (alkalmazottak_uj_reszleg_combo.SelectedItem == null))
            {
                MessageBox.Show("Toltsd ki az osszes mezot!");
            }
            else
            {
                Double r;
                if (Double.TryParse(alkalmazottak_uj_nev_textbox.Text.ToString(), out r) || Double.TryParse(alkalmazottak_uj_cim_textbox.Text.ToString(), out r))
                {
                    MessageBox.Show("A nev es a cim az nem numerikus!");
                }
                else
                {
                    if (Double.TryParse(alkalmazottak_uj_telefon_textbox.Text.ToString(), out r) && Double.TryParse(alkalmazottak_uj_fizetes_textbox.Text.ToString(), out r))
                    {
                        if (alkalmazott_uj)
                        {
                            sqlAlkalmazottak.uj_alkalmazott(alkalmazottak_uj_nev_textbox.Text.ToString(), alkalmazottak_uj_cim_textbox.Text.ToString(), alkalmazottak_uj_telefon_textbox.Text.ToString(), alkalmazottak_uj_reszleg_combo.SelectedItem.ToString(), alkalmazottak_uj_fizetes_textbox.Text.ToString());
                            alkalmazott_uj = false;
                        }
                        else if (alkalmazott_update)
                        {
                            sqlAlkalmazottak.update_alkalmazott(alkalmazottak_uj_nev_textbox.Text.ToString(), alkalmazottak_uj_cim_textbox.Text.ToString(), alkalmazottak_uj_telefon_textbox.Text.ToString(), alkalmazottak_uj_reszleg_combo.SelectedItem.ToString(), alkalmazottak_uj_fizetes_textbox.Text.ToString());
                            alkalmazott_update = false;
                        }
                        alkalmazottak_tabla_frissit();
                        alkalmazottak_hide_uj();
                    }
                    else MessageBox.Show("A fizetes es a telefonszam numerikusak!");
                }
            }
        }
        private void alkalmazottak_modositas_button_Click(object sender, EventArgs e)
        {
            try
            {
                alkalmazottak_uj_megse_button_Click(sender, e);
                alkalmazott_update = true;
                alkalmazottak_uj_nev_textbox.Text = alkalmazottak_dataGridView.SelectedRows[0].Cells[1].Value.ToString();
                alkalmazottak_uj_cim_textbox.Text = alkalmazottak_dataGridView.SelectedRows[0].Cells[2].Value.ToString();
                alkalmazottak_uj_telefon_textbox.Text = alkalmazottak_dataGridView.SelectedRows[0].Cells[3].Value.ToString();
                alkalmazottak_uj_reszleg_combo.SelectedItem = alkalmazottak_dataGridView.SelectedRows[0].Cells[5].Value.ToString();
                alkalmazottak_uj_fizetes_textbox.Text = alkalmazottak_dataGridView.SelectedRows[0].Cells[4].Value.ToString();
                alkalmazottak_show_uj();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Valassz ki egy sort!");
            }
        }
        private void alkalmazottak_torles_button_Click(object sender, EventArgs e)
        {
            try
            {
                sqlAlkalmazottak.delete_alkalmazottak(alkalmazottak_dataGridView.SelectedRows[0].Cells[0].Value.ToString());
                alkalmazottak_tabla_frissit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Valassz ki egy sort!");
            }
        }
        private void alkalmazottak_nyelvtudas_Click(object sender, EventArgs e)
        {
            if (alkalmazottak_dataGridView.SelectedRows.Count == 1)
            {
                DataSet o = sqlAlkalmazottak.lekerdez_tudott_nyelvek(alkalmazottak_dataGridView.SelectedRows[0].Cells[0].Value.ToString());
                alkalmazottak_nyelv_dataGridView.DataSource = o;
                alkalmazottak_nyelv_dataGridView.DataMember = "Alkalmazottak";
                alkalmazottak_nyelv_dataGridView.Columns[0].Visible = false;
            }
            else MessageBox.Show("Valassz ki egy alkalmazottat akinek a nyelvtudasat latni akarod!");
        }
        private void alkalmazottak_visszaallit_Click(object sender, EventArgs e)
        {
            init_Alkalmazottak();
        }
        private void alkalmazottak_kiknyelv_Click(object sender, EventArgs e)
        {
            if (alkalmazottak_nyelv_dataGridView.SelectedRows.Count == 1)
            {
                DataSet o = sqlAlkalmazottak.lekerdez_nyelvet_tudok(alkalmazottak_nyelv_dataGridView.SelectedRows[0].Cells[0].Value.ToString());
                alkalmazottak_dataGridView.DataSource = o;
                alkalmazottak_dataGridView.DataMember = "Alkalmazottak";
                alkalmazottak_dataGridView.Columns[0].Visible = false;
            }
            else MessageBox.Show("Valassz ki egy nyelvet hogy lasd hogy kik tudnak ilyen nyelven!");
        }
        private void alkalmazottak_hozzarendel_Click(object sender, EventArgs e)
        {
            if (alkalmazottak_dataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Valaszd ki hogy melyik alkalmazottakat rendeled hozza melyik nyelvhez!");
            }
            else if (alkalmazottak_nyelv_dataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Nem valasztottal ki nyelvet!");
            }
            else
            {
                for (int i = 0; i < alkalmazottak_dataGridView.SelectedRows.Count; i++)
                {
                    for (int j = 0; j < alkalmazottak_nyelv_dataGridView.SelectedRows.Count; j++)
                    {
                        sqlAlkalmazottak.hozzarendel_alk_nyelv(alkalmazottak_dataGridView.SelectedRows[i].Cells[0].Value.ToString(), alkalmazottak_nyelv_dataGridView.SelectedRows[j].Cells[0].Value.ToString());
                    }
                }
                MessageBox.Show("Sikeres kapcsolat letesites!");
            }
        }
        private void alkalmazottak_kapcsolat_torol_Click(object sender, EventArgs e)
        {
            if (alkalmazottak_dataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Valaszd ki hogy melyik alkalmazottakat rendeled hozza melyik nyelvhez!");
            }
            else if (alkalmazottak_nyelv_dataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("Nem valasztottal ki nyelvet!");
            }
            else
            {
                for (int i = 0; i < alkalmazottak_dataGridView.SelectedRows.Count; i++)
                {
                    for (int j = 0; j < alkalmazottak_nyelv_dataGridView.SelectedRows.Count; j++)
                    {
                        sqlAlkalmazottak.torol_alk_nyelv(alkalmazottak_dataGridView.SelectedRows[i].Cells[0].Value.ToString(), alkalmazottak_nyelv_dataGridView.SelectedRows[j].Cells[0].Value.ToString());
                    }
                }
                MessageBox.Show("Sikeres kapcsolat torles!");
            }

        }
        ////////////////////////////////////////Kliensek::::::Modositas
        private void hide_uj_kliensek()
        {
            kliensek_megse_button_click.Visible = false;
            kliensek_mentes_button_click.Visible = false;
            kliensek_uj_cim_textbox.Visible = false;
            kliensek_uj_nev_textbox.Visible = false;
            kliensek_uj_tel_textbox.Visible = false;
            kliensek_uj_tel_label.Visible = false;
            kliensek_uj_nev_label.Visible = false;
            kliensek_uj_cim_label.Visible = false;
        }
        private void show_uj_kliensek()
        {
            kliensek_megse_button_click.Visible = true;
            kliensek_mentes_button_click.Visible = true;
            kliensek_uj_cim_textbox.Visible = true;
            kliensek_uj_nev_textbox.Visible = true;
            kliensek_uj_tel_textbox.Visible = true;
            kliensek_uj_tel_label.Visible = true;
            kliensek_uj_nev_label.Visible = true;
            kliensek_uj_cim_label.Visible = true;
        }
        private void kliensek_tabla_frissit()
        {
            //////////////////////////tree view
            treeView1.Nodes.Clear();
            init_TreeView();
            /////////////////////////filterek
            init_Kliensek();
            init_Vasarlasok();
        }
        private void kliensek_uj_button_Click_1(object sender, EventArgs e)
        {
            show_uj_kliensek();
        }
        private void kliensek_megse_button_click_Click(object sender, EventArgs e)
        {
            kliensek_uj_cim_textbox.Text = "";
            kliensek_uj_nev_textbox.Text = "";
            kliensek_uj_tel_textbox.Text = "";
            hide_uj_kliensek();
        }
        private void kliensek_mentes_button_click_Click(object sender, EventArgs e)
        {
            if ((kliensek_uj_nev_textbox.Text.Equals("")) || (kliensek_uj_tel_textbox.Text.Equals("")) || (kliensek_uj_cim_textbox.Text.Equals("")))
            {
                MessageBox.Show("Toltsd ki az osszes mezot!");
            }
            else
            {
                Double r;
                if (Double.TryParse(kliensek_uj_nev_textbox.Text, out r) || Double.TryParse(kliensek_uj_cim_textbox.Text, out r))
                {
                    MessageBox.Show("A kliensnev es klienscim az nem numerikus!");
                }
                else
                {
                    if (Double.TryParse(kliensek_uj_tel_textbox.Text, out r))
                    {
                        sqlKliensek.uj_kliens(kliensek_uj_nev_textbox.Text, kliensek_uj_cim_textbox.Text, kliensek_uj_tel_textbox.Text);
                        kliensek_tabla_frissit();
                        hide_uj_kliensek();
                    }
                    else MessageBox.Show("A telefonszam numerikus!");
                }
            }
        }
        ///////////////////////////////////Vasarlasok:::::::::::Modositas
        private void vasarlasok_tabla_frissit()
        {
            //////////////////////////tree view
            treeView1.Nodes.Clear();
            init_TreeView();
            init_Vasarlasok();
            init_Kliensek();
        }
        private void show_uj_vasarlasok()
        {
            vasarlasok_uj_auto_combo.Visible = true;
            vasarlasok_datum_label.Visible = true;
            vasarlasok_elado_label.Visible = true;
            vasarlasok_uj_elado_combo.Visible = true;
            vasarlasok_uj_vevo_combo.Visible = true;
            vasarlasok_vevo_label.Visible = true;
            vasarlasok_auto_label.Visible = true;
            vasarlasok_uj_datum.Visible = true;
            vasarlasok_uj_mentes_button.Visible = true;
            vasarasok_uj_megse_button.Visible = true;
        }
        private void hide_uj_vasarlasok()
        {
            vasarlasok_uj_auto_combo.Visible = false;
            vasarlasok_datum_label.Visible = false;
            vasarlasok_elado_label.Visible = false;
            vasarlasok_uj_elado_combo.Visible = false;
            vasarlasok_uj_vevo_combo.Visible = false;
            vasarlasok_vevo_label.Visible = false;
            vasarlasok_auto_label.Visible = false;
            vasarlasok_uj_datum.Visible = false;
            vasarlasok_uj_mentes_button.Visible = false;
            vasarasok_uj_megse_button.Visible = false;
        }
        private void vasarlasok_uj_button_Click(object sender, EventArgs e)
        {
            show_uj_vasarlasok();
        }
        private void vasarlasok_uj_mentes_button_Click(object sender, EventArgs e)
        {
            if ((vasarlasok_uj_vevo_combo.SelectedItem == null) || (vasarlasok_uj_elado_combo.SelectedItem == null) ||(vasarlasok_uj_auto_combo.SelectedItem == null))
            {
                MessageBox.Show("Toltsd ki az osszes mezot!");
            }
            else
            {
                  sqlVasarlasok.uj_vasarlas(vasarlasok_uj_auto_combo.SelectedItem.ToString(), vasarlasok_uj_vevo_combo.SelectedItem.ToString(), vasarlasok_uj_elado_combo.SelectedItem.ToString(),vasarlasok_uj_datum.Text);
                  vasarlasok_tabla_frissit();
                  hide_uj_vasarlasok();
            }
        }
        private void vasarasok_uj_megse_button_Click(object sender, EventArgs e)
        {
            hide_uj_vasarlasok();
            vasarlasok_uj_auto_combo.SelectedIndex = -1;
            vasarlasok_uj_elado_combo.SelectedIndex = -1;
            vasarlasok_uj_vevo_combo.SelectedIndex = -1;
            vasarlasok_uj_datum.Text = "";
        }
        /////////////////////////////////////maradek
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void auto_ujgyartoorszag_beszuras_label_Click(object sender, EventArgs e)
        {

        }
        private void gyartok_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
      }
}
