using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FeriaVirtualWeb.Models.DataContext;

namespace FeriaVirtualWeb.Models.DataManager
{
    public class UsuarioManager
    {
        public USUARIO GetUsuario(string rut, string password)
        {
            var usuario = new USUARIO();
            using (FeriaVirtualEntities db = new FeriaVirtualEntities())
            {

                if (!string.IsNullOrEmpty(rut) && !string.IsNullOrEmpty(password))
                {
                    
                    using (MD5 md5Hash = MD5.Create())
                    {
                        string hash = GetMd5Hash(md5Hash, password);

                        if (VerifyMd5Hash(md5Hash, password, hash))
                        {
                            usuario = db.USUARIO.FirstOrDefault(p => p.RUTUSUARIO == rut && p.CONTRASENA == hash);
                        }
                        else
                        {
                            Console.WriteLine("The hashes are not same.");
                        }
                      
                    }
                    
                }

                return usuario;
            }
            
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}