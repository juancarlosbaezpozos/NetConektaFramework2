using System;
using System.Text;
using System.Net;
//*API para realizar pagos en coonekta, usada en Framework 2.0*//
//Nota:Las id son sensitivas a mayusculas y minusculas
namespace ConektaFramework2
{
    public class ConektaPagos
    {
        string privatekey = string.Empty, publickey = string.Empty, version = "0.3.0";

        public ConektaPagos(string llavePrivada,string llavePublica,string versionConekta)
        {
            this.privatekey = llavePrivada;
            this.publickey = llavePublica;
            if (!string.IsNullOrEmpty(versionConekta))
            {
                this.version = versionConekta;
            }
        }

        //Base
        private HttpWebRequest _conektaBase(string sURL,string sMetodo,int nTipoLlave) //0 = privada | 1 = publica
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(sURL);
            webRequest.KeepAlive = false;
            webRequest.ContentType = "application/json";
            webRequest.Method = sMetodo;
            webRequest.Accept = string.Format("application/vnd.conekta-v{0}+json", this.version);
            if(nTipoLlave == 1)//publica
                webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(this.publickey + ":"));
            else//privada
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");     
            return webRequest;
        }

        private string _conektaBaseResultado(HttpWebRequest webRequest)
        {
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                string sResultado = string.Empty;
                using (var reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                {
                    sResultado = reader.ReadToEnd();
                }
                webResponse.Close();
                return sResultado;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Planes
        public string crearPlan(string sId,string sName,string sAmount,string sInterval,string sFrecuency)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(sId))
                {
                    sb.AppendFormat("\"id\":\"{0}\",", sId);
                }
                if (!string.IsNullOrEmpty(sName))
                {
                    sb.AppendFormat("\"name\":\"{0}\",", sName);
                }
                else
                {
                    return "Error:Requiere Nombre";
                }
                if (!string.IsNullOrEmpty(sAmount))
                {
                    sb.AppendFormat("\"amount\":\"{0}\",", sAmount);
                }
                else
                {
                    return "Error:Requiere Cantidad";
                }
                sb.Append("\"currency\":\"MXN\",");
                if (!string.IsNullOrEmpty(sInterval))
                {
                    sb.AppendFormat("\"interval\":\"{0}\",", sInterval);
                }
                if (!string.IsNullOrEmpty(sFrecuency))
                {
                    sb.AppendFormat("\"frequency\":\"{0}\",", sFrecuency);
                }

                sBody = "{"+sb.ToString().Remove(sb.ToString().Length - 1)+"}";

                HttpWebRequest webRequest = _conektaBase("https://api.conekta.io/plans", "POST", 0);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al crear Plan ({0})",ex.Message);
            }
        }

        public string editarPlan(string sIdOld,string sIdNew, string sName, string sAmount, string sInterval, string sFrecuency)
        {
            try
            {
                bool band = false;
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(sIdNew))
                {
                    sb.AppendFormat("\"id\":\"{0}\",", sIdNew);
                    band = true;
                }
                if (!string.IsNullOrEmpty(sName))
                {
                    sb.AppendFormat("\"name\":\"{0}\",", sName);
                    band = true;
                }
                if (!string.IsNullOrEmpty(sAmount))
                {
                    sb.AppendFormat("\"amount\":\"{0}\",", sAmount);
                    band = true;
                }
                if (!string.IsNullOrEmpty(sInterval))
                {
                    sb.AppendFormat("\"interval\":\"{0}\",", sInterval);
                    band = true;
                }
                if (!string.IsNullOrEmpty(sFrecuency))
                {
                    sb.AppendFormat("\"frequency\":\"{0}\",", sFrecuency);
                    band = true;
                }

                if (band)
                {
                    sBody = "{"+sb.ToString().Remove(sb.ToString().Length - 1)+"}";

                    HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/plans/{0}", sIdOld), "PUT", 0);
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                    {
                        writer.Write(sBody);
                    }
                    return _conektaBaseResultado(webRequest);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al editar Plan ({0})",ex.Message);
            }
        }

        public string eliminarPlan(string sId)
        {
            try
            {
                if (!String.IsNullOrEmpty(sId))
                {
                    HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/plans/{0}", sId), "DELETE", 0);
                    return _conektaBaseResultado(webRequest);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al eliminar Plan ({0})",ex.Message);
            }
        }

        //Clientes
        public string crearCliente(string sName, string sEmail,string sPhone,string sCards,string sPlan)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(sName))
                {
                    sb.AppendFormat("\"name\":\"{0}\",", sName);
                }
                else
                {
                    return "Error:Requiere Nombre";
                }
                if (!string.IsNullOrEmpty(sEmail))
                {
                    sb.AppendFormat("\"email\":\"{0}\",", sEmail);
                }
                else
                {
                    return "Error:Requiere Correo";
                }
                if (!string.IsNullOrEmpty(sPhone))
                {
                    sb.AppendFormat("\"phone\":\"{0}\",", sPhone);
                }
                if (!string.IsNullOrEmpty(sCards))
                {
                    sb.AppendFormat("\"cards\":[\"{0}\"],", sCards);
                }
                if (!string.IsNullOrEmpty(sPlan))
                {
                    sb.AppendFormat("\"plan\":\"{0}\",", sPlan);
                }

                sBody = "{" + sb.ToString().Remove(sb.ToString().Length - 1) + "}";

                HttpWebRequest webRequest = _conektaBase("https://api.conekta.io/customers", "POST", 0);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al crear Cliente ({0})",ex.Message);
            }
        }

        public string editarCliente(string sId,string sName,string sEmail)
        {
            try
            {
                bool band = false;
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(sName))
                {
                    sb.AppendFormat("\"name\":\"{0}\",", sName);
                    band = true;
                }
                if (!string.IsNullOrEmpty(sEmail) && band)
                {
                    sb.AppendFormat("\"email\":\"{0}\",", sEmail);
                    band = true;
                }
                else
                    band = false;
                if (band)
                {
                    sBody = "{" + sb.ToString().Remove(sb.ToString().Length - 1) + "}";

                    HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}", sId), "PUT", 0);
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                    {
                        writer.Write(sBody);
                    }
                    return _conektaBaseResultado(webRequest);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al editar Cliente ({0})",ex.Message);
            }
        }

        public string eliminarCliente(string sId)
        {
            try
            {
                if (!String.IsNullOrEmpty(sId))
                {
                    HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}", sId), "DELETE", 0);
                    return _conektaBaseResultado(webRequest);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al eliminar Cliente ({0})",ex.Message);
            }
        }
    
        //Tarjetas        
        public string agregarTarjeta(string sIdCliente, string sToken)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error:Requiere ID del Cliente";
                }
                if (!string.IsNullOrEmpty(sToken))
                {
                    sb.AppendFormat("\"token\":\"{0}\"", sToken);
                }
                else
                {
                    return "Error:Requiere Token";
                }

                sBody = "{" + sb.ToString() + "}";

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/cards/", sIdCliente), "POST", 0);
                
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al agregar Tarjeta ({0})",ex.Message);
            }
        }

        public string editarTarjeta(string sIdCliente, string sIdTarjeta,string sToken)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error:Requiere ID del Cliente";
                }
                if (string.IsNullOrEmpty(sIdTarjeta))
                {
                    return "Error:Requiere el ID de la Tarjeta";
                }
                if (!string.IsNullOrEmpty(sToken))
                {
                    sb.AppendFormat("\"token\":\"{0}\"", sToken);
                }
                else
                {
                    return "Error:Requiere el token";
                }

                sBody = "{" + sb.ToString() + "}";

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/cards/{1}", sIdCliente, sIdTarjeta), "PUT", 0);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al editar Tarjeta ({0})",ex.Message);
            }
        }

        public string eliminarTarjeta(string sIdCliente, string sIdTarjeta)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error:Requiere ID del Cliente";
                }
                if (string.IsNullOrEmpty(sIdTarjeta))
                {
                    return "Error:Requiere ID de la Tarjeta";
                }
                
                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/cards/{1}", sIdCliente, sIdTarjeta), "DELETE", 0);
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al eliminar Tarjeta ({0})",ex.Message);
            }
        }
    
        //Suscripciones
        public string crearSuscripcion(string sIdCliente,string sIdPlan,string sIdTarjeta)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error: Requiere el Id del Cliente";
                }
                if (!string.IsNullOrEmpty(sIdPlan))
                {
                    sb.AppendFormat("\"plan\":\"{0}\",",sIdPlan);
                }
                else
                {
                    return "Error:Requiere el id Plan";
                }
                if (!string.IsNullOrEmpty(sIdTarjeta))
                {
                    sb.AppendFormat("\"card\":\"{0}\",", sIdTarjeta);
                }
                
                sBody = "{" + sb.ToString().Remove(sb.ToString().Length - 1) + "}";

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/subscription", sIdCliente), "POST", 0);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }

                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al crear Suscripcion ({0})",ex.Message);
            }
        }

        public string editarSuscripcion(string sIdCliente, string sIdPlan, string sIdTarjeta)
        {
            try
            {
                bool band = false;
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error: Requiere el Id del Cliente";
                }
                if (!string.IsNullOrEmpty(sIdPlan))
                {
                    sb.AppendFormat("\"plan\":\"{0}\",", sIdPlan);
                    band = true;
                }
                if (!string.IsNullOrEmpty(sIdTarjeta))
                {
                    sb.AppendFormat("\"card\":\"{0}\",", sIdTarjeta);
                    band = true;
                }

                if (band)
                {
                    sBody = "{" + sb.ToString().Remove(sb.ToString().Length - 1) + "}";
                    HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/subscription", sIdCliente), "PUT", 0);
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                    {
                        writer.Write(sBody);
                    }
                    return _conektaBaseResultado(webRequest);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al editar Suscripcion ({0})",ex.Message);
            }
        }

        public string pausarSuscripcion(string sIdCliente)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error: Requiere el Id del Cliente";
                }

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/subscription/pause", sIdCliente), "POST", 0);
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al pausar Suscripcion ({0})",ex.Message);
            }
        }

        public string reactivarSuscripcion(string sIdCliente)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error: Requiere el Id del Cliente";
                }

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/subscription/resume", sIdCliente), "POST", 0);
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al reactivar Suscripcion ({0})",ex.Message);
            }
        }

        public string cancelarSuscripcion(string sIdCliente)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sIdCliente))
                {
                    return "Error: Requiere el Id del Cliente";
                }

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/customers/{0}/subscription/cancel", sIdCliente), "POST", 0);
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al cancelar Suscripcion ({0})",ex.Message);
            }
        }

        //Cargos
        public string crearCargo(string sDescription, string sAmount,string sReferenceID,string sCardToken)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(sDescription))
                {
                    sb.AppendFormat("\"description\":\"{0}\",", sDescription);
                }
                else
                {
                    return "Error:Requiere Descripcion";
                }
                if (!string.IsNullOrEmpty(sAmount))
                {
                    sb.AppendFormat("\"amount\":\"{0}\",", sAmount);
                }
                else 
                {
                    return "Error:Requiere Cantidad";
                }
                sb.Append("\"currency\":\"MXN\",");
                if (!string.IsNullOrEmpty(sReferenceID))
                {
                    sb.AppendFormat("\"reference_id\":\"{0}\",", sReferenceID);
                }
                if (!string.IsNullOrEmpty(sCardToken))
                {
                    sb.AppendFormat("\"card\":\"{0}\",", sCardToken);
                }

                sBody = "{" + sb.ToString().Remove(sb.ToString().Length - 1) + "}";

                HttpWebRequest webRequest = _conektaBase("https://api.conekta.io/charges", "POST", 1);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al crear Cargo ({0})",ex.Message);
            }
        }

        public string getCargo(string sId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (String.IsNullOrEmpty(sId))
                {
                    return "Error:Requiere Id del Cargo";
                }

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/charges/{0}", sId), "GET", 0);
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error al consultar Cargo ({0})");
            }
        }

        public string devolucionCargo(string sId, string sAmount)
        {
            try
            {
                string sBody = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (string.IsNullOrEmpty(sId))
                {
                    return "Error:Requiere Id Cargo";
                }
                if (!string.IsNullOrEmpty(sAmount))
                {
                    sb.AppendFormat("\"amount\":{0}", sAmount);
                }
                else
                {
                    return "Error:Requiere Cantidad";
                }
               
                sBody = "{" + sb.ToString() + "}";

                HttpWebRequest webRequest = _conektaBase(string.Format("https://api.conekta.io/charges/{0}/refund", sId), "POST", 1);
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
                return _conektaBaseResultado(webRequest);
            }
            catch (Exception ex)
            {
                return string.Format("Error:A currido un error en la devolucion de un Cargo ({0})",ex.Message);
            }
        }
    }
}
