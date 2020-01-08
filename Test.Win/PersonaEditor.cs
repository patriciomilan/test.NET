using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.Entidad;

namespace Test.Win
{
    public partial class PersonaEditor : Form
    {
        private Persona _persona;
        private List<Razon> _razones;
        private bool esNuevo;
        public PersonaEditor(Persona persona)
        {
            InitializeComponent();
            _persona = persona;
            ObtieneRazones();
            EscribePersona();
            statusStrip1.Items[0].Text = $"Editando: {_persona.Nombre} {_persona.Apellido}";
            esNuevo = false;
            SuscribeEventos();
            ActivaGuardado();
        }

        public PersonaEditor()
        {
            InitializeComponent();
            _persona = new Persona();
            ObtieneRazones();
            EscribePersona();
            statusStrip1.Items[0].Text = "Agregando nueva persona";
            esNuevo = true;
            SuscribeEventos();
            ActivaGuardado();
        }
        private void SuscribeEventos()
        {
            nombreTextbox.TextChanged += Textbox_TextChanged;
            apellidoTextbox.TextChanged += Textbox_TextChanged;
        }

        private void Textbox_TextChanged(object sender, EventArgs e)
        {
            ActivaGuardado();
        }

        private void ActivaGuardado()
        {
            toolStrip1.Items[0].Enabled = Valido();
        }

        private void ObtieneRazones()
        {
            var repositorioRazon = new Repositorio<Razon>();
            _razones = repositorioRazon.Seleccionar().Data;
        }

        private bool Valido() 
        {
            bool validado = true;
            if (string.IsNullOrEmpty(nombreTextbox.Text))
                validado = false;

            if (string.IsNullOrEmpty(apellidoTextbox.Text))
                validado = false;

            if (ObtenerRazonSeleccionada() == 0)
                validado = false;

            return validado;
        }

        private void EscribePersona()
        {

            this.nombreTextbox.Text = _persona.Nombre;
            this.apellidoTextbox.Text = _persona.Apellido;
            RadioButton radio;
            int x = 0;
            int y = 0;
            x = motivoGroupBox.Location.X - 140;
            y = motivoGroupBox.Location.Y - 80;
            foreach (var razon in _razones)
            {
                radio = new RadioButton();
                radio.Text = razon.Descripcion;
                radio.Checked = (_persona.RazonRegistroId == razon.Id);
                radio.Location = new Point(x, y);
                y = y + 20;
                radio.Width = 200;
                radio.Height = 20;
                radio.Tag = razon.Id;
                radio.CheckedChanged += Radio_CheckedChanged;
                motivoGroupBox.Controls.Add(radio);
            }
            motivoGroupBox.Height = (_razones.Count * 20) + 40;
        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            ActivaGuardado();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Actualiza()
        {
            _persona.Apellido = apellidoTextbox.Text;
            _persona.Nombre = nombreTextbox.Text;
            _persona.RazonRegistroId = ObtenerRazonSeleccionada();
        }

        private int ObtenerRazonSeleccionada() 
        {
            RadioButton radio;
            foreach (var control in motivoGroupBox.Controls)
            {
                radio = control as RadioButton;
                if (radio.Checked)
                    return Convert.ToInt32(radio.Tag);
            } 
            return 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var repositorio = new Repositorio<Persona>();
            Actualiza();
            if (esNuevo)
                repositorio.Insertar(_persona);
            else
                repositorio.Actualizar(_persona);

            Personas.ObtenerFormulario().ActualizaGrilla();
            this.Close();
        }


    }

    

}
