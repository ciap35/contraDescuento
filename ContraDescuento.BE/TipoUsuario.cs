using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BE
{
    public class TipoUsuario
    {
        private int codTipoUsuario = 0;
        private string descripcion = string.Empty;
        private BE.Grupo grupo = null;
        //private List<BE.Grupo> grupoList = null;
        private EnumTipoUsuario enumTipoUsuario;
        private bool usuario = false;
        private bool comercio = false;
        private bool otros = false;

        public int CodTipoUsuario { get { return codTipoUsuario; } set { codTipoUsuario = value; } }
        public string Descripcion { get { return descripcion; } set { descripcion = value.ToUpper(); } }
        
        public BE.Grupo Grupo { get { return grupo; } set { grupo = value; } }
        //public List<BE.Grupo> GrupoList { get { return grupoList; } set { grupoList = value; } }
        
        public EnumTipoUsuario EnumTipoUsuario { get { return enumTipoUsuario;} set { enumTipoUsuario = value; } }

        public bool Usuario { get { return usuario; } set { usuario = value; } }
        public bool Comercio { get { return comercio; } set { comercio = value; } }
        public bool Otros { get { return otros; } set { otros = value; } }


        public TipoUsuario() { /*usuario = true;*/ }

        public TipoUsuario(bool usuario,bool comercio, bool otros)
        {
            if (usuario)
                EnumTipoUsuario = EnumTipoUsuario.Usuario;
            if (comercio)
                EnumTipoUsuario = EnumTipoUsuario.Comercio;
            if (otros)
                EnumTipoUsuario = EnumTipoUsuario.Otros;
        }

        public TipoUsuario(EnumTipoUsuario tipoUsr)
        {
            if (tipoUsr == EnumTipoUsuario.Usuario)
                usuario = true;
            if (tipoUsr == EnumTipoUsuario.Comercio)
                comercio = true;
            if (tipoUsr == EnumTipoUsuario.Otros)
                otros = true;
        }

        public override string ToString()
        {
            return CodTipoUsuario+ " " + Descripcion+ " "+ Convert.ToString(Grupo);
        }
    }

    public enum EnumTipoUsuario
    {
        Usuario = 1,
        Comercio = 2,
        Otros = 3,
    }
}
