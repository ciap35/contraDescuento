using System;

namespace ContraDescuento.BE
{
    public class Usuario : IAuditable
    {
        #region Propiedades Privadas
        private int codUsuario = 0;
        private int cantIntentos = 0;
        private bool habilitado = false;
        private bool bloqueado = false;
        private string nombre = string.Empty;
        private string apellido = string.Empty;
        private DateTime fechaDeNacimiento = DateTime.MinValue;
        private string email = string.Empty;
        private string password = string.Empty;
        private char? sexo;
        private DateTime fechaCreacion = DateTime.MinValue;
        private DateTime fechaBaja = DateTime.MinValue;
        private DateTime fechaModificacion = DateTime.MinValue;
        private int dvh = 0;
        private TipoUsuario tipoUsuario = new TipoUsuario(EnumTipoUsuario.Usuario);
        private Comercio comercio = null;
        private PreguntaDeSeguridad preguntaDeSeguridad = new PreguntaDeSeguridad();

        private Idioma idioma = null;
        private GrupoBase grupo = new Grupo();
        private Telefono telefono = new Telefono();
        private Domicilio domicilio = null;
        #endregion

        #region Propiedades Públicas
        public int CodUsuario { get { return codUsuario; } set { codUsuario = value; } }
        public int CantIntentos { get { return cantIntentos; } set { cantIntentos = value; } }
        public bool Habilitado { get { return habilitado; } set { habilitado = value; } }
        public bool Bloqueado { get { return bloqueado; } set { bloqueado = value; } }
        public string Nombre { get { return nombre; } set { nombre = value; } }
        public string Apellido { get { return apellido; } set { apellido = value; } }
        public DateTime FechaDeNacimiento { get { return fechaDeNacimiento; } set { fechaDeNacimiento = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value; } }
        public char? Sexo { get { return sexo.Value; } set { sexo = value; } }
        public DateTime FechaCreacion { get { return fechaCreacion; } set { fechaCreacion = value; } }
        public DateTime FechaBaja { get { return fechaBaja; } set { fechaBaja = value; } }
        public DateTime? FechaModificacion { get { return fechaModificacion; } set { fechaModificacion = value.Value; } }
        public int DVH { get { return dvh; } set { dvh = value; } }
        public TipoUsuario TipoUsuario { get { return tipoUsuario; } set { tipoUsuario = value; } }
        public Comercio Comercio { get { return comercio; } set { comercio = value; } }
        public Idioma Idioma { get { return idioma; } set { idioma = value; } }
        public Telefono Telefono { get { return telefono; } set { telefono = value; } }
        public Domicilio Domicilio { get { return domicilio; } set { domicilio = value; } }
        public GrupoBase Grupo { get { return grupo; } set { grupo = value; } }
        public PreguntaDeSeguridad PreguntaDeSeguridad { get { return preguntaDeSeguridad; } set { preguntaDeSeguridad = value; } }
        #endregion

        public Usuario() { }

        public override string ToString()
        {
            return Nombre + ", " + Apellido;
        }

        public int CalcularDVH()
        {
            return cantIntentos +
                //(habilitado == true ? 1 : 0) +
                //(bloqueado == true ? 1 : 0) +
                //+
                Sexo.Value.ToString().Length +
                (nombre.Length) +
                (apellido.Length) +
                (fechaDeNacimiento.ToString("dd/mm/yyyy").Length) +
                email.Length +
                fechaCreacion.ToString("dd/mm/yyyy").Length +
                fechaBaja.ToString("dd/mm/yyyy").Length;
        }

        public int ObtenerDVHOriginal()
        {
            return DVH;
        }

        public string getDescripcion()
        {
            return this.ToString();
        }
    }
}
