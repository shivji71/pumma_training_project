using PummaApplication.Models;
using PummaApplication.Repo;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;


namespace PummaApplication.Controllers
{
    public class WebApiController : ApiController
    {
        // interface instance 
        private readonly ICityAndStateZip _cityAndStateZip;
        private readonly RegisterUser _registerUser;
        private readonly LoginUser _loginUser;

        public WebApiController()
        {
            _cityAndStateZip = new CityAndStateZip();
            _registerUser = new RegisterUser();
            _loginUser = new LoginUser();
        }


        //get request to access cities and states by zip code
        [HttpGet]
        [Route("api/Get_Cities")]
        public IHttpActionResult Get_Cities_State(string zipcd)
        {
            try
            {
                
                if (string.IsNullOrEmpty(zipcd))
                {
                    //return StatusCode(500, new { Message = "Please Enter Valid Zip Code" });
                    return BadRequest("Please enter a valid ZIP code");
                }
                var cities = _cityAndStateZip.GetAllCitiesAndState(zipcd);
                //if (cities == null)
                //{
                //    return StatusCode(500, new { Message = "City Or State Not Found" });
                //}
                if (cities == null || cities.Count == 0)
                {
                    return NotFound();
                }

                return Ok(cities);
            }
            catch (Exception ex)
            {
                //return StatusCode(500, new { Message ="An error occurred while processing the request" });
                return InternalServerError(ex);
            }
           
        }

        // get request to access all users OR perticular user by user ID
        [HttpGet]
        [Route("api/Get_All_Users")]
        public IHttpActionResult GetAllUsers(int user_id)
        {
            try
            {
                if (user_id < 0)
                {
                    return BadRequest("Please enter a valid User ID");
                }

                var users = _registerUser.GetAllUsers(user_id);
                if (users == null)
                {
                    string errorMessage = "User Not Found";
                    return Content(HttpStatusCode.NotFound, new { Message = errorMessage });
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // post request to save new user details
        [HttpPost]
        [Route("api/save_user")]
        public IHttpActionResult RegisterNewUser(Register user)
        {
            try
            {
                var result = _registerUser.RegisterNewUser(user);
                if (result.status == false)
                {
                    return BadRequest(result.message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // post request to login user and access welcome user details
        [HttpPost]
        [Route("api/login_user")]
        public IHttpActionResult SignInUser(Login user)
        {
            if(user == null)
            {
                return BadRequest("Please Enter Valid Username OR Password");

            }
            try
            {
                var result = _loginUser.SignInUser(user);
                if (result.status == false)
                {
                    return BadRequest(result.message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


        // post request to update user details
        [HttpPost]
        [Route("api/edit_user")]
        public IHttpActionResult UpdateUser(Register user)
        {
            try
            {
                var result = _registerUser.UpdateUser(user);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // get request to delete user by ID
        [HttpGet]
        [Route("api/delete_user")]
        public IHttpActionResult DeleteUserById(int user_id, int deletedby)
        {
            if (user_id < 0)
            {
                return BadRequest("Please Enter Valid User ID");

            }
            try
            {
                var result = _registerUser.DeleteUser(user_id,deletedby);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // upload file to server
        [HttpPost]
        [Route("api/uploadfile")]
        public IHttpActionResult UploadFile()
        {
            var httpRequest = HttpContext.Current.Request;
            var file = httpRequest.Files[0];
            var server = HttpContext.Current.Server;
            var fileName = Path.GetFileName(file.FileName);
            var physicalPath = Path.Combine(server.MapPath("~/App_Data"), fileName);
            return Ok(new { message = "File uploaded successfully" });
        }
    }
}
