using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AFDT
{
    class Tabla
    {
        public TableLayoutPanel tabla;
        List<int> list_alf = new List<int>();
        char[] lista_estados = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public Tabla(TableLayoutPanel tl_parent,int columns,int rows) 
        {
            TableLayoutPanel tabla_nueva = new TableLayoutPanel();
            tabla_nueva.ColumnCount = columns + 2;
            tabla_nueva.RowCount = rows;
            tabla_nueva.Dock = DockStyle.Fill;
            tabla_nueva.AutoSize = true;
            tl_parent.Controls.Add(GenerarTabla(tabla_nueva));


        }//end constructor

        private TableLayoutPanel GenerarTabla(TableLayoutPanel tl) {
            tl.Controls.Clear();

            for (int i = 0; i < tl.RowCount; i++)
            {
                ComboBox comboBox = new ComboBox();
                comboBox.Width = 40;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.Items.Insert(0, "");
                comboBox.Items.Insert(1, "→");
                comboBox.Items.Insert(2, "○");
                //comboBox.SelectedIndexChanged += validateComboBox;

                TextBox txt_box = new TextBox();
                txt_box.Width = 30;
                txt_box.TextAlign = HorizontalAlignment.Center;
                txt_box.TabStop = false;
                txt_box.ReadOnly = true;

                
                    tl.Controls.Add(comboBox, 0, i + 1);

                tl.Controls.Add(txt_box, 1, i + 1);
                txt_box.Text = lista_estados[i].ToString();

                //DISPLAY: slots en blanco
                for (int j = 0; j < tl.ColumnCount - 2; j++)
                {

                    if (i == 0)
                    {
                        TextBox txt_box_alf = new TextBox();
                        txt_box_alf.Width = 30;
                        txt_box_alf.TextAlign = HorizontalAlignment.Center;
                        txt_box_alf.TabStop = false;
                        txt_box_alf.ReadOnly = true;
                        txt_box_alf.Text = j.ToString();

                        tl.Controls.Add(txt_box_alf, j + 2, 0);
                    }

                    TextBox txt_void_box = new TextBox();
                    txt_void_box.Width = 30;
                    txt_void_box.TextAlign = HorizontalAlignment.Center;
                    //txt_void_box.KeyPress += validateData;

                    tl.Controls.Add(txt_void_box, j + 2, i + 1);
                    txt_void_box.Text = "";

                }

            }
            return tabla = tl;
        }//end generarTabla


        /* protected void validateData(object sender, KeyPressEventArgs e)
         {
             e.Handled = !Char.IsPunctuation(e.KeyChar)
                 && e.KeyChar != (char)Keys.Back
                 && !lista_estados.Contains(e.KeyChar.ToString())
                 || e.KeyChar.ToString() == estados.inicial;

         }*/
        /*
        private void SelectInicial(object sender, EventArgs e)
        {
            tabla.Controls.Find()
        }*/

    }
}
