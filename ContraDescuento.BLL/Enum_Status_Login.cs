using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public enum Enum_Status_Login
    {
        UsuarioPwdInvalido = 1,
        Exitoso = 2,
        Bloqueado = 3,
        Inexistente = 4,
        Baja = 5
    }
}
