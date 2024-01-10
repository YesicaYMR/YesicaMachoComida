using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.Hosting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace YesicaMachoComida
{
    public partial class Form1 : Form

    {
        Cliente[] clientes;
        int numCliente = 0;
        String[] calles = {"Salsipuedes","Mortadelo y Filemón","La alegría de la huerta","Me falta un tornillo","Mengue","Desengaño", "Poca Sangre",
        "Tantarantana","Naranjito de Triana","Tia Tula","Cilla","Muelle","Rompelanzas","Amor Hermoso"};
        bool usuarioExiste;
        int idCliente;



        public Form1()
        {
            InitializeComponent();
            clientes = new Cliente[100];
            clientes[numCliente] = new Cliente("ana", "123", "Ana garcia", 2, "24 3dcha", "699999999");
            numCliente++;
            cargarCalles();
        }

        //Evento para las teclas pulsadas en el campo contraseña
        private void txtContra_KeyPress(object sender, KeyPressEventArgs e)
        {
            //cuando la tecla pulsada sea intro
            if (e.KeyChar == (char)Keys.Return)
            {
                //comprobar que se han introducido los datos en todos los campos
                if (comprobarCamposRegistro())
                {
                    //comprueba Usuario y contraseña
                    confirmar();
                }

            }//fin if de pulsar el intro
        }//fin metodo key press


        private void crearNuevoUsuario(String usuario, String contra)
        {
            String mensaje = "Usuario no existe, ¿Desea crearlo?";
            MessageBoxButtons botones = MessageBoxButtons.YesNo;
            var botonPulsado = MessageBox.Show(mensaje, "Información", botones);
            String nombreUsuario = usuario.Trim();
            String contrasena = contra;

            if (botonPulsado == DialogResult.Yes)
            {
                pnlDatos.Visible = true;
                desactivarCampos();
                activarCampos();
                btnGuardar.Enabled = true;
                usuarioExiste = false;
            }
        }

        //activar los campos de los datos personales
        private void activarCampos()
        {
            txtNombre.Enabled = true;
            cmbCalles.Enabled = true;
            txtTelefono.Enabled = true;
            txtPiso.Enabled = true;
        }

        //desactivar los campos de datos personales
        private void desactivarCamposPersonales()
        {
            txtNombre.Enabled = false;
            cmbCalles.Enabled = false;
            txtTelefono.Enabled = false;
            txtPiso.Enabled = false;
        }

        //desactiva los campos de usuario y contraseña
        private void desactivarCampos()
        {
            txtUsuario.Enabled = false;
            txtContra.Enabled = false;
        }

        //muestra los datos personales del cliente
        private void mostrarDatos(int i)
        {
            txtNombre.Text = clientes[i].getNombre();
            cmbCalles.SelectedIndex = clientes[i].getCalle();
            txtPiso.Text = clientes[i].getPiso();
            txtTelefono.Text = clientes[i].getTelefono();
        }

        //Comprueba que se ha escrito Usuario y Contraseña
        private Boolean comprobarCamposRegistro()
        {
            Boolean correcto = false;
            //Texto usuario vacio
            if (txtUsuario.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtUsuario, "Debe introducir un nombre");
                txtUsuario.Focus();
            }
            //texto contraseña vacio
            else if (txtContra.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtContra, "debe introducir una contraseña");
                errorProvider1.SetError(txtUsuario, "");
                txtContra.Focus();
            }
            //ambos campos contienen texto
            else
            {
                correcto = true;
                errorProvider1.SetError(txtUsuario, "");
                errorProvider1.SetError(txtContra, "");
            }
            return correcto;

        }

        //guardar los datos personales
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //comprobación de que los datos son válidos
            while (comprobarDatosPersonales())
            {
                borrarErrores();
                //si existe el usuario se modifica el existente
                if (usuarioExiste)
                {
                    clientes[idCliente].actualizar(txtNombre.Text, cmbCalles.SelectedIndex, txtTelefono.Text, txtPiso.Text);
                    MessageBoxButtons botones = MessageBoxButtons.OK;
                    MessageBox.Show("Datos actualizados", "Información", botones);
                    desactivarCamposPersonales();
                }
                //si el usuario no existe se crea nuevo
                else
                {
                    clientes[numCliente] = new Cliente(txtUsuario.Text, txtContra.Text, txtNombre.Text, (int)cmbCalles.SelectedIndex, txtPiso.Text, txtTelefono.Text);
                    //suma uno al numero de crear cliente para el array
                    numCliente++;
                    MessageBoxButtons botones = MessageBoxButtons.OK;
                    MessageBox.Show("Usuario Registrado", "Información", botones);
                    desactivarCamposPersonales();
                }
                btnPedido.Enabled = true;
                btnGuardar.Enabled = false;
                break;
            }
        }

        //comprobar los datos personales
        private bool comprobarDatosPersonales()
        {
            bool correcto = false;
            String nombreSinEspacios = txtNombre.Text.Trim().Replace(" ", String.Empty);
            //comprobacion del nombre
            //se ha ingresado texto
            if (txtNombre.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtNombre, "Debe introducir un nombre");
                txtNombre.Focus();
            }
            //El texto no contiene números
            else if (!nombreSinEspacios.All(char.IsLetter))
            {
                errorProvider1.SetError(txtNombre, "El nombre no puede contener números");
                txtNombre.Focus();
            }
            //comprobacion que se ha seleccionado una calle
            else if ((cmbCalles.SelectedIndex <= -1))
            {
                errorProvider1.SetError(cmbCalles, "Debe seleccionar una calle");
                errorProvider1.SetError(txtNombre, "");
                cmbCalles.Focus();
            }
            //comprobacion teléfono
            //se ha ingresado el número
            else if (txtTelefono.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtTelefono, "Debe introducir un número de teléfono");
                errorProvider1.SetError(txtNombre, "");
                errorProvider1.SetError(cmbCalles, "");
                txtTelefono.Focus();
            }
            //el número no tiene más de 9 digitos
            else if (txtTelefono.Text.Trim().Length != 9)
            {
                errorProvider1.SetError(txtTelefono, "El teléfono debe tener 9 cifras");
                errorProvider1.SetError(txtNombre, "");
                errorProvider1.SetError(cmbCalles, "");
                txtTelefono.Focus();
            }

            //empieza por 6 o 7
            else if (!txtTelefono.Text.StartsWith("6") && !txtTelefono.Text.StartsWith("7"))
            {
                errorProvider1.SetError(txtTelefono, "El número de teléfono debe comenzar por 6 o 7");
                errorProvider1.SetError(txtNombre, "");
                errorProvider1.SetError(cmbCalles, "");
                txtTelefono.Focus();
            }

            //todo son números
            else if (!txtTelefono.Text.All(char.IsDigit))
            {
                errorProvider1.SetError(txtTelefono, "El número de teléfono no puede contener letras");
                errorProvider1.SetError(txtNombre, "");
                errorProvider1.SetError(cmbCalles, "");
                txtTelefono.Focus();
            }
            //comprobacion piso
            //se ha introducido texto
            else if (txtPiso.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtPiso, "Debe introducir número de portal y piso");
                errorProvider1.SetError(txtNombre, "");
                errorProvider1.SetError(cmbCalles, "");
                errorProvider1.SetError(txtTelefono, "");
                txtPiso.Focus();
            }
            //todo está correcto
            else
            {
                correcto = true;
            }
            return correcto;
        }


        //mensaje emergente para confirmar los datos
        private void preguntarDatosCorrectos()
        {
            btnConfirmar.Enabled = false;
            String mensaje = "¿Son correctos estos datos?";
            MessageBoxButtons botones = MessageBoxButtons.YesNo;
            var botonPulsado = MessageBox.Show(mensaje, "Información", botones);
            //si no son correcto activa los campos para poder modificarlos y guardarlos
            if (botonPulsado == DialogResult.No)
            {
                activarCampos();
                btnGuardar.Enabled = true;

            }
            //si los datos son correctos activa el boton de pedido
            else
            {
                btnPedido.Enabled = true;

            }
        }

        //boton de realizar pedido abre ventana de menu y limpia esta
        private void btnPedido_Click(object sender, EventArgs e)
        {
            Menus ventanaMenus = new Menus(txtNombre.Text);
            ventanaMenus.ShowDialog();
            limpiar();
        }

        //carga el combo de las calles
        private void cargarCalles()
        {
            for (int i = 0; i < calles.Length; i++)
            {
                cmbCalles.Items.Add(calles[i]);
            }
        }

        //pulsa el boton de confirmar en usuario y contraseña
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (comprobarCamposRegistro())
            {
                confirmar();
            }

        }

        //Comprueba Usuario y contraseña
        private void confirmar()
        {
            //comprobar si existen clientes en el array
            if (clientes == null)
            {
                crearNuevoUsuario(txtUsuario.Text, txtContra.Text);
            }
            //si hay elementos en el array
            else
            {
                // Busca usuario y contraseña
                int i = 0;
                bool encontrado = false;
                while (!encontrado && i < clientes.Length && clientes[i] != null)
                {
                    //si usuario encuentra al usuario
                    if (clientes[i].getUsuario() == txtUsuario.Text.ToLower())
                    {
                        encontrado = true;

                        //si la contraseña coincide
                        if (clientes[i].getContra() == txtContra.Text)
                        {
                            pnlDatos.Visible = true;
                            desactivarCampos();
                            mostrarDatos(i);
                            preguntarDatosCorrectos();
                            usuarioExiste = true;
                            idCliente = i;
                            break;
                        }
                        // la contraseña es incorrecta
                        else
                        {
                            errorProvider1.SetError(txtContra, "La contraseña es incorrecta");
                            txtContra.Text = "";
                            txtContra.Focus();
                            break;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }//fin while buscar usuarios
                 //si no se ha encontrado el usuario en el array
                if (!encontrado)
                {
                    crearNuevoUsuario(txtUsuario.Text, txtContra.Text);
                }
            }//fin else
        }//fin while comprobar datos registro

        //Limpia y reestablece la ventana
        private void limpiar()
        {
            txtUsuario.Text = "";
            txtContra.Text = "";
            txtNombre.Text = "";
            txtPiso.Text = "";
            txtTelefono.Text = "";
            cmbCalles.SelectedIndex = 0;
            pnlDatos.Visible = false;
            txtUsuario.Enabled = true;
            txtContra.Enabled = true;
            btnGuardar.Enabled = false;
            btnPedido.Enabled = false;
            btnConfirmar.Enabled = true;
            borrarErrores();
        }

        //pulsar el boton de desconectar
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            String mensaje = "¿Seguro que deseas salir?";
            MessageBoxButtons botones = MessageBoxButtons.YesNo;
            var botonPulsado = MessageBox.Show(mensaje, "Información", botones);


            if (botonPulsado == DialogResult.Yes)
            {
                limpiar();
            }

        }

        //borra los posibles errores que hayan saltado
        private void borrarErrores()
        {
            errorProvider1.SetError(txtTelefono, "");
            errorProvider1.SetError(txtPiso, "");
            errorProvider1.SetError(txtNombre, "");
            errorProvider1.SetError(cmbCalles, "");
        }

        
    }
}

