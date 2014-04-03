using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
//*API para realizar pagos en coonekta, usada en Framework 2.0*//
//Nota:Las id son sensitivas a mayusculas y minusculas
namespace ConektaFramework2
{
    public class ConektaPagos
    {
        string privatekey = string.Empty,publickey = string.Empty;

        public ConektaPagos(string llavePrivada,string llavePublica)
        {
            this.privatekey = llavePrivada;
            this.publickey = llavePublica;
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://api.conekta.io/plans");
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }

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
                return "Error:A currido un error al crear Plan";
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

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/plans/{0}", sIdOld));
                    webRequest.KeepAlive = false;
                    webRequest.ContentType = "application/json";
                    webRequest.Method = "PUT";
                    webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                    webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                    {
                        writer.Write(sBody);
                    }

                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                    string sResultado = string.Empty;
                    using (var reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                    {
                        sResultado = reader.ReadToEnd();
                    }
                    webResponse.Close();
                    return sResultado;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return "Error:A currido un error al editar Plan";
            }
        }

        public string eliminarPlan(string sId)
        {
            try
            {
                if (!String.IsNullOrEmpty(sId))
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/plans/{0}",sId));
                    webRequest.KeepAlive = false;
                    //webRequest.ContentType = "application/json";
                    webRequest.Method = "DELETE";
                    webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                    webRequest.Credentials = new NetworkCredential(this.privatekey, "");

                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                    string sResultado = string.Empty;
                    using (var reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                    {
                        sResultado = reader.ReadToEnd();
                    }
                    webResponse.Close();
                    return sResultado;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return "Error:A currido un error al eliminar Plan";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://api.conekta.io/customers");
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }

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
                return "Error:A currido un error al crear Cliente";
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

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}", sId));
                    webRequest.KeepAlive = false;
                    webRequest.ContentType = "application/json";
                    webRequest.Method = "PUT";
                    webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                    webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                    {
                        writer.Write(sBody);
                    }
                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                    string sResultado = string.Empty;
                    using (var reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                    {
                        sResultado = reader.ReadToEnd();
                    }
                    webResponse.Close();
                    return sResultado;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return "Error:A currido un error al editar Cliente";
            }
        }

        public string eliminarCliente(string sId)
        {
            try
            {
                if (!String.IsNullOrEmpty(sId))
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}",sId));
                    webRequest.KeepAlive = false;
                    //webRequest.ContentType = "application/json";
                    webRequest.Method = "DELETE";
                    webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                    webRequest.Credentials = new NetworkCredential(this.privatekey, "");

                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                    string sResultado = string.Empty;
                    using (var reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                    {
                        sResultado = reader.ReadToEnd();
                    }
                    webResponse.Close();
                    return sResultado;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return "Error:A currido un error al eliminar Cliente";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/cards/",sIdCliente));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }

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
                return "Error:A currido un error al agregar Tarjeta";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/cards/{1}", sIdCliente, sIdTarjeta));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "PUT";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }

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
                return "Error:A currido un error al editar Tarjeta";
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
                
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/cards/{1}", sIdCliente,sIdTarjeta));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "DELETE";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
               
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
                return "Error:A currido un error al eliminar Tarjeta";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/subscription", sIdCliente));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }

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
                return "Error:A currido un error al crear Suscripcion";
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
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/subscription", sIdCliente));
                    webRequest.KeepAlive = false;
                    webRequest.ContentType = "application/json";
                    webRequest.Method = "PUT";
                    webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                    webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                    {
                        writer.Write(sBody);
                    }
                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                    string sResultado = string.Empty;
                    using (var reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                    {
                        sResultado = reader.ReadToEnd();
                    }
                    webResponse.Close();
                    return sResultado;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return "Error:A currido un error al editar Suscripcion";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/subscription/pause", sIdCliente));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
                
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
                return "Error:A currido un error al pausar Suscripcion";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/subscription/resume", sIdCliente));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");

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
                return "Error:A currido un error al reactivar Suscripcion";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/customers/{0}/subscription/cancel", sIdCliente));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");

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
                return "Error:A currido un error al cancelar Suscripcion";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://api.conekta.io/charges");
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.publickey, "");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
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
                string msj = ex.Message;
                return "Error:A currido un error al crear Cargo";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.conekta.io/charges/{0}",sId));
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "GET";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.privatekey, "");
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
                return "Error:A currido un error al consultar Cargo";
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

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(" https://api.conekta.io/charges/{0}/refund");
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/json";
                webRequest.Method = "POST";
                webRequest.Accept = "application/vnd.conekta-v0.3.0+json";
                webRequest.Credentials = new NetworkCredential(this.publickey, "");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(webRequest.GetRequestStream()))
                {
                    writer.Write(sBody);
                }
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
                return "Error:A currido un error en la devolucion de un Cargo";
            }
        }
    }
}
