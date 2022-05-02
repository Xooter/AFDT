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

        //
        public Estado estados_validos = new Estado();
        public List<List<string>> conjuntos_cocientes = new List<List<string>>();
        public List<List<string>> conjuntos_final = new List<List<string>>();
        


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


        public string ResolverFuncion(List<string> lista_estados, string current_estado, List<string> lista_alfabeto, string current_simbolo) {
            for (int i = 0; i < lista_estados.Count; i++)
            {
                if (lista_estados[i] == current_estado)
                {
                    for (int j = 0; j < lista_alfabeto.Count; j++)
                    {
                        if (lista_alfabeto[j] == current_simbolo)
                            return lista_funcion[i].OfType<string>().ToList()[j];
                    }
                }
            }
            return null;
        }

        public void GetConvexos(List<string> lista_alf, Estado estados)
        {
            int[] count = new int[estados.lista_estados.Count];
            string next_state = estados.inicial;
            int index = estados.lista_estados.IndexOf(next_state);

            estados_validos = new Estado();
            estados_validos.inicial = estados.inicial;
            estados_validos.lista_estados_aceptacion = estados.lista_estados_aceptacion;
            conjuntos_cocientes.Clear();
            conjuntos_cocientes.Add(new List<string>() { estados.inicial });
            conjuntos_cocientes.Add(new List<string>());

            for (int i = 0; i < estados.lista_estados.Count; i++)
            {
                for (int j = 0; j < lista_alf.Count; j++)
                {

                    if (!estados_validos.lista_estados.Contains(next_state))
                    {
                        estados_validos.lista_estados.Add(next_state);
                        
                    }
                    if (!estados_validos.lista_estados_aceptacion.Contains(next_state) && !conjuntos_cocientes[0].Contains(next_state))
                        conjuntos_cocientes[0].Add(next_state);
                    if (estados_validos.lista_estados_aceptacion.Contains(next_state) && !conjuntos_cocientes[1].Contains(next_state))
                        conjuntos_cocientes[1].Add(next_state);


                    next_state = lista_funcion[index].OfType<string>().ToList()[count[index]];

                    if (count[index] < lista_alf.Count - 1)
                        count[index] += 1;
                    
                    index = estados.lista_estados.IndexOf(next_state);

                }
            }
         
        }//end
        
        public void ComprobarEquivalencia(Estado estados) {
            List<string> temp_list = new List<string>();
            conjuntos_final.Add(new List<string>() { estados.inicial });
            for (int i = 0; i < conjuntos_cocientes.Count; i++)
            {
                if (conjuntos_cocientes[i].OfType<string>().ToList().Count < 2)
                {
                    conjuntos_final.Add(new List<string>() { conjuntos_cocientes[i].OfType<string>().ToList()[0] });
                }
                else
                {
                    for (int j = 0; j < conjuntos_cocientes[i].OfType<string>().ToList().Count - 1; j++)
                    {
                        string estado_conjunto_cociente = conjuntos_cocientes[i].OfType<string>().ToList()[j];
                        int index = estados.lista_estados.IndexOf(estado_conjunto_cociente);

                        temp_list.Add(estado_conjunto_cociente);

                        string estado_conjunto_cociente_b = conjuntos_cocientes[i].OfType<string>().ToList()[j + 1];
                        int index_b = estados.lista_estados.IndexOf(estado_conjunto_cociente_b);

                        if (lista_funcion[index].OfType<string>().ToList().SequenceEqual(lista_funcion[index_b].OfType<string>().ToList()))
                        {
                            temp_list.Add(estado_conjunto_cociente_b);
                        }
                        else
                        {
                            bool agregar = true;
                            for (int m = 0; m < conjuntos_final.Count; m++)
                            {
                                string estado_conjunto_cociente_c = conjuntos_final[m][0];
                                int index_c = estados.lista_estados.IndexOf(estado_conjunto_cociente_c);
                                if (lista_funcion[index_c].OfType<string>().ToList().SequenceEqual(lista_funcion[index_b].OfType<string>().ToList()) &&
                                     index_c != index_b)
                                {
                                    conjuntos_final[m].Add(estado_conjunto_cociente_b);
                                    temp_list = new List<string>();
                                    agregar = false;
                                }
                                else
                                {
                                    agregar = true;
                                }
                            }
                            if (agregar)
                            {
                                conjuntos_final.Add(new List<string>() { estado_conjunto_cociente_b });
                                temp_list = new List<string>();
                            }
                        }
                    }
                }
                if(temp_list.Count != 0)
                    conjuntos_final.Add(temp_list);
                temp_list = new List<string>();
            }
            
        }//end
    }
}
