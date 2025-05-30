using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace JPOC_VM_DEMO.Common
{
    /// <summary>
    /// 各種ユーティリティ（Webに特化した部分を記述)
    /// </summary>
    public class Utilities : Jpoc.Tools.Core.Utilities
    {
        private static IHttpContextAccessor _httpContextAccessor;

        // Initialize this in Program.cs or Startup.cs
        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        #region Client IP Related
        public static string GetXForwardedForIpAddress()
        {
            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                if (request != null)
                {
                    if (request.Headers.TryGetValue("HTTP_X_FORWARDED_FOR", out var values))
                    {
                        return values.ToString();
                    }
                }
            }
            catch
            {
                // Swallow exception as per original code
            }
            return string.Empty;
        }

        public static string GetRemoteIpAddress()
        {
            try
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null)
                {
                    return context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                }
            }
            catch
            {
                // Swallow exception as per original code
            }
            return string.Empty;
        }

        public static string GetClientIP()
        {
            try
            {
                var context = _httpContextAccessor.HttpContext;
                if (context?.Request != null)
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                    var tempIp = GetXForwardedForIpAddress();
                    if (string.IsNullOrEmpty(tempIp))
                    {
                        tempIp = GetRemoteIpAddress();
                    }

                    if (tempIp.Contains(","))
                    {
                        var ips = tempIp.Split(',');
                        tempIp = ips[^1].Trim(); // Get rightmost IP
                    }

                    if (!string.IsNullOrEmpty(tempIp) &&
                        Regex.IsMatch(tempIp, @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$"))
                    {
                        ip = tempIp;
                    }
                    return ip;
                }
            }
            catch
            {
                return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }
        #endregion

        #region File Download Methods
        public static async Task FileDownloadAsync(IActionResult page,
                                          string fileFullPath,
                                          string fileName = "",
                                          string contentType = "",
                                          bool isInline = false)
        {
            var response = _httpContextAccessor.HttpContext?.Response;
            if (response == null) return;

            try
            {
                fileName = string.IsNullOrEmpty(fileName)
                    ? Path.GetFileName(fileFullPath)
                    : fileName;

                fileName = Uri.EscapeDataString(fileName).Replace("+", " ");
                contentType = string.IsNullOrEmpty(contentType)
                    ? GetMimeFromFile(fileFullPath)
                    : contentType;

                byte[] fileData = await File.ReadAllBytesAsync(fileFullPath);

                response.Clear();
                response.Headers.Clear();
                response.ContentType = contentType;
                response.Headers.Add("Content-Disposition",
                    $"{(isInline ? "inline" : "attachment")}; filename={fileName}");
                response.Headers.Add("Content-Length", fileData.Length.ToString());
                await response.Body.WriteAsync(fileData, 0, fileData.Length);
                await response.Body.FlushAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> DownloadCsvFileAsync(
            HttpContext context,
            DataView dataView,
            string fileName,
            List<string> exceptColumns = null,
            bool includeHeader = true,
            char delimiter = ',',
            string enclosure = "\"",
            string encoding = "shift_jis")
        {
            if (dataView == null || context == null) return false;

            try
            {
                // Convert DataView to CSV string
                string csvString = ConvertDataViewToCsv(dataView, exceptColumns, includeHeader, delimiter, enclosure);
                if (string.IsNullOrEmpty(csvString)) return false;

                // Encode the string to bytes using specified encoding
                byte[] data = EncodeByteArray(csvString, encoding);
                if (data == null || data.Length == 0) return false;

                // Set response headers
                context.Response.Clear();
                context.Response.Headers.Clear();
                context.Response.ContentType = "text/plain";
                context.Response.Headers.Add("Content-Disposition", $"attachment; filename={Uri.EscapeDataString(fileName)}");
                context.Response.Headers.Add("Content-Length", data.Length.ToString());

                // Write the data to the response stream
                await context.Response.Body.WriteAsync(data, 0, data.Length);
                await context.Response.Body.FlushAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Overload for DataTable
        public static async Task<bool> DownloadCsvFileAsync(
            HttpContext context,
            DataTable dataTable,
            string fileName,
            List<string> exceptColumns = null,
            bool includeHeader = true,
            char delimiter = ',',
            string enclosure = "\"",
            string encoding = "shift_jis")
        {
            if (dataTable == null) return false;
            return await DownloadCsvFileAsync(
                context,
                dataTable.DefaultView,
                fileName,
                exceptColumns,
                includeHeader,
                delimiter,
                enclosure,
                encoding);
        }

        // For compatibility with existing code, provide a synchronous version
        public static bool DownloadCsvFile(
            HttpContext context,
            DataView dataView,
            string fileName,
            List<string> exceptColumns = null,
            bool includeHeader = true,
            char delimiter = ',',
            string enclosure = "\"",
            string encoding = "shift_jis")
        {
            return DownloadCsvFileAsync(
                context,
                dataView,
                fileName,
                exceptColumns,
                includeHeader,
                delimiter,
                enclosure,
                encoding).GetAwaiter().GetResult();
        }

        // Helper method for usage in controller
        public static IActionResult DownloadCsvFileResult(
            DataView dataView,
            string fileName,
            List<string> exceptColumns = null,
            bool includeHeader = true,
            char delimiter = ',',
            string enclosure = "\"",
            string encoding = "shift_jis")
        {
            if (dataView == null) return new BadRequestResult();

            string csvString = ConvertDataViewToCsv(dataView, exceptColumns, includeHeader, delimiter, enclosure);
            if (string.IsNullOrEmpty(csvString)) return new BadRequestResult();

            byte[] data = EncodeByteArray(csvString, encoding);
            if (data == null || data.Length == 0) return new BadRequestResult();

            return new FileContentResult(data, "text/plain")
            {
                FileDownloadName = fileName
            };
        }
        #endregion

        #region Encryption/Decryption
        public static byte[] EncryptRijndael128Bit(string strToBeEncrypted)
        {
            var plainText = Encoding.Unicode.GetBytes(strToBeEncrypted);
            var sb = new StringBuilder();
            sb.Append(GlobalVariables.Rijndael128Bit.secretKey);

            var saltBuilder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                saltBuilder.Append("," + sb.Length.ToString());
            }
            var salt = Encoding.ASCII.GetBytes(saltBuilder.ToString());

            using var pwdGen = new Rfc2898DeriveBytes(sb.ToString(), salt, 10000);
            using var aes = Aes.Create();
            aes.BlockSize = 256;

            var key = pwdGen.GetBytes(aes.KeySize / 8);
            var iv = pwdGen.GetBytes(aes.BlockSize / 8);

            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainText, 0, plainText.Length);
            }
            return ms.ToArray();
        }

        // Other methods converted similarly...
        #endregion
    }
}

