using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YesicaMachoComida
{
    public partial class Menus : Form
    {
        String nombre_cliente;
        String[] primPlatosGral = { "","Pasta Bolognesa", "Patatas Riojana", "Paella" };
        String[] segPlatosGral = {"", "Solomillo con salsa de pimienta", "Rodaballo al horno", "Pollo asado" };
        String[] postreGral = { "","Flan", "Tarta de queso", "Profiteroles" };
        String[] primPlatosVeg = { "","Quiche de verduras con leche de soja", "Ensalada con frutas de temporada", "Hummus de remolacha" };
        String[] segPlatosVeg = { "","Berenjenas rellenas", "Guiso de garbanzos y pimientos", "Rattatuille" };
        String[] postresVeg = { "","Pudding de chocolate vegano", "Frutas asadas", "Tarta de limón" };
        String[] bebidas = {"", "Agua", "Vino", "Cerveza", "Refresco", "Café" };
        double[] precios = {0, 1.5, 4, 2.5, 1.8, 1.2 };
        Dictionary<String, double> bebidas_precios;
        double precioPrimero=4.4;
        double precioSegundo=5.5;
        double precioPostre=3;
        double suplementoVegano = 1.2;
        double precioPan = 0.8;
        double precioTotal=0;

        public Menus(String nombre)
        {
            InitializeComponent();
            nombre_cliente = nombre;
            bebidas_precios = new Dictionary<String, double>();
            //creación diccionario
            for (int i = 0; i < bebidas.Length; i++)
            {
                bebidas_precios[bebidas[i]] = precios[i];
            }


        }

        private void Menus_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text += (string)nombre_cliente;
            cargarCombos();
        }

        //Carga la información de todos los combos
        private void cargarCombos()
        {
            for (int i = 0; i < primPlatosGral.Length; i++)
            {
                cmbPrimGral.Items.Add(primPlatosGral[i]);
            }

            for (int i = 0; i < segPlatosGral.Length; i++)
            {
                cmbSegGral.Items.Add(segPlatosGral[i]);
            }

            for (int i = 0; i < postreGral.Length; i++)
            {
                cmbPostresGral.Items.Add(postreGral[i]);
            }

            for (int i = 0; i < primPlatosVeg.Length; i++)
            {
                cmbPrimVeg.Items.Add(primPlatosVeg[i]);
            }

            for (int i = 0; i < segPlatosVeg.Length; i++)
            {
                cmbSegVeg.Items.Add(segPlatosVeg[i]);
            }

            for (int i = 0; i < postresVeg.Length; i++)
            {
                cmbPostresVeg.Items.Add(postresVeg[i]);
            }
            for (int i = 0; i < bebidas.Length; i++)
            {
                cmbBebidas.Items.Add(bebidas[i]);
            }
        }




        //RadioButton del menú vegano
        private void radVegano_CheckedChanged(object sender, EventArgs e)
        {
            limpiar();
            pnlVegano.Enabled = true;
            pnlGral.Enabled = false;
        }

        //RadioButton del menú general
        private void radGral_CheckedChanged(object sender, EventArgs e)
        {
            limpiar();
            pnlVegano.Enabled = false;
            pnlGral.Enabled = true;
        }

        //checkBox de las bebidas. muestra el combo bebidas o lo oculta
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chckBebida.Checked)
            {
                cmbBebidas.Visible = true;
            }
            else
            {
                cmbBebidas.Visible = false;
            }
        }

        //Botón de salir
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            String mensaje = "¿Seguro que deseas salir?";
            MessageBoxButtons botones = MessageBoxButtons.YesNo;
            var botonPulsado = MessageBox.Show(mensaje, "Información", botones);


            if (botonPulsado == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            //comprobar que se ha seleccionado segundo plato
            if (cmbSegGral.SelectedIndex <= 0 && cmbSegVeg.SelectedIndex <= 0)
            {
                //del menu general
                if (pnlGral.Enabled)
                {
                    errorProvider1.SetError(cmbSegGral, "Debe seleccionar un segundo");
                    errorProvider1.SetError(cmbSegVeg, "");
                }
               //del menu vegano
                else
                {
                    errorProvider1.SetError(cmbSegVeg, "Debe seleccionar un segundo");
                    errorProvider1.SetError(cmbSegGral, "");
                }
               
            }
            //si bebida está marcado pero no se ha pedido ninguna
            else if (chckBebida.Checked && cmbBebidas.SelectedIndex <= 0)
            {
                errorProvider1.SetError(cmbBebidas, "Debe seleccionar una bebida");
                errorProvider1.SetError(cmbSegGral, "");
                errorProvider1.SetError(cmbSegVeg, "");
            }
            //si todo está correcto
            else
            {
                panel1.Visible = true;
                generarTicket();
            }

        }

        //pulsar boton cancelar del Ticket
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            precioTotal = 0;
        }

        private void generarTicket()
        {
            //menu general activo
            if (pnlGral.Enabled) { 
                //primer plato
                if (cmbPrimGral.SelectedIndex <= 0)
                {
                lblPrimero.Text = "";
                lblPrecioPrim.Text = "";
                }
                else 
                { 
                    lblPrimero.Text = primPlatosGral[cmbPrimGral.SelectedIndex];
                    lblPrecioPrim.Text = precioPrimero+" €";
                    precioTotal += precioPrimero;
                }
                //Segundo plato
                    lblSegundo.Text = segPlatosGral[cmbSegGral.SelectedIndex];
                    lblPrecioSeg.Text = precioSegundo+" €";
                precioTotal += precioSegundo;
                //postre
                if (cmbPostresGral.SelectedIndex <= 0)
                {
                    lblPostre.Text = "";
                    lblPrecioPostre.Text = "";
                }
                else
                {
                    lblPostre.Text = postreGral[cmbPostresGral.SelectedIndex];
                    lblPrecioPostre.Text = precioPostre+" €";
                    precioTotal += precioPostre;
                }
            }//fin ticket menu general
            //menu vegano activo
            else
            {
                //primer plato
                if (cmbPrimVeg.SelectedIndex <= 0)
                {
                    lblPrimero.Text = "";
                    lblPrecioPrim.Text = "";
                }
                else
                {
                    lblPrimero.Text = primPlatosVeg[cmbPrimVeg.SelectedIndex];
                    lblPrecioPrim.Text = precioPrimero * suplementoVegano + " €";
                    precioTotal += precioPrimero*suplementoVegano;
                }
                //Segundo plato
                lblSegundo.Text = segPlatosVeg[cmbSegVeg.SelectedIndex];
                lblPrecioSeg.Text = precioSegundo * suplementoVegano + " €";
                precioTotal += precioSegundo*suplementoVegano;
                //postre
                if (cmbPostresGral.SelectedIndex <= 0)
                {
                    lblPostre.Text = "";
                    lblPrecioPostre.Text = "";
                }
                else
                {
                    lblPostre.Text = postreGral[cmbPostresGral.SelectedIndex];
                    lblPrecioPostre.Text = precioPostre+" €";
                    precioTotal += precioPostre;
                }
            }//fin ticket menu vegano
            //Si el pan está marcado
            if(chckPan.Checked)
            {
                lblPan.Text = "Pan";
                lblPrecioPan.Text = precioPan + " ";
                precioTotal += precioPan;
                }
            else
            {
                lblPan.Text = "";
                lblPrecioPan.Text = "";
            }
            //si la bebida está marcada
            if (chckBebida.Checked) {
                //recoge el texto del combo
                lblBebida.Text = cmbBebidas.Text;
                //del diccionario coge el precio del texto de bebida
                lblPrecioBebida.Text = bebidas_precios[cmbBebidas.Text] +" €";
                precioTotal += precios[cmbBebidas.SelectedIndex];

            }
            else
            {
                lblBebida.Text = "";
                lblPrecioBebida.Text = "";
            }

            //Precio total del pedido
            lblTotal.Text = precioTotal.ToString();
        }

        //boton de confirmar el pedido en el ticket
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            String mensaje = "Tu pedido se está procesando. En breve estará en tu domicilio.¿Quieres realizar otro pedido?";
            MessageBoxButtons botones = MessageBoxButtons.YesNo;
            var botonPulsado = MessageBox.Show(mensaje, "Información", botones);
            if(botonPulsado == DialogResult.No)
            {
                this.Close();
            }
            else
            {
                limpiar();
                panel1.Visible = false;
                precioTotal = 0;
                chckBebida.Checked = false;
                chckPan.Checked = false;
            }
        }


        //Limpia la pantalla y reestablece los combos y los checkBox
        private void limpiar()
        {
            cmbBebidas.SelectedIndex = 0;
            cmbPrimGral.SelectedIndex = 0;
            cmbPrimVeg.SelectedIndex = 0;
            cmbSegGral.SelectedIndex = 0;
            cmbSegVeg.SelectedIndex = 0;
            cmbPostresGral.SelectedIndex = 0;
            cmbPostresVeg.SelectedIndex = 0;
            errorProvider1.SetError(cmbSegVeg, "");
            errorProvider1.SetError(cmbSegGral, "");
            errorProvider1.SetError(cmbBebidas, "");
        }
    }
}
