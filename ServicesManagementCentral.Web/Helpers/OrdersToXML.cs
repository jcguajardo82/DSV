using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServicesManagement.Web.Helpers
{
    public class OrdersToXML
    {
        public XDocument CreateXMLDocument(DataSet dsOrder, string nroOrder)
        {

            //LogW Logging = new LogW("Inicio");

            // se llena la tabla del encabecado de la orden
            DataTable dt2 = dsOrder.Tables[0];

            // se llena la tabla con el detalle de los prodcutos en el carrito
            DataTable dt3 = dsOrder.Tables[1];

            // se llena la tabla con el detalle del flete
            DataTable dt4 = dsOrder.Tables[2];

            // Logging.LogWrite("Datatables [dt2, dt3, dt4] recuperados");

            string XmlOrder = nroOrder;
            //Logging.LogWrite("Datatables recuperados para Orden:" + XmlOrder);

            XDocument ordersOut = null;

            try
            {

                XNamespace nm = "http://www.demandware.com/xml/impex/order/2006-10-31";
                //Logging.LogWrite("Namespace:" + nm.ToString());

                XElement XmlSETC1 = new XElement(nm + "order", new XAttribute("order-no", dt2.Rows[0][0].ToString()),
                    new XElement(nm + "order-date", dt2.Rows[0][1].ToString()),
                    new XElement(nm + "created-by", dt2.Rows[0][2].ToString()),
                    // new XElement(nm + "store-number", dt2.Rows[0][3].ToString()),
                    new XElement(nm + "original-order-no", dt2.Rows[0][4].ToString()),
                    new XElement(nm + "currency", dt2.Rows[0][5].ToString()),
                    new XElement(nm + "customer-locale", dt2.Rows[0][6].ToString()),
                    new XElement(nm + "taxation", dt2.Rows[0][7].ToString()),
                    new XElement(nm + "invoice-no", dt2.Rows[0][8].ToString()),

                    // informacion del cliente (customer)
                    new XElement(nm + "customer",
                        new XElement(nm + "customer-name", dt2.Rows[0][9].ToString()),
                        new XElement(nm + "customer-email", dt2.Rows[0][10].ToString()),
                        new XElement(nm + "billing-address",
                            new XElement(nm + "first-name", dt2.Rows[0][11].ToString()),
                            new XElement(nm + "last-name", dt2.Rows[0][12].ToString()),
                            // new XElement(nm + "company-name", dt2.Rows[0][20].ToString()),
                            new XElement(nm + "address1", dt2.Rows[0][13].ToString()),
                            new XElement(nm + "address2", dt2.Rows[0][14].ToString()),
                            new XElement(nm + "city", dt2.Rows[0][15].ToString()),
                            new XElement(nm + "postal-code", dt2.Rows[0][16].ToString()),
                            new XElement(nm + "state-code", dt2.Rows[0][17].ToString()),
                            new XElement(nm + "country-code", dt2.Rows[0][18].ToString()),
                            new XElement(nm + "phone", dt2.Rows[0][19].ToString()),
                            // new XElement(nm + "rfc", dt2.Rows[0][21].ToString())

                            // Company Name - custom atribute
                            new XElement(nm + "custom-attributes",
                                    new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "company-name"), dt2.Rows[0][20].ToString())
                                )
                        )
                    ),



                    // Status de la orden
                    new XElement(nm + "status",
                        new XElement(nm + "order-status", dt2.Rows[0][22].ToString()),
                        new XElement(nm + "shipping-status", dt2.Rows[0][23].ToString()),
                        new XElement(nm + "confirmation-status", dt2.Rows[0][24].ToString()),
                        new XElement(nm + "payment-status", dt2.Rows[0][25].ToString())
                    ),

                    new XElement(nm + "current-order-no", dt2.Rows[0][26].ToString())
                    );

                // Listado del carrito de compra (producto a producto)
                XElement XmlSETCProducts = new XElement(nm + "product-lineitems", "");

                // ciclo para nodos de los productos del carrito
                int nrows3 = dt3.Rows.Count;

                if (nrows3 > 0)
                {
                    XElement XmlSETCProductTMP = new XElement(nm + "product-lineitem", "");
                    float GrossAdjust = 0;

                    for (int i = 0; i < nrows3; i++)
                    {
                        XmlSETCProductTMP = new XElement(nm + "product-lineitem",
                                new XElement(nm + "net-price", dt3.Rows[i][0].ToString()),
                                new XElement(nm + "tax", dt3.Rows[i][1].ToString()),
                                new XElement(nm + "gross-price", dt3.Rows[i][2].ToString()),
                                new XElement(nm + "base-price", dt3.Rows[i][3].ToString()),
                                new XElement(nm + "lineitem-text", dt3.Rows[i][4].ToString()),
                                new XElement(nm + "tax-basis", dt3.Rows[i][5].ToString()),
                                // new XElement(nm + "position", dt3.Rows[i][6].ToString()),
                                new XElement(nm + "position", i + 1),
                                new XElement(nm + "product-id", dt3.Rows[i][7].ToString()),
                                new XElement(nm + "product-name", dt3.Rows[i][8].ToString()),
                                new XElement(nm + "quantity", new XAttribute("unit", dt3.Rows[i][10].ToString()), dt3.Rows[i][9].ToString()),
                                new XElement(nm + "tax-rate", dt3.Rows[i][11].ToString()),
                                new XElement(nm + "shipment-id", dt3.Rows[i][12].ToString()),
                                new XElement(nm + "gift", dt3.Rows[i][13].ToString()),

                                // barcode - custom atribute
                                new XElement(nm + "custom-attributes",
                                        new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "barcode"), dt3.Rows[i][24].ToString()),
                                        new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "PriceOffer"), dt3.Rows[i][0].ToString()),
                                        new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "conversionFactor"),"1.0"),
                                        new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "productNote"), dt3.Rows[i][25].ToString()),
                                        new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "salesUnit"), dt3.Rows[i][10].ToString())
                                    )

                                );

                        // Validar si el gross-price trae valor distinto a 0
                        GrossAdjust = Convert.ToSingle(dt3.Rows[i][16].ToString().Trim());

                        if (GrossAdjust > 0)
                        {
                            XElement XmlSETCProductAdjust = new XElement(nm + "price-adjustments",
                                new XElement(nm + "price-adjustment",
                                        new XElement(nm + "net-price", dt3.Rows[i][14].ToString()),
                                        new XElement(nm + "tax", dt3.Rows[i][15].ToString()),
                                        new XElement(nm + "gross-price", dt3.Rows[i][16].ToString()),
                                        new XElement(nm + "base-price", dt3.Rows[i][17].ToString()),
                                        new XElement(nm + "lineitem-text", dt3.Rows[i][18].ToString()),
                                        new XElement(nm + "tax-basis", dt3.Rows[i][19].ToString()),
                                        new XElement(nm + "promotion-id", dt3.Rows[i][20].ToString()),
                                        new XElement(nm + "campaign-id", dt3.Rows[i][21].ToString())
                                    // new XElement(nm + "additional-points", dt3.Rows[i][22].ToString()),
                                    // new XElement(nm + "redemption-point", dt3.Rows[i][23].ToString())
                                    )
                                    );
                            XmlSETCProductTMP.Add(XmlSETCProductAdjust);
                        }

                        XmlSETCProducts.Add(XmlSETCProductTMP);
                    }
                }
                else
                {
                    XElement XmlSETCProduct = new XElement(nm + "product-lineitem",
                             new XElement(nm + "net-price", "0.00"),
                             new XElement(nm + "tax", "0.00"),
                             new XElement(nm + "gross-price", "0.00"),
                             new XElement(nm + "base-price", "0.00"),
                             new XElement(nm + "lineitem-text", "undefined"),
                             new XElement(nm + "tax-basis", "0.00"),
                             new XElement(nm + "position", "0"),
                             new XElement(nm + "product-id", "undefined"),
                             new XElement(nm + "product-name", "undefined"),
                             new XElement(nm + "quantity", new XAttribute("unit", ""), "0"),
                             new XElement(nm + "tax-rate", "0.0"),
                             new XElement(nm + "shipment-id", "0"),
                             new XElement(nm + "gift", "false"),
                             // barcode - custom atribute
                             new XElement(nm + "custom-attributes",
                                    new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "barcode"), "0")
                                ),

                             new XElement(nm + "price-adjustments",
                                new XElement(nm + "price-adjustment",
                                     new XElement(nm + "net-price", "0.00"),
                                     new XElement(nm + "tax", "0.00"),
                                     new XElement(nm + "gross-price", "0.00"),
                                     new XElement(nm + "base-price", "0.00"),
                                     new XElement(nm + "lineitem-text", "undefined"),
                                     new XElement(nm + "tax-basis", "0.00"),
                                     new XElement(nm + "promotion-id", "undefined"),
                                     new XElement(nm + "campaign-id", "undefined")
                                // new XElement(nm + "additional-points", "0"),
                                // new XElement(nm + "redemption-point", "0")
                                )
                            )
                         );
                    XmlSETCProducts.Add(XmlSETCProduct);
                }

                // descripción y costes del envio (shipment)
                int nrows4 = dt4.Rows.Count;
                XElement XmlSETCShipping = new XElement(nm + "shipping-lineitems", "");

                if (nrows4 > 0)
                {
                    XmlSETCShipping = new XElement(nm + "shipping-lineitems",
                            new XElement(nm + "shipping-lineitem",
                                new XElement(nm + "net-price", dt4.Rows[0][0].ToString()),
                                new XElement(nm + "tax", dt4.Rows[0][1].ToString()),
                                new XElement(nm + "gross-price", dt4.Rows[0][2].ToString()),
                                new XElement(nm + "base-price", dt4.Rows[0][3].ToString()),
                                new XElement(nm + "lineitem-text", dt4.Rows[0][4].ToString()),
                                new XElement(nm + "tax-basis", dt4.Rows[0][5].ToString()),
                                // check - adjustment-shipping
                                new XElement(nm + "item-id", dt4.Rows[0][6].ToString()),
                                new XElement(nm + "shipment-id", dt4.Rows[0][7].ToString()),
                                new XElement(nm + "tax-rate", dt4.Rows[0][8].ToString())
                            )
                        );
                }
                else
                {
                    XmlSETCShipping = new XElement(nm + "shipping-lineitems",
                            new XElement(nm + "shipping-lineitem",
                                new XElement(nm + "net-price", "0.00"),
                                new XElement(nm + "tax", "0.00"),
                                new XElement(nm + "gross-price", "0.00"),
                                new XElement(nm + "base-price", "0.00"),
                                new XElement(nm + "lineitem-text", ""),
                                new XElement(nm + "tax-basis", "0.00"),
                                new XElement(nm + "item-id", "undefined"),
                                new XElement(nm + "shipment-id", "0"),
                                new XElement(nm + "tax-rate", "0.00")
                            )
                        );
                }

                // informacion detallada del envio (shipment) con sus totales.
                XElement XmlSETCShipments = new XElement(nm + "shipments",
                        new XElement(nm + "shipment", new XAttribute("shipment-id", dt2.Rows[0][27].ToString()),
                            new XElement(nm + "status",
                                new XElement(nm + "shipping-status", dt2.Rows[0][28].ToString())
                            ),

                            new XElement(nm + "shipping-method", dt2.Rows[0][29].ToString()),

                            // direccion de envio.
                            new XElement(nm + "shipping-address",
                                new XElement(nm + "first-name", dt2.Rows[0][30].ToString()),
                                new XElement(nm + "last-name", dt2.Rows[0][31].ToString()),
                                new XElement(nm + "address1", dt2.Rows[0][32].ToString()),
                                new XElement(nm + "address2", dt2.Rows[0][33].ToString()),
                                // new XElement(nm + "reference", dt2.Rows[0][34].ToString()),
                                // new XElement(nm + "geolocation", dt2.Rows[0][35].ToString()),
                                new XElement(nm + "city", dt2.Rows[0][36].ToString()),
                                new XElement(nm + "postal-code", dt2.Rows[0][37].ToString()),
                                new XElement(nm + "state-code", dt2.Rows[0][38].ToString()),
                                new XElement(nm + "country-code", dt2.Rows[0][39].ToString()),
                                new XElement(nm + "phone", dt2.Rows[0][40].ToString()),

                                new XElement(nm + "custom-attributes",
                                    new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "RFC"), dt2.Rows[0][21].ToString()),
                                    new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "geolocation"), dt2.Rows[0][35].ToString()),
                                    new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "reference"), dt2.Rows[0][34].ToString())
                                )
                            ),

                            new XElement(nm + "gift", dt2.Rows[0][41].ToString()),

                            // totales del envio
                            new XElement(nm + "totals",
                                new XElement(nm + "merchandize-total",
                                    new XElement(nm + "net-price", dt2.Rows[0][42].ToString()),
                                    new XElement(nm + "tax", dt2.Rows[0][43].ToString()),
                                    new XElement(nm + "gross-price", dt2.Rows[0][44].ToString())
                                ),
                                new XElement(nm + "adjusted-merchandize-total",
                                    new XElement(nm + "net-price", dt2.Rows[0][45].ToString()),
                                    new XElement(nm + "tax", dt2.Rows[0][46].ToString()),
                                    new XElement(nm + "gross-price", dt2.Rows[0][47].ToString())
                                ),
                                new XElement(nm + "shipping-total",
                                    new XElement(nm + "net-price", dt2.Rows[0][48].ToString()),
                                    new XElement(nm + "tax", dt2.Rows[0][49].ToString()),
                                    new XElement(nm + "gross-price", dt2.Rows[0][50].ToString())
                                ),
                                new XElement(nm + "adjusted-shipping-total",
                                    new XElement(nm + "net-price", dt2.Rows[0][51].ToString()),
                                    new XElement(nm + "tax", dt2.Rows[0][52].ToString()),
                                    new XElement(nm + "gross-price", dt2.Rows[0][53].ToString())
                                ),
                                new XElement(nm + "shipment-total",
                                    new XElement(nm + "net-price", dt2.Rows[0][54].ToString()),
                                    new XElement(nm + "tax", dt2.Rows[0][55].ToString()),
                                    new XElement(nm + "gross-price", dt2.Rows[0][56].ToString())
                                )
                            ),

                            // deliveryDate - custom atribute
                            new XElement(nm + "custom-attributes",
                                new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "deliveryDate"), dt2.Rows[0][84].ToString())
                            )

                        )
                    );

                // informacion detallada de los totales de la orden
                XElement XmlSETTotals = new XElement(nm + "totals",
                        new XElement(nm + "merchandize-total",
                            new XElement(nm + "net-price", dt2.Rows[0][57].ToString()),
                            new XElement(nm + "tax", dt2.Rows[0][58].ToString()),
                            new XElement(nm + "gross-price", dt2.Rows[0][59].ToString())
                        ),
                        new XElement(nm + "adjusted-merchandize-total",
                            new XElement(nm + "net-price", dt2.Rows[0][60].ToString()),
                            new XElement(nm + "tax", dt2.Rows[0][61].ToString()),
                            new XElement(nm + "gross-price", dt2.Rows[0][62].ToString())
                        ),
                        new XElement(nm + "shipping-total",
                            new XElement(nm + "net-price", dt2.Rows[0][63].ToString()),
                            new XElement(nm + "tax", dt2.Rows[0][64].ToString()),
                            new XElement(nm + "gross-price", dt2.Rows[0][65].ToString())
                        ),
                        new XElement(nm + "adjusted-shipping-total",
                            new XElement(nm + "net-price", dt2.Rows[0][66].ToString()),
                            new XElement(nm + "tax", dt2.Rows[0][67].ToString()),
                            new XElement(nm + "gross-price", dt2.Rows[0][68].ToString())
                        ),
                        new XElement(nm + "order-total",
                            new XElement(nm + "net-price", dt2.Rows[0][69].ToString()),
                            new XElement(nm + "tax", dt2.Rows[0][70].ToString()),
                            new XElement(nm + "gross-price", dt2.Rows[0][71].ToString())
                        )
                    );

                // detalle del pago
                XElement XmlSETCPayments = new XElement(nm + "payments", "");
                if (dt2.Rows[0][86].ToString() == "21" || dt2.Rows[0][86].ToString() == "22")
                    {
                        XmlSETCPayments = new XElement(nm + "payments",
                                new XElement(nm + "payment",
                                    new XElement(nm + "credit-card",
                                        new XElement(nm + "card-type", dt2.Rows[0][72].ToString()),
                                        new XElement(nm + "card-number", dt2.Rows[0][73].ToString()),
                                        new XElement(nm + "card-holder", dt2.Rows[0][74].ToString()),
                                        new XElement(nm + "card-token", dt2.Rows[0][75].ToString()),
                                        new XElement(nm + "expiration-month", dt2.Rows[0][76].ToString()),
                                        new XElement(nm + "expiration-year", dt2.Rows[0][77].ToString())
                                    ),
                                    new XElement(nm + "amount", dt2.Rows[0][78].ToString()),
                                    new XElement(nm + "processor-id", dt2.Rows[0][79].ToString()),
                                    new XElement(nm + "transaction-id", dt2.Rows[0][80].ToString())
                                )
                            );
                    }
                if (dt2.Rows[0][86].ToString() == "5" || dt2.Rows[0][86].ToString() == "6")
                {
                    XmlSETCPayments = new XElement(nm + "payments",
                            new XElement(nm + "payment",
                                new XElement(nm + "custom-method",
                                    new XElement(nm + "method-name", dt2.Rows[0][87].ToString())
                                ),
                                    new XElement(nm + "amount", dt2.Rows[0][78].ToString()),
                                    new XElement(nm + "processor-id", dt2.Rows[0][87].ToString()),
                                    new XElement(nm + "transaction-id", dt2.Rows[0][80].ToString())
                            )
                        );
                }

                // IP del cliente - desde donde accede.
                XElement XmlSETCRemoteHost = new XElement(nm + "remoteHost", dt2.Rows[0][81].ToString());

                // custom atributes - al final de la orden
                XElement XmlSETCCustom = new XElement(nm + "custom-attributes", "");

                if (dt2.Rows[0][86].ToString() == "21" || dt2.Rows[0][86].ToString() == "22")
                {
                    XmlSETCCustom = new XElement(nm + "custom-attributes",
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "additionalPoints"), dt2.Rows[0][82].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "redeemedPoints"), dt2.Rows[0][83].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "storeID"), dt2.Rows[0][3].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "orderNote"), dt2.Rows[0][85].ToString())
                    );
                }

                if (dt2.Rows[0][86].ToString() == "5")
                {
                    XmlSETCCustom = new XElement(nm + "custom-attributes",
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "expectedCashAmount"), dt2.Rows[0][88].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "additionalPoints"), dt2.Rows[0][82].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "redeemedPoints"), dt2.Rows[0][83].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "storeID"), dt2.Rows[0][3].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "orderNote"), dt2.Rows[0][85].ToString())
                    );
                }

                if (dt2.Rows[0][86].ToString() == "6")
                {
                    XmlSETCCustom = new XElement(nm + "custom-attributes",
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "expectedVoucherAmount"), dt2.Rows[0][88].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "additionalPoints"), dt2.Rows[0][82].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "redeemedPoints"), dt2.Rows[0][83].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "storeID"), dt2.Rows[0][3].ToString()),
                            new XElement(nm + "custom-attribute", new XAttribute("attribute-id", "orderNote"), dt2.Rows[0][85].ToString())
                    );
                }

                XmlSETC1.Add(XmlSETCProducts);
                XmlSETC1.Add(XmlSETCShipping);
                XmlSETC1.Add(XmlSETCShipments);
                XmlSETC1.Add(XmlSETTotals);
                XmlSETC1.Add(XmlSETCPayments);
                XmlSETC1.Add(XmlSETCRemoteHost);
                XmlSETC1.Add(XmlSETCCustom);

                //Logging.LogWrite("XML: Objetos XmlSetc1 creados y llenos");

                // se crea el objeto base XmlSETC
                XElement XmlSETC = new XElement(nm + "orders", "");
                XmlSETC.Add(XmlSETC1);

                // Logging.LogWrite("XML: Objeto XmlSETC se ha complementado" );

                RemoveEmptyNamespace(XmlSETC);

                // Logging.LogWrite("XML: Crear objeto XDcoument orders");
                XDocument orders = new XDocument(
                  new XDeclaration("1.0", "utf-8", "yes"),
                  XmlSETC
                  );

                // Logging.LogWrite("XML: Objeto XDcoument orders esta lleno");

                ordersOut = orders;
                //Logging.LogWrite("XML: Objeto XDcoument Orders/ordersOut estan lleno");
            }
            catch (Exception ex)
            {
                string returnMessage = "error:" + ex.ToString();
                //Logging.LogWrite("Incidenecia:");
                //Logging.LogWrite(returnMessage);
            }

            return ordersOut;
        }

        private static void RemoveEmptyNamespace(XElement element)
        {
            XAttribute attr = element.Attribute("xmlns");
            if (attr != null && string.IsNullOrEmpty(attr.Value))
                attr.Remove();
            foreach (XElement el in element.Elements())
                RemoveEmptyNamespace(el);
        }
    }
}