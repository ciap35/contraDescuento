using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class Logueese : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ClearHeaders();
            Response.AddHeader("REFRESH", "5;URL=Index.aspx");
        }
    }
}