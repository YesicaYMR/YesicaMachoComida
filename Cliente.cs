using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesicaMachoComida
{
  

    public class Cliente
    {
        String usuario;
        String contra;
        String nombre;
        int calle;
        String piso;
        String telefono;

        public Cliente(string usuario, string contra, string nombre,  int calle,string piso, string telefono)
        {
            this.usuario = usuario;
            this.contra = contra;
            this.nombre = nombre;
            this.calle = calle;
            this.piso = piso;            
            this.telefono = telefono;
        }
        public String getUsuario()
        {
            return usuario;
        }
        public String getContra()
        {
            return contra;
        }
        public string getNombre()
        {
            return nombre;
        }
        
        public int getCalle() {
            return calle;   
        }
        public String getTelefono()
        {
            return telefono;
        }
        public String getPiso()
        {
            return piso;
        }
      
        public void actualizar(String nombre,int calle,string telefono,string piso)
        {
            this.nombre = nombre; this.calle = calle;this.telefono=telefono;this.piso = piso;
        }

    }
}
