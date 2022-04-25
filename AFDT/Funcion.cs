using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AFDT
{
    class Funcion
    {
        public List<string[]> lista_funcion = new List<string[]>();
        string[] rowList = new string[0];
        public bool allDone = false;
        
        //generar matriz de las funciones
        //luego se usa para resolver y traducir
        public void GenerarFuncion(TableLayoutPanel tl) {
            lista_funcion.Clear();
            allDone = true;
            for (int r = 1; r < tl.RowCount + 1; r++)
            {
                rowList = new string[tl.ColumnCount];
                for (int c = 2; c < tl.ColumnCount; c++)
                {
                    Control control = tl.GetControlFromPosition(c, r);
                    if (control != null)
                    {
                        if (control.Text.Length > 0)
                            control.BackColor = ColorTranslator.FromHtml("#B4CFB0");
                        else
                        {
                            control.BackColor = ColorTranslator.FromHtml("#FF7878");
                            allDone = false;
                        }
                        rowList[c] = control.Text;
                    }
                }
                lista_funcion.Add(rowList);
            }
        }

        //update table
        public void UpdateTabla(TableLayoutPanel tableFunciones, Form1 form) {
            tableFunciones.Controls.Clear();
            tableFunciones.RowCount = form.estados.lista_estados.Count;
            tableFunciones.ColumnCount = form.alfabeto_entrada.lista_alf.Count + 2;
           
            // GENERAR TABLAS e.e
            for (int i = 0; i < form.estados.lista_estados.Count; i++)
            {

                TextBox txtBox_ = new TextBox();
                txtBox_.Width = 30;
                txtBox_.Text = "→";
                txtBox_.TextAlign = HorizontalAlignment.Center;
                txtBox_.TabStop = false;
                txtBox_.ReadOnly = true;

                TextBox txt_box = new TextBox();
                txt_box.Width = 30;
                txt_box.TextAlign = HorizontalAlignment.Center;
                txt_box.TabStop = false;
                txt_box.ReadOnly = true;

                if (form.estados.lista_estados[i] == form.estados.inicial)
                {
                    txtBox_.Text = "→";
                    tableFunciones.Controls.Add(txtBox_, 0, i + 1);
                }
                else if (form.estados.lista_estados_aceptacion.Contains(form.estados.lista_estados[i]))
                {
                    txtBox_.Text = "○";
                    tableFunciones.Controls.Add(txtBox_, 0, i + 1);
                }
                else
                {
                    txtBox_.Text = "";
                    tableFunciones.Controls.Add(txtBox_, 0, i + 1);
                }

                tableFunciones.Controls.Add(txt_box, 1, i + 1);
                txt_box.Text = form.estados.lista_estados[i];

                //DISPLAY: slots en blanco
                for (int j = 0; j < form.alfabeto_entrada.lista_alf.Count; j++)
                {
                    
                    if (i == 0)
                    {
                        TextBox txt_box_alf = new TextBox();
                        txt_box_alf.Width = 30;
                        txt_box_alf.TextAlign = HorizontalAlignment.Center;
                        txt_box_alf.TabStop = false;
                        txt_box_alf.ReadOnly = true;
                        txt_box_alf.Text = form.alfabeto_entrada.lista_alf[j];

                        tableFunciones.Controls.Add(txt_box_alf, j + 2, 0);
                    }

                    TextBox txt_void_box = new TextBox();
                    txt_void_box.Width = 30;
                    txt_void_box.TextAlign = HorizontalAlignment.Center;
                    if (tableFunciones.Name == "tableFunciones")
                        txt_void_box.KeyPress += form.validateData;
                    else
                        txt_void_box.KeyPress += form.validateData_salida;


                    tableFunciones.Controls.Add(txt_void_box, j + 2, i + 1);
                    txt_void_box.Text = "";

                }

            }
        }

        //esto se uso solo para el debug
        public string listToString() {
            string fullListString = string.Empty;
            foreach (string[] tiny_list in lista_funcion)
            {
                fullListString += string.Join(" ", tiny_list);
            }
            return fullListString;
        }

        
        public string ResolverFuncion(List<string> lista_estados, string current_estado,List<string> lista_alfabeto, string current_simbolo) {
            for (int i = 0; i < lista_estados.Count; i++)
            {
                if (lista_estados[i] == current_estado)
                {
                    for (int j = 0; j < lista_alfabeto.Count;j++)
                    {
                        if (lista_alfabeto[j] == current_simbolo)
                            return lista_funcion[i].OfType<string>().ToList()[j];
                    }
                }
            }
            return null;
        }
    }
}
