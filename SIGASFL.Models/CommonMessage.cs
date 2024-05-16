using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Models
{
    public class CommonMessage
    {
        public int Code { get; set; }
        public string Title { get; set; }
        public HttpStatusCode StatusCode { get; set; }


        /// <summary>
        /// Code = 0, Message = ClientResponse<object>.SuccesMessage, StatusCode = System.Net.HttpStatusCode.OK
        /// </summary>
        public static CommonMessage SUCCESS
        {
            get
            {
                return new CommonMessage
                {
                    Code = ClientResponse<object>.DefaultSuccessCode,
                    Title = ClientResponse<object>.DefaultSuccessMsg,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
        }


        /// <summary>
        /// Code = 7001, Title = "The record could not be created", StatusCode = System.Net.HttpStatusCode.NotModified
        /// </summary>
        public static CommonMessage ERROR_RECORD_NOT_CREATED
        {
            get
            {
                return new CommonMessage { Code = 7001, Title = "Record could not be created", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 7002, Title = "The record could not be found", StatusCode = System.Net.HttpStatusCode.NotFound 
        /// </summary>
        public static CommonMessage ERROR_RECORD_NOT_FOUND
        {
            get
            {
                return new CommonMessage { Code = 7002, Title = "Registro no encontrado", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        public static CommonMessage ERROR_USERNAME_NOT_FOUND
        {
            get
            {
                return new CommonMessage { Code = 9002, Title = "Sorry, we don't recognize that email address or phone number", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 7003, Title = "The record could not be removed", StatusCode = System.Net.HttpStatusCode.InternalServerError 
        /// </summary>
        public static CommonMessage ERROR_RECORD_NOT_REMOVED
        {
            get
            {
                return new CommonMessage { Code = 7003, Title = "El registro no pudo ser eliminado", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 7004, Title = "The record could not be updated", StatusCode = System.Net.HttpStatusCode.InternalServerError
        /// </summary>
        public static CommonMessage ERROR_RECORD_NOT_UPDATED
        {
            get
            {
                return new CommonMessage { Code = 7004, Title = "El registro no pudo ser actualizado", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 9999, Title = "Exception: {0}", StatusCode = System.Net.HttpStatusCode.InternalServerError
        /// </summary>
        public static CommonMessage ERROR_EXCEPTION
        {

            get
            {
                return new CommonMessage { Code = 9999, Title = "System Exception: {0}", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 9998, Title = "Bad request", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_BAD_REQUEST
        {

            get
            {
                return new CommonMessage { Code = 9998, Title = "Bad Reqquest", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }

        /// <summary>
        /// Code = 8001, Title = "Usuario o clave invalidos", StatusCode = System.Net.HttpStatusCode.Unauthorized
        /// </summary>
        public static CommonMessage ERROR_AUTH_INVALID_USERNAME
        {

            get
            {
                return new CommonMessage { Code = 8001, Title = "Email Address is not valid.", StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }
        }

        /// <summary>
        /// Code = 8002, Title = "Usuario o clave invalidos", StatusCode = System.Net.HttpStatusCode.Unauthorized
        /// </summary>
        public static CommonMessage ERROR_AUTH_INVALID_USERNAME_LOGIN
        {
            get
            {
                return new CommonMessage { Code = 8002, Title = "Invalid Email Address or Password.", StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }
        }

        /// <summary>
        /// Code = 8003, Title = "The token could not be generated", StatusCode = System.Net.HttpStatusCode.Unauthorized
        /// </summary>
        public static CommonMessage ERROR_GENERATING_TOKEN
        {
            get
            {
                return new CommonMessage { Code = 8003, Title = "El token no pudo ser generado", StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }
        }


        /// <summary>
        /// Code = 8006, Title = "Password not updated", StatusCode = System.Net.HttpStatusCode.InternalServerError
        /// </summary>
        public static CommonMessage ERROR_PASS_NOT_UPDATED
        {

            get
            {
                return new CommonMessage { Code = 8006, Title = "La clave no pudo ser actualizada", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 8007, Title = "The user has already registered", StatusCode = System.Net.HttpStatusCode.InternalServerError
        /// </summary>
        public static CommonMessage ERROR_USER_ALREADY_REGISTERED
        {
            get
            {
                return new CommonMessage { Code = 8007, Title = "El usuario ya fue registrado", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 8008, Title = "The email address is invalid", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_INVALID_EMAIL
        {
            get
            {
                return new CommonMessage { Code = 8008, Title = "Email Address is not valid.", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }

        /// <summary>
        /// Code = 8009, Title = "The authorization to access the system is pending.", StatusCode = System.Net.HttpStatusCode.Unauthorized
        /// </summary>
        public static CommonMessage ERROR_USER_UNAUTHORIZED
        {
            get
            {
                return new CommonMessage { Code = 8009, Title = "Autorización para acceder al sistema esta pendiente de aprobación.", StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }
        }

        /// <summary>
        /// Code = 8010, Title = "Las claves no coinciden.", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_PASSWORD_NOT_MATCH
        {

            get
            {
                return new CommonMessage { Code = 8010, Title = "Las claves no coinciden.", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }


        /// <summary>
        /// Code = 8011, Title = "Token invalido", StatusCode = System.Net.HttpStatusCode.Unauthorized
        /// </summary>
        public static CommonMessage ERROR_INVALID_TOKEN
        {
            get
            {
                return new CommonMessage { Code = 8011, Title = "Token invalido", StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }
        }



        /// <summary>
        /// Code = 8012, Title = "Solicitud de usuario invalida o expirada", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_USER_REQUEST_NOT_FOUND
        {
            get
            {
                return new CommonMessage { Code = 8012, Title = "Solicitud de usuario invalida o expirada", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }


        /// <summary>
        /// Code = 8013, Title = "La clave es obligatoria para la confirmacion. Favor Validar.", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_USER_CONFIRMATION_INVALID_PASSWORD
        {
            get
            {
                return new CommonMessage { Code = 8013, Title = "La clave es obligatoria para la confirmacion. Favor Validar.", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }



        /// <summary>
        /// Code = 8014, Title = "Ocurrio una excepcion en la confirmacion del usuario.", StatusCode = System.Net.HttpStatusCode.InternalServerError
        /// </summary>
        public static CommonMessage ERROR_USER_CONFIRMATION_EXCEPTION
        {
            get
            {
                return new CommonMessage { Code = 8014, Title = "Ocurrio una excepcion en la confirmacion del usuario.", StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }

        /// <summary>
        /// Code = 8015, Title = "EL correo electronico ya fue registrado.", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_USER_REQUEST_EMAIL_REGISTERED
        {
            get
            {
                return new CommonMessage { Code = 8015, Title = "This Email is already in use.", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }


        /// <summary>
        /// Code = 8016, Title = "Correo electronico ya tiene una solicitud de correo pendiente.", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_USER_REQUEST_EMAIL_PENDING
        {
            get
            {
                return new CommonMessage { Code = 8016, Title = "Correo electronico ya tiene una solicitud de correo pendiente.", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }

        /// <summary>
        /// Code = 8017, Title = "Correo electronico ya tiene una solicitud de correo pendiente.", StatusCode = System.Net.HttpStatusCode.BadRequest
        /// </summary>
        public static CommonMessage ERROR_PASSWORD_REQUIRED
        {
            get
            {
                return new CommonMessage { Code = 8017, Title = "Password is required.", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }
        }

        public static void SetMessage<T>(CommonMessage commonMessage, ref ClientResponse<T> clientResponse, params object[] args)
        {
            clientResponse.Code = commonMessage.Code;
            clientResponse.Title = args.Length == 0 ? commonMessage.Title : string.Format(commonMessage.Title, args);
            clientResponse.Messages.Add(clientResponse.Title);
            clientResponse.StatusCode = commonMessage.StatusCode;
            clientResponse.IsSuccess = (commonMessage.Code == ClientResponse<T>.DefaultSuccessCode);
        }

    }
}
