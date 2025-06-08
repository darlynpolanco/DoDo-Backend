namespace DoDo.Configuration
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; } // Esencial para la conexión
        public string UserName { get; set; } // CORREGIDO: Debe coincidir con el JSON
        public string Password { get; set; }
        public string SenderName { get; set; } // AÑADIDO
        public string SenderEmail { get; set; } // AÑADIDO
    }
}

        //public string Host { get; set; }
        //public int Port { get; set; }
        //public bool EnableSsl { get; set; }
        //public string User { get; set; }
        //public string Password { get; set; }
