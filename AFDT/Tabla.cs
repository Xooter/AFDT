using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AFDT
{
    class Tabla
    {
        public TableLayoutPanel tabla;
        //no se por que esta esto XD
        public List<string> list_alf = new List<string>();
        //
        char[] lista_estados = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public Estado estado = new Estado();
        public List<string[]> lista_funcion = new List<string[]>();
        public Funcion funcion = new Funcion();

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
                comboBox.TabStop = false;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.Items.Insert(0, "");
                comboBox.Items.Insert(1, "→");
                comboBox.Items.Insert(2, "○");
                comboBox.SelectedIndexChanged += validateComboBox;

                TextBox txt_box = new TextBox();
                txt_box.Width = 30;
                txt_box.TextAlign = HorizontalAlignment.Center;
                txt_box.TabStop = false;
                txt_box.ReadOnly = true;

                
                tl.Controls.Add(comboBox, 0, i + 1);
                tl.Controls.Add(txt_box, 1, i + 1);

                estado.lista_estados.Add(lista_estados[i].ToString().ToLower());
                
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
                        list_alf.Add(j.ToString());

                        tl.Controls.Add(txt_box_alf, j + 2, 0);
                    }

                    TextBox txt_void_box = new TextBox();
                    txt_void_box.Width = 30;
                    txt_void_box.TextAlign = HorizontalAlignment.Center;
                    txt_void_box.KeyPress += validateData;

                    tl.Controls.Add(txt_void_box, j + 2, i + 1);
                    txt_void_box.Text = "";
                }

            }
            return tabla = tl;
        }//end generarTabla


        protected void validateData(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length > 0)
                textBox.Text = textBox.Text.Remove(0, 1);
            if (!estado.lista_estados.Contains(e.KeyChar.ToString()) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
            else
            {
                SendKeys.Send("{TAB}");
                textBox.BackColor = Color.White;
            }
        }//end
        
        private void validateComboBox(object sender, EventArgs e)
        {
            ComboBox s = sender as ComboBox;
            if (s.SelectedIndex == 1)
            {
                foreach (Control c in tabla.Controls)
                    if ((c is ComboBox) && (c as ComboBox).SelectedIndex == 1 && c != s)
                    {
                        (c as ComboBox).SelectedIndex = 0;
                    }
                estado.inicial = estado.lista_estados[tabla.GetPositionFromControl(s).Row - 1];
            }
            else if (s.SelectedIndex == 2)
            {
                if (!estado.lista_estados_aceptacion.Contains(estado.lista_estados[tabla.GetPositionFromControl(s).Row - 1]))
                    estado.lista_estados_aceptacion.Add(estado.lista_estados[tabla.GetPositionFromControl(s).Row - 1]);
            }
            if (s.SelectedIndex == 0 || s.SelectedIndex == 1)
            {
                estado.lista_estados_aceptacion.Remove(estado.lista_estados[tabla.GetPositionFromControl(s).Row - 1]);
            }


        }//end


        public void PintarEstadosValidos() 
        {
            for (int i = 0; i < tabla.RowCount;i++)
            {
                if (funcion.estados_validos.lista_estados.Contains(tabla.GetControlFromPosition(1, i + 1).Text.ToLower()))
                {
                    tabla.GetControlFromPosition(1, i + 1).BackColor = ColorTranslator.FromHtml("#B4CFB0");
                    for (int j = 2; j < tabla.ColumnCount; j++)
                    {
                        Random rnd = new Random();
                        Control txt_box = tabla.GetControlFromPosition(j, i + 1);
                        if (funcion.conjuntos_final.Count < 1)
                        {
                            for (int m = 0; m < funcion.conjuntos_cocientes.Count; m++)
                            {
                                if (funcion.conjuntos_cocientes[m].Contains(txt_box.Text))
                                {
                                    funcion.lista_funcion[i][j] = string.Join("C", m);
                                    (txt_box as TextBox).ReadOnly = true;
                                    (txt_box as TextBox).KeyPress -= validateData;
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < funcion.conjuntos_final.Count; k++)
                            {
                                if (funcion.conjuntos_final[k].Contains(txt_box.Text))
                                {
                                    txt_box.Text = "C" + k;
                                    (txt_box as TextBox).ReadOnly = true;
                                    (txt_box as TextBox).KeyPress -= validateData;
                                }
                            }
                        }

                    }
                }
                else
                {
                    for (int j = 1; j < tabla.ColumnCount; j++)
                    {
                        tabla.GetControlFromPosition(j, i + 1).BackColor = ColorTranslator.FromHtml("#FF7878");
                        (tabla.GetControlFromPosition(j, i + 1) as TextBox).ReadOnly = true;
                        (tabla.GetControlFromPosition(j, i + 1) as TextBox).KeyPress -= validateData;
                    }
                }
            }
        }
    }
}
