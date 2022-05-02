using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AFDT
{
    public partial class Form1 : Form
    {

        


        public Alfabeto alfabeto_entrada = new Alfabeto();
        public Alfabeto alfabeto_salida = new Alfabeto();
        public Estado estados = new Estado();
        Funcion funcion_transicion = new Funcion();
        Funcion funcion_salida = new Funcion();
        List<Tabla> lista_tablas = new List<Tabla>();


        //AFDT = (ΣE, ΣS, Q, q0, A, f, g)
        //ΣE: Alfabeto de símbolos de entrada
        //ΣS : Alfabeto de símbolos de salida
        //Q : Conjunto finito y no vacío, de estados posibles
        //q0 : Estado inicial de operación, q0  Q
        //A : Conjunto de estados de aceptación, A  Q
        //f : Función de transición, f: Q  ΣE → Q
        //g : Función de salida, g: Q  ΣE → ΣS
        // TODO: inputs al hacer split detectan espacio, queda raro generan tabla


        public Form1()
        {
            InitializeComponent();

            initializeLogic();
        }

        private void initializeLogic()
        {
            alfabeto_entrada.lista_alf = new List<string>() { "a", "b", "c" };
            alfabeto_salida.lista_alf = new List<string>() { "d", "e", "f" };
            estados.lista_estados = new List<string>() { "p", "q", "r", "s" };
            txt_alfabeto_entrada.Text = "a - b - c";
            txt_alfabeto_salida.Text = "d - e - f";
            txt_estados.Text = "p - q - r - s";
            estados.inicial = estados.lista_estados[0];
            txt_AFDT.Text = "AFDT = " + "( { " + string.Join(", ", alfabeto_entrada.lista_alf) + " }, { " + string.Join(", ", alfabeto_salida.lista_alf) + " }, { " + string.Join(", ", estados.lista_estados) + " }, " + estados.inicial + " , { Ø }, f, g )";
            txt_error.ForeColor = Color.Red;

            combo_inicial.SelectedIndexChanged += SelectInicial;
            
            UpdateTabla();
            UpdateComboBox();
            combo_inicial.Text = estados.lista_estados[0];

        }

        public void UpdateTabla() {
            funcion_transicion.UpdateTabla(tableFunciones,this);
            funcion_salida.UpdateTabla(table_funciones_salida,this);
        }

        public void UpdateComboBox() {
            combo_inicial.Items.Clear();

            foreach (string c in estados.lista_estados)
                combo_inicial.Items.Add(c);

            combo_inicial.Text = estados.lista_estados[0];
            estados.inicial = estados.lista_estados[0];
        }
        
        private void SelectInicial(object sender, EventArgs e) {
            estados.inicial = (string)combo_inicial.SelectedItem;
            UpdateTabla();
        }

        //validar si cada slot de la tabla tiene una letra valida de longitud 1
        //(asegurar que no se pongan 2 palabras en el mismo slot)
        //****************************************//

        public void validateData(object sender, KeyPressEventArgs e) {
            TextBox textBox = sender as TextBox;

            if (textBox.Text.Length > 0)
                textBox.Text = textBox.Text.Remove(0, 1);
            if (!estados.lista_estados.Contains(e.KeyChar.ToString()))
                e.Handled = true;
            else
            {
                SendKeys.Send("{TAB}");
                textBox.BackColor = Color.White;
            }
        }
        public void validateData_salida(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length > 0)
                textBox.Text = textBox.Text.Remove(0, 1);
            if (!alfabeto_salida.lista_alf.Contains(e.KeyChar.ToString()))
                e.Handled = true;
            else
            {
                SendKeys.Send("{TAB}");
                textBox.BackColor = Color.White;
            }

        }
        protected void validateData_traduccion(object sender, KeyPressEventArgs e)
        {
            e.Handled = !alfabeto_entrada.lista_alf.Contains(e.KeyChar.ToString()) && e.KeyChar != (char)Keys.Back;
                
        }
        protected void validateData_aceptacion(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Char.IsPunctuation(e.KeyChar)
                && e.KeyChar != (char)Keys.Back
                && !estados.lista_estados.Contains(e.KeyChar.ToString())
                || e.KeyChar.ToString() == estados.inicial;

        }
        //****************************************//

        //get enter de inputs
        //****************************************//
        private void input_entrada_KeyDown(object sender, KeyEventArgs e)
        {
            txt_error.Text = "";

            if (e.KeyCode == Keys.Enter && input_entrada.Text.Length > 0
              && funcion_salida.allDone
              && funcion_transicion.allDone)
            {
                //traduccion
                List<string> alf_para_traducir = new List<string>(Regex.Replace(input_entrada.Text, @"\s+", "").Select(x => x.ToString()).ToList());

                string estado_actual = estados.inicial;
                string cadena_salida = string.Empty;

                foreach (string simbolo in alf_para_traducir)
                {
                    cadena_salida += " " + funcion_salida.ResolverFuncion(estados.lista_estados, estado_actual, alfabeto_entrada.lista_alf, simbolo);
                    estado_actual = funcion_transicion.ResolverFuncion(estados.lista_estados, estado_actual, alfabeto_entrada.lista_alf, simbolo);
                }


                txt_resultado.Text = cadena_salida;
                e.SuppressKeyPress = true;
                //
            }
            else
            {
                txt_error.Text = "Error al traducir!";
            }
        }


        private void input_alfabeto_entrada_KeyDown(object sender, KeyEventArgs e)
        {
            input_alfabeto_entrada.ForeColor = Color.Black;
            if (e.KeyCode == Keys.Enter && input_alfabeto_entrada.Text.Length > 0)
            {
                alfabeto_entrada.lista_alf.Clear();
                List<string> new_alf =  new List<string>(Regex.Replace(input_alfabeto_entrada.Text, @"\s+", "").Select(x => x.ToString()).ToList());
                if (alfabeto_entrada.validate_alf(new_alf))
                {
                    alfabeto_entrada.lista_alf = new_alf;
                    txt_alfabeto_entrada.Text = String.Join(" - ", alfabeto_entrada.lista_alf);
                    txt_alfabeto_entrada.ForeColor = Color.Black;
                    input_alfabeto_entrada.ForeColor = Color.Green;
                    UpdateTabla();
                    txt_AFDT.Text = "AFDT = " + "({ " + string.Join(", ", alfabeto_entrada.lista_alf) + " }, { " + string.Join(", ", alfabeto_salida.lista_alf) + " }, { " + string.Join(", ", estados.lista_estados) + " }, '" + estados.inicial + "' , {" + string.Join(", ", estados.lista_estados_aceptacion) + "}, f, g)";
                }
                else
                {
                    txt_alfabeto_entrada.ForeColor = Color.Red;
                    input_alfabeto_entrada.ForeColor = Color.Red;
                    txt_alfabeto_entrada.Text = "ERROR!";

                }

                e.SuppressKeyPress = true;
            }
        }

        private void input_alfabeto_salida_KeyDown(object sender, KeyEventArgs e)
        {
            input_alfabeto_salida.ForeColor = Color.Black;
            if (e.KeyCode == Keys.Enter && input_alfabeto_salida.Text.Length > 0)
            {
                alfabeto_salida.lista_alf.Clear();
                List<string> new_alf = new List<string>(Regex.Replace(input_alfabeto_salida.Text, @"\s+", "").Select(x => x.ToString()).ToList());
                if (alfabeto_salida.validate_alf(new_alf))
                {
                    alfabeto_salida.lista_alf = new_alf;
                    txt_alfabeto_salida.Text = String.Join(" - ", alfabeto_salida.lista_alf);
                    txt_alfabeto_salida.ForeColor = Color.Black;
                    input_alfabeto_salida.ForeColor = Color.Green;
                    txt_AFDT.Text = "AFDT = " + "({ " + string.Join(", ", alfabeto_entrada.lista_alf) + " }, { " + string.Join(", ", alfabeto_salida.lista_alf) + " }, { " + string.Join(", ", estados.lista_estados) + " }, '" + estados.inicial + "' , {" + string.Join(", ", estados.lista_estados_aceptacion) + "}, f, g)";

                }
                else
                {
                    txt_alfabeto_salida.ForeColor = Color.Red;
                    input_alfabeto_salida.ForeColor = Color.Red;
                    txt_alfabeto_salida.Text = "ERROR!";

                }
                e.SuppressKeyPress = true;
            }
        }

        private void input_estados_KeyDown(object sender, KeyEventArgs e)
        {
            input_estados.ForeColor = Color.Black;
            if (e.KeyCode == Keys.Enter && input_estados.Text.Length > 0)
            {
                estados.lista_estados.Clear();
                List<string> new_alf = new List<string>(Regex.Replace(input_estados.Text, @"\s+", "").Select(x => x.ToString()).ToList());
                if (estados.validate_estado(new_alf))
                {
                    estados.lista_estados = new_alf;
                    txt_estados.Text = String.Join(" - ", estados.lista_estados);
                    txt_estados.ForeColor = Color.Black;
                    input_estados.ForeColor = Color.Green;
                    UpdateTabla();
                    UpdateComboBox();
                    txt_AFDT.Text = "AFDT = " + "({ " + string.Join(", ", alfabeto_entrada.lista_alf) + " }, { " + string.Join(", ", alfabeto_salida.lista_alf) + " }, { " + string.Join(", ", estados.lista_estados) + " }, '" + estados.inicial + "' , {" + string.Join(", ", estados.lista_estados_aceptacion) + "}, f, g)";

                }
                else
                {
                    txt_estados.ForeColor = Color.Red;
                    input_estados.ForeColor = Color.Red;
                    txt_estados.Text = "ERROR!";

                }

                e.SuppressKeyPress = true;
            }
        }

        private void input_aceptacion_KeyDown(object sender, KeyEventArgs e)
        {
            input_aceptacion.ForeColor = Color.Black;
            if (e.KeyCode == Keys.Enter && input_aceptacion.Text.Length > 0)
            {
                estados.lista_estados_aceptacion.Clear();
                List<string> new_alf = new List<string>(Regex.Replace(input_aceptacion.Text, @"\s+", "").Select(x => x.ToString()).ToList());
                if (estados.validate_estado_aceptacion(new_alf))
                {
                    estados.lista_estados_aceptacion = new_alf;
                    txt_aceptacion.Text = String.Join(", ", estados.lista_estados_aceptacion);
                    txt_aceptacion.ForeColor = Color.Black;
                    input_aceptacion.ForeColor = Color.Green;
                    UpdateTabla();
                    txt_AFDT.Text = "AFDT = " + "({ " + string.Join(", ", alfabeto_entrada.lista_alf) + " }, { " + string.Join(", ", alfabeto_salida.lista_alf) + " }, { " + string.Join(", ", estados.lista_estados) + " }, '" + estados.inicial + "' , {" + string.Join(", ", estados.lista_estados_aceptacion) + "}, f, g)";

                }
                else
                {
                    txt_aceptacion.ForeColor = Color.Red;
                    input_aceptacion.ForeColor = Color.Red;
                    txt_aceptacion.Text = "ERROR!";
                }

                e.SuppressKeyPress = true;
            }
        }

        //generar las funciones para la traduccion
        private void btn_generar_Click(object sender, EventArgs e)
        {
            funcion_transicion.GenerarFuncion(tableFunciones);
            funcion_salida.GenerarFuncion(table_funciones_salida);

            if (funcion_transicion.allDone && funcion_salida.allDone)
            { 
                btn_generar.ForeColor = Color.Green;
                input_entrada.Text = "";
            }
            else
                btn_generar.ForeColor = Color.Red;
        }

        #region minimizacion

        //minimizacion
        private void btn_crear_tabla_Click(object sender, EventArgs e)
        {
            lista_tablas.Add(new Tabla(tableMinimizacion,
                input_min_alf.Text != "" ? Int32.Parse(input_min_alf.Text) : 2,
                input_min_estados.Text != "" ? Int32.Parse(input_min_estados.Text) : 5));
        }

        

        private void btn_borrar_tablas_Click(object sender, EventArgs e)
        {
            foreach (Tabla tl in lista_tablas)
            {
                tableMinimizacion.Controls.Remove(tl.tabla);
            }
            lista_tablas.Clear();
            btn_minimizar.ForeColor = Color.Black;
        }

       

        private void btn_minimizar_Click(object sender, EventArgs e)
        {
            string fullListString = string.Empty;

            foreach (Tabla tabla in lista_tablas)
            {
                if (tabla.estado.inicial != "" )
                {
                    tabla.funcion.GenerarFuncion(tabla.tabla);
                    if (tabla.funcion.allDone)
                    {
                        tabla.funcion.GetConvexos(tabla.list_alf, tabla.estado);
                        btn_minimizar.ForeColor = Color.Green;
                        tabla.PintarEstadosValidos();
                        tabla.funcion.ComprobarEquivalencia(tabla.estado);
                        tabla.PintarEstadosValidos();
                    }
                    else
                        btn_minimizar.ForeColor = Color.Red;
                }
                else
                    btn_minimizar.ForeColor = Color.Red;
            }
        }
        

        private void input_min_estados_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Char.IsNumber(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }
        #endregion
    }

}
