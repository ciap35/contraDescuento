using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class Producto
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Producto() { }

        public void Alta(ref BE.Producto producto)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@titulo", producto.Titulo);
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    conexion.sqlCmd.Parameters.Add("@precio",(System.Data.SqlTypes.SqlDecimal)producto.Precio);
                    conexion.sqlCmd.Parameters.AddWithValue("@descuento", (System.Data.SqlTypes.SqlDecimal)producto.Descuento);
                    //conexion.sqlCmd.Parameters.AddWithValue("@cantidad", producto.Cantidad); //BORRAR
                    if (producto.Categoria != null && producto.Categoria.CodCategoria > 0)
                        conexion.sqlCmd.Parameters.AddWithValue("@codCategoria", producto.Categoria.CodCategoria);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@codCategoria", DBNull.Value);
                    if (producto.SubCategoria != null && producto.SubCategoria.CodSubCategoria > 0)
                        conexion.sqlCmd.Parameters.AddWithValue("@codSubCategoria", producto.SubCategoria.CodSubCategoria);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@codSubCategoria", DBNull.Value);

                    if (producto.FechaVigenciaDesde != DateTime.MinValue)
                        conexion.sqlCmd.Parameters.AddWithValue("@fechaVigenciaDesde", producto.FechaVigenciaDesde);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@fechaVigenciaDesde", DBNull.Value);
                    if (producto.FechaVigenciaHasta != DateTime.MinValue)
                        conexion.sqlCmd.Parameters.AddWithValue("@fechaVigenciaHasta", producto.FechaVigenciaHasta);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@fechaVigenciaHasta", DBNull.Value);
                    if (!producto.MantenerFoto)
                    {
                        if (producto.Foto == null)
                            conexion.sqlCmd.Parameters.AddWithValue("@foto", System.Data.SqlTypes.SqlBinary.Null);
                        else
                            conexion.sqlCmd.Parameters.AddWithValue("@foto", producto.Foto);
                    }
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@foto", System.Data.SqlTypes.SqlBinary.Null);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        producto.CodProducto = Convert.ToInt32(conexion.sqlReader["codProducto"]);
                    };
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void Baja(BE.Producto producto,BE.Comercio comercio,BE.Domicilio domicilio)
        //{
        //    try
        //    {
        //        using (conexion = new Acceso.Conexion())
        //        {
        //            conexion.Abrir();
        //            conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            conexion.sqlCmd.CommandText = "ProductoBaja";
        //            conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
        //            conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
        //            conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", domicilio.CodDomicilio);
        //            conexion.sqlCmd.ExecuteNonQuery();

        //            conexion.Cerrar();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Modificar(BE.Producto producto)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@titulo", producto.Titulo);
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    conexion.sqlCmd.Parameters.AddWithValue("@precio", producto.Precio);
                    conexion.sqlCmd.Parameters.AddWithValue("@descuento", producto.Descuento);
                    //conexion.sqlCmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    if (producto.Categoria != null && producto.Categoria.CodCategoria > 0)
                        conexion.sqlCmd.Parameters.AddWithValue("@codCategoria", producto.Categoria.CodCategoria);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@codCategoria", DBNull.Value);
                    if (producto.SubCategoria != null && producto.SubCategoria.CodSubCategoria > 0)
                        conexion.sqlCmd.Parameters.AddWithValue("@codSubCategoria", producto.SubCategoria.CodSubCategoria);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@codSubCategoria", DBNull.Value);


                    conexion.sqlCmd.Parameters.AddWithValue("@fechaVigenciaDesde", producto.FechaVigenciaDesde);
                    conexion.sqlCmd.Parameters.AddWithValue("@fechaVigenciaHasta", producto.FechaVigenciaHasta);
                    if (!producto.MantenerFoto)
                        conexion.sqlCmd.Parameters.AddWithValue("@foto", producto.Foto);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@foto", System.Data.SqlTypes.SqlBinary.Null);

                    conexion.sqlCmd.ExecuteNonQuery();

                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Producto producto, BE.Comercio comercio)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.ExecuteNonQuery();

                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BE.Producto Obtener(BE.Producto producto)
        {
            BE.Producto _producto = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        _producto = new BE.Producto();
                        _producto.CodProducto = producto.CodProducto;
                        _producto.Titulo = Convert.ToString(conexion.sqlReader["titulo"]);
                        _producto.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                        _producto.Precio = Convert.ToDecimal(conexion.sqlReader["precio"]);
                        if (conexion.sqlReader["descuento"] != DBNull.Value)
                            _producto.Descuento = Convert.ToDecimal(conexion.sqlReader["descuento"]);
                        //_producto.Cantidad = Convert.ToInt32(conexion.sqlReader["cantidad"]);
                        _producto.FechaVigenciaDesde = Convert.ToDateTime(conexion.sqlReader["fechaVigenciaDesde"]);
                        _producto.FechaVigenciaHasta = Convert.ToDateTime(conexion.sqlReader["fechaVigenciaHasta"]);
                        _producto.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["fechaCreacion"]);
                        if (conexion.sqlReader["fechaBaja"] != DBNull.Value)
                            _producto.FechaBaja = Convert.ToDateTime(conexion.sqlReader["fechaBaja"]);
                        if (conexion.sqlReader["fechaModificacion"] != DBNull.Value)
                            _producto.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["fechaModificacion"]);
                        _producto.Foto = (byte[])(conexion.sqlReader["foto"]);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _producto;
        }

        public List<BE.Producto> Listar(BE.Comercio comercio)
        {
            List<BE.Producto> lstProducto = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoPuntoDeVentaListar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        lstProducto = new List<BE.Producto>();
                    while (conexion.sqlReader.Read())
                    {
                        BE.Producto prod = new BE.Producto();
                        prod.CodProducto = Convert.ToInt32(conexion.sqlReader["Producto.codProducto"]);
                        prod.Titulo = Convert.ToString(conexion.sqlReader["Producto.titulo"]);
                        prod.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        prod.Precio = Convert.ToDecimal(conexion.sqlReader["Producto.precio"]);
                        if (conexion.sqlReader["Producto.descuento"] != DBNull.Value)
                            prod.Descuento = Convert.ToDecimal(conexion.sqlReader["Producto.descuento"]);
                       
                        if (conexion.sqlReader["Producto.Categoria"] != DBNull.Value) { 
                         if(prod.Categoria==null)
                                prod.Categoria = new BE.Categoria();
                            prod.Categoria.CodCategoria = Convert.ToInt32(conexion.sqlReader["Producto.Categoria"]);
                            prod.Categoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.Categoria.Descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.codSubCategoria"] != DBNull.Value)
                        {
                            if (prod.SubCategoria == null)
                                prod.SubCategoria = new BE.SubCategoria();
                            prod.SubCategoria.CodSubCategoria= Convert.ToInt32(conexion.sqlReader["Producto.codSubCategoria"]);
                            prod.SubCategoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.SubCategoria.Descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.fechaVigenciaDesde"] != DBNull.Value)
                            prod.FechaVigenciaDesde = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaDesde"]);
                        if (conexion.sqlReader["Producto.fechaVigenciaHasta"] != DBNull.Value)
                            prod.FechaVigenciaHasta = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaHasta"]);
                        prod.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["Producto.fechaCreacion"]);
                        if (conexion.sqlReader["Producto.fechaBaja"] != DBNull.Value)
                            prod.FechaBaja = Convert.ToDateTime(conexion.sqlReader["Producto.fechaBaja"]);
                        if (conexion.sqlReader["Producto.fechaModificacion"] != DBNull.Value)
                            prod.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["Producto.fechaModificacion"]);
                        if (conexion.sqlReader["Producto.foto"] != DBNull.Value)
                            prod.Foto = (byte[])(conexion.sqlReader["Producto.foto"]);
                        DAL.Domicilio domDAL = new DAL.Domicilio();
                        BE.Domicilio dom = new BE.Domicilio() { CodDomicilio = Convert.ToInt32(conexion.sqlReader["Domicilio.codDomicilio"]) };
                        dom = domDAL.Obtener(dom);
                        prod.PuntosDeVenta.Add(dom);
                        if (conexion.sqlReader["Producto_PuntoDeVenta.cantidad"] != DBNull.Value && dom != null)
                            prod.PuntosDeVentaStock.Add(dom, Convert.ToInt32(conexion.sqlReader["Producto_PuntoDeVenta.cantidad"]));
                       
                        
                        lstProducto.Add(prod);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstProducto;
        }

        public List<BE.Categoria> ListarCategoria()
        {
            List<BE.Categoria> lstCategoria = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoCategoriaListar";

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        lstCategoria = new List<Categoria>();
                    while (conexion.sqlReader.Read())
                    {
                        BE.Categoria categoria = new Categoria();
                        categoria.CodCategoria = Convert.ToInt32(conexion.sqlReader["codProductoCategoria"]);
                        categoria.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                        lstCategoria.Add(categoria);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstCategoria;
        }

        public List<BE.SubCategoria> ListarSubCategoria(BE.Categoria categoria)
        {
            List<BE.SubCategoria> lstSubCategoria = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoSubCategoriaListar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProductoCategoria", categoria.CodCategoria);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        lstSubCategoria = new List<BE.SubCategoria>();
                    while (conexion.sqlReader.Read())
                    {
                        BE.SubCategoria SubCategoria = new BE.SubCategoria();
                        SubCategoria.CodSubCategoria = Convert.ToInt32(conexion.sqlReader["codProductoSubcategoria"]);
                        SubCategoria.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                        lstSubCategoria.Add(SubCategoria);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstSubCategoria;
        }
    }
}
