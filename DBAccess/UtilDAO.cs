using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Hosting;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using System.Net.Mail;
using System.Threading;
using Newtonsoft.Json;

using WcfService.Common;
using WcfService.Model;
using WcfService.DAO;

using HttpMultipartParser;

namespace WcfService.DAO
{
    public class UtilDAO
    {
        private static string SMTPSERVER = "box520.bluehost.com";//"smtp.gmail.com";
        private static int PORTNO = 25; //587;

        const int huge_size = 720;
        const int big_size = 320;
        const int small_size = 100;
        const int small_size_ = 200;

        const string rs_url = "http://rs.abc.com/images/100/";

        string connectionString = ConfigurationManager.AppSettings["cm_conn"].ToString();

        #region Upload image

        public async Task<notify_image_upload> ProductUploadFile(Stream stream)
        {
            notify_image_upload myList = new notify_image_upload();
            myList.status = false;
            myList.url = "error";
            myList.image_id = -1;

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header = headers["product_id"];
            int _product_id = header == null ? 0 : Convert.ToInt32(header);
           

            try
            {
                if (stream.Length == 0) return myList;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //throw ex; // Logs.writeToLogFile(ex.ToString());
            }

            var parser = new MultipartFormDataParser(stream);

            var fileName_ = parser.Files[0].FileName.ToString();
            
            Stream data = parser.Files[0].Data;

            var tmpFileName = "PD" + _product_id.ToString() + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(fileName_);
            var tmp_big_filename = "PD" + _product_id.ToString() + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(fileName_);
            var tmp_small_filename = "PD" + _product_id.ToString() + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(fileName_);

            string FilePath = Config.ImageFolder + "/" + huge_size.ToString() + "/" + tmpFileName;
            string FilePathThumbs = Config.ImageFolder + "/" + big_size.ToString() + "/" + tmp_big_filename;
            string FilePathThumbs_ = Config.ImageFolder + "/" + small_size.ToString() + "/" + tmp_small_filename;

            if (await Token.AuthenticatedCheck())
            {

                this.ProcessFile(data, FilePath, FilePathThumbs, FilePathThumbs_);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "s_upload_image";
                            cmd.Parameters.AddWithValue("@client_id", null);
                            cmd.Parameters.AddWithValue("@outlet_id", null);
                            cmd.Parameters.AddWithValue("@product_id", _product_id);
                            cmd.Parameters.AddWithValue("@program_id", null);
                            cmd.Parameters.AddWithValue("@user_id", null);
                            cmd.Parameters.AddWithValue("@url", tmpFileName);
                            cmd.Parameters.AddWithValue("@last_id", SqlDbType.BigInt);
                            cmd.Parameters["@last_id"].Direction = ParameterDirection.Output;

                            cmd.Connection = conn;
                            //conn.Open();
                            await conn.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                            //cmd.ExecuteNonQuery();

                            myList.status = true;
                            myList.url = rs_url + tmpFileName;
                            myList.image_id = Convert.ToInt64(cmd.Parameters["@last_id"].Value);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logs.writeToLogFile(ex.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - product_uploadimg_token_required" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return myList;
        }

        public async Task<notify_image_upload> UserUploadFile(Stream stream)
        {
            notify_image_upload myList = new notify_image_upload();
            myList.status = false;
            myList.url = "error";
            myList.image_id = -1;

            var headers = WebOperationContext.Current.IncomingRequest.Headers;
            var header = headers["user_id"];
            int _user_id = header == null ? 0 : Convert.ToInt32(header);

            try
            {
                if (stream.Length == 0) return myList;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //throw ex; // Logs.writeToLogFile(ex.ToString());
            }

            var parser = new MultipartFormDataParser(stream);


            var fileName_ = parser.Files[0].FileName.ToString();
            Stream data = parser.Files[0].Data;

            var tmpFileName = "U" + _user_id.ToString() + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(fileName_);
            var tmp_big_filename = "U" + _user_id.ToString() + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(fileName_);
            var tmp_small_filename = "U" + _user_id.ToString() + DateTime.Now.ToString("ddMMyyhhmmss") + Path.GetExtension(fileName_);

            string FilePath = Config.ImageFolder + "/" + huge_size.ToString() + "/" + tmpFileName;
            string FilePathThumbs = Config.ImageFolder + "/" + big_size.ToString() + "/" + tmp_big_filename;
            string FilePathThumbs_ = Config.ImageFolder + "/" + small_size.ToString() + "/" + tmp_small_filename;

            if (await Token.AuthenticatedCheck())
            {
                //Stream huge_image = ImageResize(data, ImageFormat.Jpeg, huge_size);
                Stream big_image = ImageResize(data, ImageFormat.Jpeg, big_size);
                //saving thumnail
                Write_stream_to_server(FilePathThumbs, big_image);

                Stream thumbnail_image = ImageResize(data, ImageFormat.Jpeg, small_size_);
                //save thumbnail
                Write_stream_to_server(FilePathThumbs_, thumbnail_image);

                //saving orginal image
                //Write_stream_to_server(FilePath, huge_image);

                this.ProcessFile(data, FilePath, FilePathThumbs, FilePathThumbs_);

                //save on db
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "s_upload_image";
                            cmd.Parameters.AddWithValue("@client_id", null);
                            cmd.Parameters.AddWithValue("@outlet_id", null);
                            cmd.Parameters.AddWithValue("@product_id", null);
                            cmd.Parameters.AddWithValue("@program_id", null);
                            cmd.Parameters.AddWithValue("@user_id", _user_id);
                            cmd.Parameters.AddWithValue("@url", tmpFileName);
                            cmd.Parameters.AddWithValue("@last_id", SqlDbType.BigInt);
                            cmd.Parameters["@last_id"].Direction = ParameterDirection.Output;

                            cmd.Connection = conn;
                            //conn.Open();
                            //cmd.ExecuteNonQuery();

                            await conn.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();

                            myList.status = true;
                            myList.url = rs_url + tmpFileName;
                            myList.image_id = Convert.ToInt64(cmd.Parameters["@last_id"].Value);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logs.writeToLogFile(ex.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                ErrorDetail err = new ErrorDetail() { error_info = "Error", error_detail = "error on - user_uploadimg" };
                throw new WebFaultException<ErrorDetail>(err, HttpStatusCode.Forbidden);
            }
            return myList;
        }

        #endregion

        #region email

        //http://www.codeproject.com/Tips/685171/Send-Email-using-WCF-Service
        //http://www.aspsnippets.com/Articles/How-to-send-email-Asynchronously-in-ASPNet-using-Background-Thread.aspx

        public string SendEmail(string gmailUserName
                                    , string gmailUserPassword
                                    , string[] emailToAddress
                                    , string[] ccemailTo
                                    , string subject
                                    , string body
                                    , bool isBodyHtml)
        {
            if (gmailUserName == null || gmailUserName.Trim().Length == 0)
            {
                return "User Name Empty";
            }
            if (gmailUserPassword == null || gmailUserPassword.Trim().Length == 0)
            {
                return "Email Password Empty";
            }
            if (emailToAddress == null || emailToAddress.Length == 0)
            {
                return "Email To Address Empty";
            }

            List<string> tempFiles = new List<string>();

            SmtpClient smtpClient = new SmtpClient(SMTPSERVER, PORTNO);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(gmailUserName, gmailUserPassword);
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(gmailUserName);
                message.Subject = subject == null ? "" : subject;
                message.Body = body == null ? "" : body;
                message.IsBodyHtml = isBodyHtml;

                foreach (string email in emailToAddress)
                {
                    message.To.Add(email);
                }

                if (ccemailTo != null && ccemailTo.Length > 0)
                {
                    foreach (string emailCc in ccemailTo)
                    {
                        message.CC.Add(emailCc);
                    }
                }
                try
                {
                    smtpClient.Send(message);
                    return "Email Send SuccessFully";
                }
                catch
                {
                    return "Email Send failed";
                }
            }
        }


        #endregion

        #region private

        private void Write_stream_to_server(string FilePath, Stream data) //Stream
        {
            int length = 0;
            try
            {
                Thread postimage = new Thread(delegate ()
                {
                    using (Stream writer = new FileStream(FilePath, FileMode.Create))
                    {
                        int readCount;

                        //var buffer = new byte[65000]; //new byte[8192];
                        byte[] buffer = new byte[65000];

                        //while ((readCount = data.Read(buffer, 0, buffer.Length)) != 0)  //while ((readCount = stream.Read(buffer, 0, buffer.Length)) != 0)
                        while ((readCount = data.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            writer.Write(buffer, 0, readCount);
                            length += readCount;
                        }

                        writer.Close();
                        writer.Dispose();
                    }
                });
                postimage.IsBackground = true;
                postimage.Start();
                Thread.Sleep(0);



                ///////////////////////////////
                //RemoteFileInfo request;

                //FileStream targetStream = null;
                //Stream sourceStream = request.FileByteStream;

                //string uploadFolder = @"C:\upload\";

                //string filePath = Path.Combine(uploadFolder, request.FileName);

                //using (targetStream = new FileStream(filePath, FileMode.Create,
                //                      FileAccess.Write, FileShare.None))
                //{
                //    //read from the input stream in 65000 byte chunks

                //    const int bufferLen = 65000;
                //    byte[] buffer = new byte[bufferLen];
                //    int count = 0;
                //    while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                //    {
                //        // save to output stream
                //        targetStream.Write(buffer, 0, count);
                //    }
                //    targetStream.Close();
                //    sourceStream.Close();
                //}
            }
            catch (Exception ex)
            {
                Logs.writeToLogFile(ex.ToString());
            }

        }

        private async void ProcessFile(Stream d, string fn1, string fn2, string fn3)
        {

            //Stream huge_image = ImageResize(d, ImageFormat.Jpeg, huge_size);
            Stream huge_image = await Task.FromResult<Stream>(ImageResize(d, ImageFormat.Jpeg, huge_size));
            //saving orginal image

            //Task t1 = new Task(delegate { Write_stream_to_server(fn1, huge_image); });
            await Task.Run(() => Write_stream_to_server(fn1, huge_image));
            //Write_stream_to_server(FilePath, huge_image);

            //Stream big_image = ImageResize(d, ImageFormat.Jpeg, big_size);
            Stream big_image = await Task.FromResult<Stream>(ImageResize(d, ImageFormat.Jpeg, big_size));
            //saving thumnail

            //Task t2 = new Task(delegate { Write_stream_to_server(fn2, big_image); });
            await Task.Run(() => Write_stream_to_server(fn2, big_image));
            //Write_stream_to_server(FilePathThumbs, big_image);

            //Stream thumbnail_image = ImageResize(d, ImageFormat.Jpeg, small_size_);
            Stream thumbnail_image = await Task.FromResult<Stream>(ImageResize(d, ImageFormat.Jpeg, small_size_));

            //save thumbnail
            //Task t3 = new Task(delegate { Write_stream_to_server(fn3, thumbnail_image); });
            await Task.Run(() => Write_stream_to_server(fn3, thumbnail_image));

            //Write_stream_to_server(FilePathThumbs_, thumbnail_image);

            //t1.Start();
            //t2.Start();
            //t3.Start();
        }

        public static System.IO.Stream ImageResize(System.IO.Stream inputStream, System.Drawing.Imaging.ImageFormat contentType, Int32 maximumDimension)
        {

            System.IO.Stream result = new System.IO.MemoryStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream);
            try
            {
                Int32 thumbnailWidth = (img.Width > img.Height) ? maximumDimension : img.Width * maximumDimension / img.Height;
                Int32 thumbnailHeight = (img.Width > img.Height) ? img.Height * maximumDimension / img.Width : maximumDimension;

                //System.Drawing.Image thumbnail = img.GetThumbnailImage(thumbnailWidth, thumbnailHeight, null, IntPtr.Zero);
                //thumbnail.Save(result, contentType);
                //result.Seek(0, 0);

                var thumbnailBitmap = new Bitmap(thumbnailWidth, thumbnailHeight);

                var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imageRectangle = new Rectangle(0, 0, thumbnailWidth, thumbnailHeight);
                thumbnailGraph.DrawImage(img, imageRectangle);
                thumbnailBitmap.Save(result, img.RawFormat);

                result.Seek(0, 0);

                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                //thumbnail.Dispose();
            }
            catch (Exception ex)
            {
                Logs.writeToLogFile(ex.ToString());
            }
            return result;
        }

        #endregion
    }

}