using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.DAL.Actualizaciones
{
    public class DALActualizaciones
    {
        public static DataSet SuppliersWHStockHeader_iUP(int idSupplierWH, int idSupplierWHCode, decimal stockLevel, decimal stockCodes,
            string StockDate, string StockTime)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                string[] fecha = StockDate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string fechaParam = fecha[2] + "/" + fecha[1] + "/" + fecha[0];

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@idSupplierWH", idSupplierWH);
                parametros.Add("@idSupplierWHCode", idSupplierWHCode);
                parametros.Add("@stockLevel", stockLevel);
                parametros.Add("@stockCodes", stockCodes);
                parametros.Add("@StockDate", fechaParam);
                parametros.Add("@StockTime", StockTime);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[common].[SuppliersWHStockHeader_iUP]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet SuppliersWHStockDetail_dUP(int idSupplierWH, int idSupplierWHCode)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@idSupplierWH", idSupplierWH);
                parametros.Add("@idSupplierWHCode", idSupplierWHCode);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[common].[SuppliersWHStockDetail_dUP]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet SuppliersWHStockDetail_iUP(int idSupplierWH, int idSupplierWHCode, decimal barCode, decimal stockLevel,
            decimal qtyStockSafety, decimal qtyStockForSale, decimal qtyStockReserved, string StockDate, string StockTime, int bitInsertDMOk)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                string[] fecha = StockDate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string fechaParam = fecha[2] + "/" + fecha[1] + "/" + fecha[0];

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@idSupplierWH", idSupplierWH);
                parametros.Add("@idSupplierWHCode", idSupplierWHCode);
                parametros.Add("@barCode", barCode);
                parametros.Add("@stockLevel", stockLevel);
                parametros.Add("@qtyStockSafety", qtyStockSafety);
                parametros.Add("@qtyStockForSale", qtyStockForSale);
                parametros.Add("@qtyStockReserved", qtyStockReserved);
                parametros.Add("@StockDate", fechaParam);
                parametros.Add("@StockTime", StockTime);
                parametros.Add("@bitInsertDMOk", bitInsertDMOk);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[common].[SuppliersWHStockDetail_iUP]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        #region "Datos Maestros"
        public static DataSet up_Corp_Vendor_Del_Inventario(int idSupplierWH, int idSupplierWHCode)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DM"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@NumProv_LIFNR", idSupplierWH);
                parametros.Add("@Clave_Almacen", idSupplierWHCode);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_Corp_Vendor_Del_Inventario]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet up_Corp_Vendor_Ins_Inventario(int idSupplierWH, int idSupplierWHCode, decimal barCode, decimal stockLevel,
            decimal qtyStockSafety, decimal qtyStockForSale, decimal qtyStockReserved, string StockDate, string StockTime)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DM"].ConnectionString);

                string[] fecha = StockDate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string fechaParam = fecha[2] + "/" + fecha[1] + "/" + fecha[0] + " " + StockTime;

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@NumProv_LIFNR", idSupplierWH);
                parametros.Add("@Clave_Almacen", idSupplierWHCode);
                parametros.Add("@barCode", barCode);
                parametros.Add("@LABST", stockLevel);
                parametros.Add("@PreOrden", qtyStockReserved);
                parametros.Add("@Stock_Seguridad", qtyStockSafety);
                parametros.Add("@Fec_Movto", fechaParam);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_Corp_Vendor_Ins_Inventario]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet upCorpOMS_Cns_UeNoStock(string IdOwner, string IdTienda)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DM"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                if(!string.IsNullOrEmpty(IdOwner))
                    parametros.Add("@IdOwner", IdOwner);

                if (!string.IsNullOrEmpty(IdTienda))
                    parametros.Add("@IdTienda", IdTienda);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoStock]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        #endregion


    }
}