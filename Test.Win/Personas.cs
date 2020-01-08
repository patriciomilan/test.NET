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
    public partial class Personas : Form
    {
        public Personas()
        {
            InitializeComponent();
            ActualizaGrilla();
            personasGridView.DoubleClick += PersonasGridView_DoubleClick;
        }

        private void PersonasGridView_DoubleClick(object sender, EventArgs e)
        {
            personas = this;
            Persona persona = personasGridView.SelectedRows[0].DataBoundItem as Persona;
            PersonaEditor personaEditor = new PersonaEditor(persona);
            personaEditor.ShowDialog();
        }

        public void ActualizaGrilla()
        {
            var repositorio = new Repositorio<PersonaView>();
            personasGridView.DataSource = repositorio.Seleccionar().Data;
        }

        private void agregarBoton_Click(object sender, EventArgs e)
        {
            personas = this;
            PersonaEditor personaEditor = new PersonaEditor();
            personaEditor.ShowDialog();
        }

        private static Personas personas;
        public static Personas ObtenerFormulario() 
        {
            return personas;
        }

    }
}
