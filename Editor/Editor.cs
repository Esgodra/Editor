/*
 * Creado por: Daniel Rolando Perez Donis (Esgodra)
 * Carnet: 1290-18-17176
 * 
 * 
 * 
 * (c) Todos los derechos reservados
 */






using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace Editor
{

    public partial class Editor : Form
    {
        #region Variables_Globales
        private int Lineas = 0;
        private bool abrir = false;
        private string Path = "";
        #endregion

        #region Enumeradores
        public enum ScrollBarType : uint
        {
            SbHorz = 0,
            SbVert = 1,
            SbCtl = 2,
            SbBoth = 3
        }

        public enum Message : uint
        {
            WM_VSCROLL = 0x0115
        }

        public enum ScrollBarCommands : uint
        {
            SB_THUMBPOSITION = 4
        } 
        #endregion

        public Editor()
        {
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new TestColorTable());
            statusBar();
        }

        #region Imports
        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Eventos
        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            SincronizaScroll();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            statusBar();
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            statusBar();
        }

        private void ajusteDeLineaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ajusteDeLineaToolStripMenuItem.Checked)
            {
                richTextBox1.WordWrap = false;
                ajusteDeLineaToolStripMenuItem.Checked = false;
            }
            else
            {
                richTextBox1.WordWrap = true;
                ajusteDeLineaToolStripMenuItem.Checked = true;
            }
        }

        private void fuenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.Font = fontDialog1.Font;
        }

        private void zoomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LineNumberTextBox.ZoomFactor = richTextBox1.ZoomFactor = (float)(richTextBox1.ZoomFactor + 0.1);
        }

        private void zoomToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            LineNumberTextBox.ZoomFactor = richTextBox1.ZoomFactor = (float)(richTextBox1.ZoomFactor - 0.1);
        }

        private void barraDeEstadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (barraDeEstadoToolStripMenuItem.Checked)
            {
                statusStrip1.Hide();
                barraDeEstadoToolStripMenuItem.Checked = false;
            }
            else
            {
                statusStrip1.Show();
                barraDeEstadoToolStripMenuItem.Checked = true;
            }
        }

        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText.Remove(richTextBox1.SelectedText.IndexOf(richTextBox1.SelectedText), richTextBox1.SelectedText.LastIndexOf(richTextBox1.SelectedText));
        }

        private void buscarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Find(Interaction.InputBox("Ingrese un texto", "Buscar..."));
            }
            catch
            {
                MessageBox.Show("No se encuentra el texto especificado");
            }

        }

        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void seleccionarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void irAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string numeroLinea = Interaction.InputBox("Ingrese una linea", "Buscar...", "1");

            try
            {
                if (numeroLinea != "")
                {
                    int linea = 1;
                    linea = Convert.ToInt32(numeroLinea);
                    richTextBox1.Select(richTextBox1.GetFirstCharIndexFromLine(linea - 1), 0);
                }
            }
            catch
            {
                MessageBox.Show("No existe el numero de linea");
            }
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Confirmar_Cambios()) { Nuevo(); }


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            NumeroLinea();
            if (abrir)
            {
                this.Text += '*';
                abrir = false;
            }
            SincronizaScroll();
        }

        private void numeroDeLineaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (numeroDeLineaToolStripMenuItem.Checked)
            {
                LineNumberTextBox.Hide();
                zoomToolStripMenuItem.Enabled = true;
                numeroDeLineaToolStripMenuItem.Checked = false;
            }
            else
            {
                LineNumberTextBox.Show();
                zoomToolStripMenuItem.Enabled = false;
                numeroDeLineaToolStripMenuItem.Checked = true;
            }
        }

        private void asignacionDeVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(1);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!Confirmar_Cambios()) { return; };
            Nuevo();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string valor = listBox1.SelectedValue.ToString();
            richTextBox1.Find(valor);
            richTextBox1.Focus();
            richTextBox1.SelectionColor = Color.Red;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Confirmar_Cambios()) { this.Close(); }

        }

        private void declaracionVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(1);
        }

        private void toolStripButton4_ButtonClick(object sender, EventArgs e)
        {
            toolStripButton4.ShowDropDown();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (Path == "")
            {
                GuardarComo();
            }
            else
            {
                Guardar();
            }
            abrir = true;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            GuardarComo();
            abrir = true;
        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Confirmar_Cambios()) { e.Cancel = true; }
        }

        private void numerosRealesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(2);
        }

        private void numerosRealesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(2);
        }
        private void nombresDeVariableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(3);
        }        
        private void nombresDeVariableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(3);
        }
        private void numerosRacionalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(4);
        }
        private void numerosRacionalesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(4);
        }
        private void lenguaje0sY1sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(5);
        }
        private void lenguaje0sY1sToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LlamadaAutomatas(5);
        }
        #endregion        

        #region Metodos
        //Metodo para crear un archivo nuevo
        private void Nuevo()
        {
            //Se resetea el ritchtextbo principal
            richTextBox1.ResetText();
            //Se modifica el titulo para mostrar que es un nuevo archivo
            this.Text = "Untitled.txt - Editor";
            //Se cambia abrir a true para mostrar * si hay cambios y se limpia la variable path
            abrir = true;
            Path = "";
        }

        //Metodo para guardar
        public void Guardar()
        {
            richTextBox1.SaveFile(Path, RichTextBoxStreamType.PlainText);
            this.Text = this.Text.Replace("*", "");
        }

        //Metodo para verificar si el usuario quiere guardar los cambios que ha realizado en un documento retorna un valor booleano para ser manejada por el invocador
        public Boolean Confirmar_Cambios()
        {
            if (richTextBox1.Text != "" && !abrir)
            {
                DialogResult option = MessageBox.Show("Desea guardar los cambios realizados en el archivo actual?", "Editor", MessageBoxButtons.YesNoCancel);
                if (option == DialogResult.Yes && Path == "")
                {
                    GuardarComo();
                    return true;
                }
                else if (option == DialogResult.Yes && Path != "")
                {
                    Guardar();
                    return true;
                }
                else if (option == DialogResult.No)
                {
                    abrir = true;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            return true;
        }

        //Metodo para enumerar las lineas del richtextbox principal
        public void NumeroLinea()
        {
            //Si el numero de lineas del LineNumber es mayor al numro de lineas del Richtextbox principal, se ejecuta un ciclo que sustituye el contenido del linenumber
            //Generando los numeros de lineas nuevas
            if (Lineas > richTextBox1.Lines.Length || abrir)
            {
                LineNumberTextBox.Text = "";
                Lineas = richTextBox1.Lines.Length;
                for (int i = 1; i <= Lineas; i++)
                {
                    LineNumberTextBox.Text += i + "\n";
                }
            }
            //Se agrega el numero de linea que se acaba de escribir en el richtexbox si el numero de lineas de LineNumber es menor que el del richtextbox primario
            else if (Lineas < richTextBox1.Lines.Length)
            {
                if (richTextBox1.Lines.Length == 1) { LineNumberTextBox.Text = ""; }
                Lineas = richTextBox1.Lines.Length;
                LineNumberTextBox.Text += Lineas + "\n";
            }
            else if (richTextBox1.Lines.Length == 0 || richTextBox1.Text == "") { LineNumberTextBox.Text = "1"; }
        }

        //Metodo que determina el automata que se ejecuta y realiza la llamada
        private void LlamadaAutomatas(int opcion)
        {
            //Variables que se utilizaran en el metodo
            bool correcta = false;
            string Textolinea = "";
            ArrayList Errores = new ArrayList();
            //Limpiamos el listextBox para que muestre los nuevos errores detectados
            listBox1.ResetText();
            //Iniciamos un for que se ejecuta en base a la cantidad de lineas que tiene el richtextbox principal
            for (int i = 1; i <= richTextBox1.Lines.Length; i++)
            {
                //Asignamos el contenido de la linea a  y lo enviamos a la clase externa automata para ser analizada dependiendo de la opcion seleccionada a menos
                //que el textbox este vacio
                Textolinea = richTextBox1.Lines.GetValue(i - 1).ToString();
                if (Textolinea != "")
                {
                    if (opcion == 1)
                    {
                        correcta = Automatas.Asignacion(Textolinea);
                    }
                    else if (opcion == 2)
                    {
                        correcta = Automatas.Numeros(Textolinea);
                    }
                    else if (opcion == 3) {
                        correcta = Automatas.NombreVariable(Textolinea);
                    }
                    else if (opcion == 4)
                    {
                        correcta = Automatas.Racional(Textolinea);
                    }
                    else if (opcion == 5)
                    {
                        correcta = Automatas.Binario(Textolinea);
                    }

                    //Si la linea es incorrecta se agrega al array que se convertira en dataSet para el listbox
                    if (!correcta)
                    {
                        Errores.Add(new Error("Error en linea " + i + ", en texto " + Textolinea, Textolinea));
                    }
                }

            }
            //Si textoLinea no esta vacio se asigna el dataSet para el listBox

            listBox1.DataSource = Errores;
            listBox1.DisplayMember = "InformacionDeLinea";
            listBox1.ValueMember = "TextoConError";
            richTextBox1.SelectAll();
            richTextBox1.SelectionColor = Color.White;
            richTextBox1.Text = richTextBox1.Text;
            if (listBox1.Items.Count > 0)
            {
                MessageBox.Show("Se han detectado errores");
            }
        }

        //Clase que cancela que se utiliza como dataset para el listtextbox
        public class Error
        {
            private string Int_TextoConError;
            private string Int_InformacionDeLinea;

            public Error(string InformacionDeLinea, string TextoConError)
            {

                this.Int_TextoConError = TextoConError;
                this.Int_InformacionDeLinea = InformacionDeLinea;
            }

            public string TextoConError
            {
                get
                {
                    return Int_TextoConError;
                }
            }

            public string InformacionDeLinea
            {

                get
                {
                    return Int_InformacionDeLinea;
                }
            }
        }

        //Metodo para abrir archivos
        public void Abrir()
        {
            if (!Confirmar_Cambios()) { return; }
            // Se crea un nuevo openFile
            OpenFileDialog openFile1 = new OpenFileDialog();

            // Se inicializa el openfile buscando archivos .txt
            openFile1.DefaultExt = "*.txt";
            openFile1.Filter = "Archivos de texto|*.txt";


            // Determinar si el usuario ya selecciono un archivo
            if (openFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               openFile1.FileName.Length > 0)
            {
                // Cargar texto
                richTextBox1.LoadFile(openFile1.FileName, System.Windows.Forms.RichTextBoxStreamType.PlainText);
                Path = openFile1.FileName;
                this.Text = openFile1.SafeFileName + " - Editor";
                //Bandera para identificar si el archivo se acaba de abrir
                abrir = true;
                NumeroLinea();
            }

        }

        //Metodo para guardar como y guardar archivos nuevos
        public void GuardarComo()
        {
            SaveFileDialog save = new SaveFileDialog();

            save.DefaultExt = "*.txt";
            save.Filter = "Archivos de texto | *.txt";

            if (save.ShowDialog() == DialogResult.OK && save.FileName.Length > 0)
            {
                richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                this.Text = save.FileName + "- Editor";
                Path = save.FileName;
            }
        }

        //Metodo que sincroniza los scroll del ritchtextbox principal y el LineNumber
        public void SincronizaScroll()
        {
            int nPos = GetScrollPos(richTextBox1.Handle, (int)ScrollBarType.SbVert);
            nPos <<= 16;
            uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
            SendMessage(LineNumberTextBox.Handle, (int)Message.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));
        }

        //Metodo que actualiza la barra de estado con la posicion actual del puntero
        public void statusBar()
        {
            // Obtiene la linea
            int index = richTextBox1.SelectionStart;
            int linea = richTextBox1.GetLineFromCharIndex(index);

            // Obtiene la columna
            int firstChar = richTextBox1.GetFirstCharIndexFromLine(linea);
            int column = index - firstChar;

            toolStripStatusLabel3.Text = (richTextBox1.ZoomFactor * 100).ToString() + "%";
            toolStripStatusLabel4.Text = "Col. " + (column + 1) + " Line " + (linea + 1);

        }






        #endregion


    }

    public class TestColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(31, 64, 64); }
        }

        public override Color MenuBorder  
        {
            get { return Color.FromArgb(37,38,36) ; }
        }

    }
}